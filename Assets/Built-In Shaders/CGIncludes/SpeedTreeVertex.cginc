#ifndef SPEEDTREE_VERTEX_INCLUDED
#define SPEEDTREE_VERTEX_INCLUDED

///////////////////////////////////////////////////////////////////////  
//  SpeedTree v6 Vertex Processing

///////////////////////////////////////////////////////////////////////  
//  struct SpeedTreeVB

// texcoord setup
//
//		BRANCHES					FRONDS						LEAVES
// 0	diffuse uv, branch wind xy	"							"
// 1	lod xyz, seam amount		lod xyz, 0					anchor xyz, lod scalar
// 2	detail uv, seam uv			frond wind xyz, 0			leaf wind xyz, leaf group

struct SpeedTreeVB 
{
	float4 vertex		: POSITION;
	float4 tangent		: TANGENT;
	float3 normal		: NORMAL;
	float4 texcoord		: TEXCOORD0;
	float4 texcoord1	: TEXCOORD1;
	float4 texcoord2	: TEXCOORD2;
	float2 texcoord3	: TEXCOORD3;
	half4 color			: COLOR;
};


///////////////////////////////////////////////////////////////////////  
//  SpeedTree winds

#ifdef ENABLE_WIND

#define WIND_QUALITY_NONE		0
#define WIND_QUALITY_FASTEST	1
#define WIND_QUALITY_FAST		2
#define WIND_QUALITY_BETTER		3
#define WIND_QUALITY_BEST		4
#define WIND_QUALITY_PALM		5

uniform half _WindQuality;
uniform half _WindEnabled;

#include "SpeedTreeWind.cginc"

#endif

///////////////////////////////////////////////////////////////////////  
//  OffsetSpeedTreeVertex

void OffsetSpeedTreeVertex(inout SpeedTreeVB Data, float LodValue)
{
	float3 FinalPosition = Data.vertex.xyz;
	float3 TreePos = float3(_Object2World[0].w, _Object2World[1].w, _Object2World[2].w);

	#ifdef ENABLE_WIND
		half windQuality = _WindQuality * _WindEnabled;

		float3 vRotatedWindVector, vRotatedBranchAnchor;
		if (windQuality > WIND_QUALITY_NONE)
		{
			// compute rotated wind parameters
			vRotatedWindVector = normalize(mul((float3x3)_World2Object, _ST_WindVector.xyz));
			vRotatedBranchAnchor = normalize(mul((float3x3)_World2Object, _ST_WindBranchAnchor.xyz)) * _ST_WindBranchAnchor.w;
		}
		else
		{
			vRotatedWindVector = float3(0.0f, 0.0f, 0.0f);
			vRotatedBranchAnchor = float3(0.0f, 0.0f, 0.0f);
		}
	#endif

	#if defined(GEOM_TYPE_BRANCH) || defined(GEOM_TYPE_FROND)

		// smooth LOD
		#ifdef LOD_FADE_PERCENTAGE
			FinalPosition = lerp(FinalPosition, Data.texcoord1.xyz, LodValue);
		#endif

		// frond wind, if needed
		#if defined(ENABLE_WIND) && defined(GEOM_TYPE_FROND)
			if (windQuality == WIND_QUALITY_PALM)
				FinalPosition = RippleFrond(FinalPosition, Data.normal, Data.texcoord.x, Data.texcoord.y, Data.texcoord2.x, Data.texcoord2.y, Data.texcoord2.z);
		#endif

	#elif defined(GEOM_TYPE_FACING_LEAF) || defined(GEOM_TYPE_LEAF)

		// remove anchor position
		FinalPosition -= Data.texcoord1.xyz;

		// smooth LOD
		#ifdef LOD_FADE_PERCENTAGE
			#if defined(GEOM_TYPE_FACING_LEAF)
				FinalPosition *= lerp(1.0, Data.texcoord1.w, LodValue);
			#else
				float3 LodPosition = float3(Data.texcoord1.w, Data.texcoord3.x, Data.texcoord3.y);
				FinalPosition = lerp(FinalPosition, LodPosition, LodValue);
			#endif
		#endif

		// face camera-facing leaf to camera
		#ifdef GEOM_TYPE_FACING_LEAF
			float offsetLen = length(FinalPosition);
			FinalPosition = mul(FinalPosition.xyz, (float3x3)UNITY_MATRIX_IT_MV); // inv(MV) * FinalPosition
			FinalPosition = normalize(FinalPosition) * offsetLen; // make sure the offset vector is still scaled
		#endif

		#ifdef ENABLE_WIND
			// leaf wind
			if (windQuality > WIND_QUALITY_FASTEST && windQuality < WIND_QUALITY_PALM)
			{
				float LeafWindTrigOffset = Data.texcoord1.x + Data.texcoord1.y;
				FinalPosition = LeafWind(windQuality == WIND_QUALITY_BEST, Data.texcoord2.w > 0.0, FinalPosition, Data.normal, Data.texcoord2.x, float3(0,0,0), Data.texcoord2.y, Data.texcoord2.z, LeafWindTrigOffset, vRotatedWindVector);
			}
		#endif

		// move back out to anchor
		FinalPosition += Data.texcoord1.xyz;

	#endif

	#ifdef ENABLE_WIND
		#ifndef GEOM_TYPE_MESH
			if (windQuality >= WIND_QUALITY_BETTER)
			{
				// branch wind (applies to all 3D geometry)
				FinalPosition = BranchWind(windQuality == WIND_QUALITY_PALM, FinalPosition, TreePos, float4(Data.texcoord.zw, 0, 0), vRotatedWindVector, vRotatedBranchAnchor);
			}
		#endif

		if (windQuality > WIND_QUALITY_NONE)
		{
			// global wind
			FinalPosition = GlobalWind(FinalPosition, TreePos, true, vRotatedWindVector, _ST_WindGlobal.x);
		}
	#endif

	Data.vertex.xyz = FinalPosition;
}

#endif // SPEEDTREE_VERTEX_INCLUDED
