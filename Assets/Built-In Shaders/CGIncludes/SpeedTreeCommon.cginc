#ifndef SPEEDTREE_COMMON_INCLUDED
#define SPEEDTREE_COMMON_INCLUDED

#include "UnityCG.cginc"

#define SPEEDTREE_Y_UP

#if defined(GEOM_TYPE_BRANCH_DETAIL) || defined(GEOM_TYPE_BRANCH_BLEND)
	#define GEOM_TYPE_BRANCH
#endif

#include "SpeedTreeVertex.cginc"

// Define Input structure

struct Input
{
	fixed4 color;
	half3 interpolator1;
	#if defined(GEOM_TYPE_BRANCH_DETAIL) || defined(GEOM_TYPE_BRANCH_BLEND)
		half3 interpolator2;
	#endif
	#ifdef LOD_FADE_CROSSFADE
		half3 myScreenPos;
	#endif
};
	
// Define uniforms

#define mainTexUV interpolator1.xy
uniform sampler2D _MainTex;

#ifdef GEOM_TYPE_BRANCH_DETAIL
	#define Detail interpolator2.xy
	uniform sampler2D _DetailTex;
#endif

#ifdef GEOM_TYPE_BRANCH_BLEND
	#define BranchBlend interpolator2
#endif

#if defined(GEOM_TYPE_FROND) || defined(GEOM_TYPE_LEAF) || defined(GEOM_TYPE_FACING_LEAF)
	#define SPEEDTREE_ALPHATEST
	uniform fixed _Cutoff;
#endif

#ifdef EFFECT_HUE_VARIATION
	#define HueVariationAmount interpolator1.z
	uniform half4 _HueVariation;
#endif

#ifdef EFFECT_BUMP
	uniform sampler2D _BumpMap;
#endif

#ifdef LOD_FADE_CROSSFADE
	uniform sampler2D _DitherMaskLOD2D;
#endif

uniform fixed4 _Color;
uniform half _Shininess;

// Vertex processing

void SpeedTreeVert(inout SpeedTreeVB IN, out Input OUT)
{
	UNITY_INITIALIZE_OUTPUT(Input, OUT);

	OUT.mainTexUV = IN.texcoord.xy;
	OUT.color = _Color;
	OUT.color.rgb *= IN.color.r; // ambient occlusion factor

	#ifdef EFFECT_HUE_VARIATION
		float hueVariationAmount = frac(_Object2World[0].w + _Object2World[1].w + _Object2World[2].w);
		hueVariationAmount += frac(IN.vertex.x + IN.normal.y + IN.normal.x) * 0.5 - 0.3;
		OUT.HueVariationAmount = saturate(hueVariationAmount * _HueVariation.a);
	#endif

	#ifdef GEOM_TYPE_BRANCH_DETAIL
		OUT.Detail = IN.texcoord2.xy;
	#endif

	#ifdef GEOM_TYPE_BRANCH_BLEND
		OUT.BranchBlend = float3(IN.texcoord2.zw, IN.texcoord1.w);
	#endif

	OffsetSpeedTreeVertex(IN, unity_LODFade.x);

	#ifdef LOD_FADE_CROSSFADE
		float4 pos = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.myScreenPos = ComputeScreenPos(pos).xyw;
		OUT.myScreenPos.xy *= _ScreenParams.xy * 0.25;
	#endif
}

// Fragment processing

#ifdef EFFECT_BUMP
	#define SPEEDTREE_DATA_NORMAL			fixed3 Normal;
	#define SPEEDTREE_COPY_NORMAL(to, from)	to.Normal = from.Normal;
#else
	#define SPEEDTREE_DATA_NORMAL
	#define SPEEDTREE_COPY_NORMAL(to, from)
#endif

#define SPEEDTREE_COPY_FRAG(to, from)	\
	to.Albedo = from.Albedo;			\
	to.Alpha = from.Alpha;				\
	to.Specular = from.Specular;		\
	to.Gloss = from.Gloss;				\
	SPEEDTREE_COPY_NORMAL(to, from)

struct SpeedTreeFragOut
{
	fixed3 Albedo;
	fixed Alpha;
	half Specular;
	fixed Gloss;
	SPEEDTREE_DATA_NORMAL
};

void SpeedTreeFrag(Input IN, out SpeedTreeFragOut OUT)
{
	#ifdef LOD_FADE_CROSSFADE
		half2 projUV = IN.myScreenPos.xy / IN.myScreenPos.z;
		projUV.y = frac(projUV.y) * 0.0625 /* 1/16 */ + unity_LODFade.y; // quantized lod fade by 16 levels
		clip(tex2D(_DitherMaskLOD2D, projUV).a - 0.5);
	#endif

	half4 diffuseColor = tex2D(_MainTex, IN.mainTexUV);

	OUT.Alpha = diffuseColor.a * _Color.a;
	#ifdef SPEEDTREE_ALPHATEST
		clip(OUT.Alpha - _Cutoff);
	#endif

	#ifdef GEOM_TYPE_BRANCH_DETAIL
		half4 detailColor = tex2D(_DetailTex, IN.Detail);
		diffuseColor.rgb = lerp(diffuseColor.rgb, detailColor.rgb, detailColor.a);
	#endif

	#ifdef GEOM_TYPE_BRANCH_BLEND
		half4 blendColor = tex2D(_MainTex, IN.BranchBlend.xy);
		half amount = saturate(IN.BranchBlend.z);
		diffuseColor.rgb = lerp(blendColor.rgb, diffuseColor.rgb, amount);
	#endif

	#ifdef EFFECT_HUE_VARIATION
		half3 shiftedColor = lerp(diffuseColor.rgb, _HueVariation.rgb, IN.HueVariationAmount);
		half maxBase = max(diffuseColor.r, max(diffuseColor.g, diffuseColor.b));
		half newMaxBase = max(shiftedColor.r, max(shiftedColor.g, shiftedColor.b));
		maxBase /= newMaxBase;
		maxBase = maxBase * 0.5f + 0.5f;
		// preserve vibrance
		shiftedColor.rgb *= maxBase;
		diffuseColor.rgb = saturate(shiftedColor);
	#endif

	OUT.Albedo = diffuseColor.rgb * IN.color.rgb;
	OUT.Gloss = diffuseColor.a;
	OUT.Specular = _Shininess;

	#ifdef EFFECT_BUMP
		OUT.Normal = UnpackNormal(tex2D(_BumpMap, IN.mainTexUV));
	#endif
}

#endif // SPEEDTREE_COMMON_INCLUDED
