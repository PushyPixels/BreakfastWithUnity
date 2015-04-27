Shader "Nature/SpeedTree Billboard"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
		_HueVariation ("Hue Variation", Color) = (1.0,0.5,0.0,0.1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialEnum(None,0,Fastest,1)] _WindQuality ("Wind Quality", Range(0,1)) = 0

		[HideInInspector] _TreeInfo0 ("TreeInfo0", Vector) = (0,0,0,0)
		[HideInInspector] _TreeInfo1 ("TreeInfo1", Vector) = (0,0,0,0)
		[HideInInspector] _TreeInfo2 ("TreeInfo2", Vector) = (0,0,0,0)
		[HideInInspector] _TreeInfo3 ("TreeInfo3", Vector) = (0,0,0,0)
		[HideInInspector] _TreeSize0 ("TreeSize0", Vector) = (0,0,0,0)
		[HideInInspector] _TreeSize1 ("TreeSize1", Vector) = (0,0,0,0)
		[HideInInspector] _TreeSize2 ("TreeSize2", Vector) = (0,0,0,0)
		[HideInInspector] _TreeSize3 ("TreeSize3", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords0 ("ImageTexCoords0", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords1 ("ImageTexCoords1", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords2 ("ImageTexCoords2", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords3 ("ImageTexCoords3", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords4 ("ImageTexCoords4", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords5 ("ImageTexCoords5", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords6 ("ImageTexCoords6", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords7 ("ImageTexCoords7", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords8 ("ImageTexCoords8", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords9 ("ImageTexCoords9", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords10 ("ImageTexCoords10", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords11 ("ImageTexCoords11", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords12 ("ImageTexCoords12", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords13 ("ImageTexCoords13", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords14 ("ImageTexCoords14", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords15 ("ImageTexCoords15", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords16 ("ImageTexCoords16", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords17 ("ImageTexCoords17", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords18 ("ImageTexCoords18", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords19 ("ImageTexCoords19", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords20 ("ImageTexCoords20", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords21 ("ImageTexCoords21", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords22 ("ImageTexCoords22", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords23 ("ImageTexCoords23", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords24 ("ImageTexCoords24", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords25 ("ImageTexCoords25", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords26 ("ImageTexCoords26", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords27 ("ImageTexCoords27", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords28 ("ImageTexCoords28", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords29 ("ImageTexCoords29", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords30 ("ImageTexCoords30", Vector) = (0,0,0,0)
		[HideInInspector] _ImageTexCoords31 ("ImageTexCoords31", Vector) = (0,0,0,0)
		[HideInInspector] _InstanceData ("InstanceData", Vector) = (0,0,0,0)
	}

	// targeting SM3.0+
	SubShader
	{
		Tags
		{
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="TransparentCutout"
			"DisableBatching"="LODFading"
		}
		LOD 400
		Cull Off

		CGPROGRAM
			#pragma surface surf Lambert vertex:SpeedTreeBillboardVert nolightmap
			#pragma target 3.0
			#pragma multi_compile __ LOD_FADE_CROSSFADE
			#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
			#pragma shader_feature EFFECT_BUMP
			#pragma shader_feature EFFECT_HUE_VARIATION
			#define ENABLE_WIND
			#include "SpeedTreeBillboardCommon.cginc"

			void surf(Input IN, inout SurfaceOutput OUT)
			{
				SpeedTreeFragOut o;
				SpeedTreeFrag(IN, o);
				SPEEDTREE_COPY_FRAG(OUT, o)
			}
		ENDCG

		Pass
		{
			Tags { "LightMode" = "Vertex" }

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_fog
				#pragma multi_compile __ LOD_FADE_CROSSFADE
				#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
				#pragma shader_feature EFFECT_HUE_VARIATION
				#define ENABLE_WIND
				#include "SpeedTreeBillboardCommon.cginc"

				struct v2f 
				{
					float4 vertex	: SV_POSITION;
					UNITY_FOG_COORDS(0)
					Input data		: TEXCOORD1;
				};

				v2f vert(SpeedTreeBillboardData v)
				{
					v2f o;
					SpeedTreeBillboardVert(v, o.data);
					o.data.color.rgb *= ShadeVertexLightsFull(v.vertex, v.normal, 4, true);
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					SpeedTreeFragOut o;
					SpeedTreeFrag(i.data, o);
					fixed4 c = fixed4(o.Albedo, o.Alpha);
					UNITY_APPLY_FOG(i.fogCoord, c);
					return c;
				}
			ENDCG
		}
	}

	// targeting SM2.0: Cross-fading, Hue variation and Camera-facing billboard are turned off for less instructions
	SubShader
	{
		Tags
		{
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="TransparentCutout"
		}
		LOD 400
		Cull Off

		CGPROGRAM
			#pragma surface surf Lambert vertex:SpeedTreeBillboardVert nolightmap
			#pragma shader_feature EFFECT_BUMP
			#include "SpeedTreeBillboardCommon.cginc"

			void surf(Input IN, inout SurfaceOutput OUT)
			{
				SpeedTreeFragOut o;
				SpeedTreeFrag(IN, o);
				SPEEDTREE_COPY_FRAG(OUT, o)
			}
		ENDCG

		Pass
		{
			Tags { "LightMode" = "Vertex" }

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "SpeedTreeBillboardCommon.cginc"

				struct v2f 
				{
					float4 vertex	: SV_POSITION;
					UNITY_FOG_COORDS(0)
					Input data		: TEXCOORD1;
				};

				v2f vert(SpeedTreeBillboardData v)
				{
					v2f o;
					SpeedTreeBillboardVert(v, o.data);
					o.data.color.rgb *= ShadeVertexLightsFull(v.vertex, v.normal, 2, false);
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					SpeedTreeFragOut o;
					SpeedTreeFrag(i.data, o);
					fixed4 c = fixed4(o.Albedo, o.Alpha);
					UNITY_APPLY_FOG(i.fogCoord, c);
					return c;
				}
			ENDCG
		}
	}

	FallBack "Transparent/Cutout/VertexLit"
}
