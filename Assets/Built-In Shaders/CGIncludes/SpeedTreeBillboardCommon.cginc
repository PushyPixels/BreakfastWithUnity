#ifndef SPEEDTREE_BILLBOARD_COMMON_INCLUDED
#define SPEEDTREE_BILLBOARD_COMMON_INCLUDED

#define SPEEDTREE_ALPHATEST
uniform fixed _Cutoff;

#include "SpeedTreeCommon.cginc"

uniform float3 _BillboardNormal;
uniform float3 _BillboardTangent;
uniform float _CameraXZAngle;

uniform float4 _TreeInfo[4];			// x: num of billboard slices; y: 1.0f / (delta angle between slices)
uniform float4 _TreeSize[4];
uniform float4 _ImageTexCoords[32];

uniform float4 _InstanceData;

struct SpeedTreeBillboardData
{
	float4 vertex		: POSITION;
	float2 texcoord		: TEXCOORD0;
	float4 texcoord1	: TEXCOORD1;
	float3 normal		: NORMAL;
	float4 tangent		: TANGENT;
	float4 color		: COLOR;
};

void SpeedTreeBillboardVert(inout SpeedTreeBillboardData IN, out Input OUT)
{
	UNITY_INITIALIZE_OUTPUT(Input, OUT);

	float treeType = IN.color.a * 255.0f;
	float4 treeInfo = _TreeInfo[treeType];
	float4 treeSize = _TreeSize[treeType];

	// assume no scaling & rotation
	float3 worldPos = IN.vertex.xyz + float3(_Object2World[0].w, _Object2World[1].w, _Object2World[2].w);

#ifdef BILLBOARD_FACE_CAMERA_POS
	float3 eyeVec = normalize(_WorldSpaceCameraPos - worldPos);
	float3 billboardTangent = normalize(float3(-eyeVec.z, 0, eyeVec.x));			// cross(eyeVec, {0,1,0})
	float3 billboardNormal = float3(billboardTangent.z, 0, -billboardTangent.x);	// cross({0,1,0},billboardTangent)
	float3 angle = atan2(billboardNormal.z, billboardNormal.x);						// signed angle between billboardNormal to {0,0,1}
	angle += angle < 0 ? 2 * UNITY_PI : 0;
#else
	float3 billboardTangent = _BillboardTangent;
	float3 billboardNormal = _BillboardNormal;
	float angle = _CameraXZAngle;
#endif

	float4 instanceData = _InstanceData.x > 0 ? _InstanceData.xyzw : IN.texcoord1.xyzw;
	float widthScale = instanceData.x;
	float heightScale = instanceData.y;
	float rotation = instanceData.z;

	float2 percent = IN.texcoord.xy;
	float3 billboardPos = (percent.x - 0.5f) * treeSize.x * widthScale * billboardTangent;
	billboardPos.y += (percent.y * treeSize.y + treeSize.z) * heightScale;

#ifdef ENABLE_WIND
	if (_WindQuality * _WindEnabled > 0)
		billboardPos = GlobalWind(billboardPos, worldPos, true, _ST_WindVector.xyz, instanceData.w);
#endif

	IN.vertex.xyz += billboardPos;
	IN.vertex.w = 1.0f;
	IN.normal = billboardNormal.xyz;
	IN.tangent = float4(billboardTangent.xyz,-1);

	float slices = treeInfo.x;
	float invDelta = treeInfo.y;
	angle += rotation;

	float imageIndex = fmod(floor(angle * invDelta + 0.5f), slices);
	float4 imageTexCoords = _ImageTexCoords[treeInfo.z + imageIndex];
	if (imageTexCoords.w < 0)
	{
		OUT.mainTexUV = imageTexCoords.xy - imageTexCoords.zw * percent.yx;
	}
	else
	{
		OUT.mainTexUV = imageTexCoords.xy + imageTexCoords.zw * percent;
	}

	OUT.color = _Color;

#ifdef EFFECT_HUE_VARIATION
	float hueVariationAmount = frac(worldPos.x + worldPos.y + worldPos.z);
	OUT.HueVariationAmount = saturate(hueVariationAmount * _HueVariation.a);
#endif

#ifdef LOD_FADE_CROSSFADE
	float4 pos = mul (UNITY_MATRIX_MVP, IN.vertex);
	OUT.myScreenPos = ComputeScreenPos(pos).xyw;
	OUT.myScreenPos.xy *= _ScreenParams.xy * 0.25f;
#endif
}

#endif // SPEEDTREE_BILLBOARD_COMMON_INCLUDED
