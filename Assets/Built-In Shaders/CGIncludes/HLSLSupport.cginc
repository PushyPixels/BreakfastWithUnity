// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECT' with 'tex2D'
// Upgrade NOTE: replaced 'texRECTbias' with 'tex2Dbias'
// Upgrade NOTE: replaced 'texRECTlod' with 'tex2Dlod'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

#ifndef HLSL_SUPPORT_INCLUDED
#define HLSL_SUPPORT_INCLUDED


#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOX360) || defined(SHADER_API_D3D11_9X)
#define UNITY_COMPILER_HLSL
#elif defined(SHADER_TARGET_GLSL)
#define UNITY_COMPILER_HLSL2GLSL
#else
#define UNITY_COMPILER_CG
#endif

#if !defined(SV_Target)
#	if defined(SHADER_API_PSSL)
#		define SV_Target S_TARGET_OUTPUT
#	elif !defined(SHADER_API_XBOXONE)
#		define SV_Target COLOR
#	endif
#endif

#if !defined(SV_Depth)
#	if defined(SHADER_API_PSSL)
#		define SV_Depth S_DEPTH_OUTPUT
#	elif !defined(SHADER_API_XBOXONE)
#		define SV_Depth DEPTH
#	endif
#endif

#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOX360) || defined(SHADER_API_D3D11_9X)
#pragma warning (disable : 3205) // conversion of larger type to smaller
#pragma warning (disable : 3568) // unknown pragma ignored
#endif

#if !defined(SHADER_TARGET_GLSL) && !defined(SHADER_API_PSSL)
#define fixed half
#define fixed2 half2
#define fixed3 half3
#define fixed4 half4
#define fixed4x4 half4x4
#define fixed3x3 half3x3
#define fixed2x2 half2x2
#define sampler2D_half sampler2D
#define sampler2D_float sampler2D
#define samplerCUBE_half samplerCUBE
#define samplerCUBE_float samplerCUBE
#endif

#if defined(SHADER_API_PSSL)
#define uniform
#define half float
#define half2 float2
#define half3 float3
#define half4 float4
#define half2x2 float2x2
#define half3x3 float3x3
#define half4x4 float4x4
#define fixed float
#define fixed2 float2
#define fixed3 float3
#define fixed4 float4
#define fixed4x4 half4x4
#define fixed3x3 half3x3
#define fixed2x2 half2x2

#define CBUFFER_START(name) ConstantBuffer name {
#define CBUFFER_END };
#elif defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
#define CBUFFER_START(name) cbuffer name {
#define CBUFFER_END };
#else
#define CBUFFER_START(name)
#define CBUFFER_END
#endif

#if defined(SHADOWS_NATIVE) && defined(SHADER_API_PSSL)
	#define UNITY_DECLARE_SHADOWMAP(tex)		Texture2D tex; SamplerComparisonState sampler##tex
	#define UNITY_SAMPLE_SHADOW(tex,coord)		tex.SampleCmpLOD0(sampler##tex,(coord).xy,(coord).z)
	#define UNITY_SAMPLE_SHADOW_PROJ(tex,coord)	tex.SampleCmpLOD0(sampler##tex,(coord).xy/(coord).w,(coord).z/(coord).w)
#elif defined(SHADOWS_NATIVE) && (defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X))
	// DX11 syntax for shadow maps
	#define UNITY_DECLARE_SHADOWMAP(tex) Texture2D tex; SamplerComparisonState sampler##tex
	#define UNITY_SAMPLE_SHADOW(tex,coord) tex.SampleCmpLevelZero (sampler##tex,(coord).xy,(coord).z)
	#define UNITY_SAMPLE_SHADOW_PROJ(tex,coord) tex.SampleCmpLevelZero (sampler##tex,(coord).xy/(coord).w,(coord).z/(coord).w)
#elif defined(SHADOWS_NATIVE) && defined(SHADER_TARGET_GLSL)
	// hlsl2glsl syntax for shadow maps
	#define UNITY_DECLARE_SHADOWMAP(tex) sampler2DShadow tex
	#define UNITY_SAMPLE_SHADOW(tex,coord) shadow2D (tex,(coord).xyz)
	#define UNITY_SAMPLE_SHADOW_PROJ(tex,coord) shadow2Dproj (tex,coord)
#elif defined(SHADOWS_NATIVE) && defined(SHADER_API_PSP2) && !defined(SHADER_API_PSM)
	#define UNITY_DECLARE_SHADOWMAP(tex) sampler2D tex
	#define UNITY_SAMPLE_SHADOW(tex,coord) f1tex2D<float>(tex, (coord).xyz)
	#define UNITY_SAMPLE_SHADOW_PROJ(tex,coord) f1tex2Dproj<float>(tex, coord)
#else
	// Cg syntax for shadow maps
	#define UNITY_DECLARE_SHADOWMAP(tex) sampler2D tex
	#define UNITY_SAMPLE_SHADOW(tex,coord) tex2D (tex,(coord).xyz).r
	#define UNITY_SAMPLE_SHADOW_PROJ(tex,coord) tex2Dproj (tex,coord).r
#endif



#define sampler2D sampler2D
#define tex2D tex2D
#define tex2Dlod tex2Dlod
#define tex2Dbias tex2Dbias
#define tex2Dproj tex2Dproj

#if defined(SHADER_API_PSSL)
#define VPOS			S_POSITION
#elif defined(UNITY_COMPILER_CG)
// Cg seems to use WPOS instead of VPOS semantic?
#define VPOS WPOS
// Cg does not have tex2Dgrad and friends, but has tex2D overload that
// can take the derivatives
#define tex2Dgrad tex2D
#define texCUBEgrad texCUBE
#define tex3Dgrad tex3D
#endif

#if !defined(SHADER_API_XBOX360) && !defined(SHADER_API_PS3) && !defined(SHADER_API_GLES) && !defined(SHADER_API_GLES3) && !defined(SHADER_TARGET_GLSL) && !defined(SHADER_API_D3D11) && !defined(SHADER_API_D3D11_9X) && !defined(SHADER_API_PSP2)
#define UNITY_HAS_LIGHT_PARAMETERS 1
#endif

#if defined(SHADER_API_PSSL)

struct sampler1D {
	Texture1D		t;
	SamplerState	s;
};
struct sampler2D {
	Texture2D		t;
	SamplerState	s;
};
struct sampler3D {
	Texture3D		t;
	SamplerState	s;
};
struct samplerCUBE {
	TextureCube		t;
	SamplerState	s;
};

float4 tex1D(sampler1D x, float v)				{ return x.t.Sample(x.s, v); }
float4 tex2D(sampler2D x, float2 v)				{ return x.t.Sample(x.s, v); }
float4 tex3D(sampler3D x, float3 v)				{ return x.t.Sample(x.s, v); }
float4 texCUBE(samplerCUBE x, float3 v)			{ return x.t.Sample(x.s, v); }

float4 tex1Dbias(sampler1D x, in float4 t)		{ return x.t.SampleBias(x.s, t.x, t.w); }
float4 tex2Dbias(sampler2D x, in float4 t)		{ return x.t.SampleBias(x.s, t.xy, t.w); }
float4 tex3Dbias(sampler3D x, in float4 t)		{ return x.t.SampleBias(x.s, t.xyz, t.w); }
float4 texCUBEbias(samplerCUBE x, in float4 t)	{ return x.t.SampleBias(x.s, t.xyz, t.w); }

float4 tex1Dlod(sampler1D x, in float4 t)		{ return x.t.SampleLOD(x.s, t.x, t.w); }
float4 tex2Dlod(sampler2D x, in float4 t)		{ return x.t.SampleLOD(x.s, t.xy, t.w); }
float4 tex3Dlod(sampler3D x, in float4 t)		{ return x.t.SampleLOD(x.s, t.xyz, t.w); }
float4 texCUBElod(samplerCUBE x, in float4 t)	{ return x.t.SampleLOD(x.s, t.xyz, t.w); }

float4 tex1Dgrad(sampler1D x, float t, float dx, float dy)			{ return x.t.SampleGradient(x.s, t, dx, dy); }
float4 tex2Dgrad(sampler2D x, float2 t, float2 dx, float2 dy)		{ return x.t.SampleGradient(x.s, t, dx, dy); }
float4 tex3Dgrad(sampler3D x, float3 t, float3 dx, float3 dy)		{ return x.t.SampleGradient(x.s, t, dx, dy); }
float4 texCUBEgrad(samplerCUBE x, float3 t, float3 dx, float3 dy)	{ return x.t.SampleGradient(x.s, t, dx, dy); }

float4 tex1Dproj(sampler1D s, in float2 t)		{ return tex1D(s, t.x / t.y); }
float4 tex1Dproj(sampler1D s, in float4 t)		{ return tex1D(s, t.x / t.w); }
float4 tex2Dproj(sampler2D s, in float3 t)		{ return tex2D(s, t.xy / t.z); }
float4 tex2Dproj(sampler2D s, in float4 t)		{ return tex2D(s, t.xy / t.w); }
float4 tex3Dproj(sampler3D s, in float4 t)		{ return tex3D(s, t.xyz / t.w); }
float4 texCUBEproj(samplerCUBE s, in float4 t)	{ return texCUBE(s, t.xyz / t.w); }

#elif defined(SHADER_API_XBOX360)

float4 tex2Dproj(in sampler2D s, in float4 t) 
{ 
	float2 ti=t.xy / t.w;
	return tex2D( s, ti);
}

float4 tex2Dproj(in sampler2D s, in float3 t) 
{ 
	float2 ti=t.xy / t.z;
	return tex2D( s, ti);
}


#endif

#if defined(SHADER_API_XBOX360) || defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined (SHADER_TARGET_GLSL)
#define FOGC FOG
#endif

// Use VFACE pixel shader input semantic in your shaders to get front-facing scalar value.
#if defined(UNITY_COMPILER_CG)
#define VFACE FACE
#endif
#if defined(UNITY_COMPILER_HLSL2GLSL)
#define FACE VFACE
#endif

#if defined(SHADER_API_PSSL)
#define SV_POSITION S_POSITION
#elif !defined(SHADER_API_D3D11) && !defined(SHADER_API_D3D11_9X)
#define SV_POSITION POSITION
#endif


#if defined(SHADER_API_D3D9) || defined(SHADER_API_XBOX360) || defined(SHADER_API_PS3) || defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_PSP2) || defined(SHADER_API_PSSL)
#define UNITY_ATTEN_CHANNEL r
#else
#define UNITY_ATTEN_CHANNEL a
#endif

#if defined(SHADER_API_D3D9) || defined(SHADER_API_XBOX360) || defined(SHADER_API_PSP2)
#define UNITY_HALF_TEXEL_OFFSET
#endif

#if defined(SHADER_API_D3D9) || defined(SHADER_API_XBOX360) || defined(SHADER_API_PS3) || defined(SHADER_API_FLASH) || defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_PSP2) || defined(SHADER_API_PSSL)
#define UNITY_UV_STARTS_AT_TOP 1
#endif

#if defined(SHADER_API_D3D9) || defined(SHADER_API_XBOX360) || defined(SHADER_API_PS3) || defined(SHADER_API_FLASH) || defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
#define UNITY_NEAR_CLIP_VALUE (0.0)
#else
#define UNITY_NEAR_CLIP_VALUE (-1.0)
#endif


#if defined(SHADER_API_D3D9) || defined (SHADER_API_FLASH)
#define UNITY_MIGHT_NOT_HAVE_DEPTH_TEXTURE
#endif


#if (defined(SHADER_API_OPENGL) && !defined(SHADER_TARGET_GLSL)) || defined(SHADER_API_PSP2)
#define UNITY_BUGGY_TEX2DPROJ4
#define UNITY_PROJ_COORD(a) (a).xyw
#else
#define UNITY_PROJ_COORD(a) a
#endif


#if defined(UNITY_COMPILER_HLSL)
#define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
#else
#define UNITY_INITIALIZE_OUTPUT(type,name)
#endif

#if defined(SHADER_API_D3D11)
#define UNITY_CAN_COMPILE_TESSELLATION 1
#	define UNITY_domain					domain
#	define UNITY_partitioning			partitioning
#	define UNITY_outputtopology			outputtopology
#	define UNITY_patchconstantfunc		patchconstantfunc
#	define UNITY_outputcontrolpoints	outputcontrolpoints
#elif defined(SHADER_API_PSSL)
#	define UNITY_CAN_COMPILE_TESSELLATION 1

#	define SV_OutputControlPointID		S_OUTPUT_CONTROL_POINT_ID
#	define SV_TessFactor				S_EDGE_TESS_FACTOR
#	define SV_InsideTessFactor			S_INSIDE_TESS_FACTOR
#	define SV_DomainLocation			S_DOMAIN_LOCATION

#	define UNITY_domain					DOMAIN_PATCH_TYPE
#	define UNITY_partitioning			PARTITIONING_TYPE
#	define UNITY_outputtopology			OUTPUT_TOPOLOGY_TYPE
#	define UNITY_patchconstantfunc		PATCH_CONSTANT_FUNC
#	define UNITY_outputcontrolpoints	OUTPUT_CONTROL_POINTS
#endif

// Not really needed anymore, but did ship in Unity 4.0; with D3D11_9X remapping them to .r channel.
// Now that's not used.
#define UNITY_SAMPLE_1CHANNEL(x,y) tex2D(x,y).a
#define UNITY_ALPHA_CHANNEL a


#endif
