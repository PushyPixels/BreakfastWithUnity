#ifndef UNITY_STANDARD_BRDF_INCLUDED
#define UNITY_STANDARD_BRDF_INCLUDED

#include "UnityCG.cginc"
#include "UnityStandardConfig.cginc"
#include "UnityLightingCommon.cginc"

//-------------------------------------------------------------------------------------
half4 unity_LightGammaCorrectionConsts;
#define unity_LightGammaCorrectionConsts_PIDiv4 (unity_LightGammaCorrectionConsts.x)
#define unity_LightGammaCorrectionConsts_HalfDivPI (unity_LightGammaCorrectionConsts.y)
#define unity_LightGammaCorrectionConsts_8 (unity_LightGammaCorrectionConsts.z)
#define unity_LightGammaCorrectionConsts_SqrtHalfPI (unity_LightGammaCorrectionConsts.w)

inline half DotClamped (half3 a, half3 b)
{
	#if (SHADER_TARGET < 30)
		return saturate(dot(a, b));
	#else
		return max(0.0f, dot(a, b));
	#endif
}

inline half Pow4 (half x)
{
	return x*x*x*x;
}

inline half2 Pow4 (half2 x)
{
	return x*x*x*x;
}

inline half3 Pow4 (half3 x)
{
	return x*x*x*x;
}

inline half4 Pow4 (half4 x)
{
	return x*x*x*x;
}

// Pow5 uses the same amount of instructions as generic pow(), but has 2 advantages:
// 1) better instruction pipelining
// 2) no need to worry about NaNs
inline half Pow5 (half x)
{
	return x*x * x*x * x;
}

inline half2 Pow5 (half2 x)
{
	return x*x * x*x * x;
}

inline half3 Pow5 (half3 x)
{
	return x*x * x*x * x;
}

inline half4 Pow5 (half4 x)
{
	return x*x * x*x * x;
}

inline half LambertTerm (half3 normal, half3 lightDir)
{
	return DotClamped (normal, lightDir);
}

inline half BlinnTerm (half3 normal, half3 halfDir)
{
	return DotClamped (normal, halfDir);
}

inline half3 FresnelTerm (half3 F0, half cosA)
{
	half t = Pow5 (1 - cosA);	// ala Schlick interpoliation
	return F0 + (1-F0) * t;
}
inline half3 FresnelLerp (half3 F0, half3 F90, half cosA)
{
	half t = Pow5 (1 - cosA);	// ala Schlick interpoliation
	return lerp (F0, F90, t);
}
// approximage Schlick with ^4 instead of ^5
inline half3 FresnelLerpFast (half3 F0, half3 F90, half cosA)
{
	half t = Pow4 (1 - cosA);
	return lerp (F0, F90, t);
}
inline half3 LazarovFresnelTerm (half3 F0, half roughness, half cosA)
{
	half t = Pow5 (1 - cosA);	// ala Schlick interpoliation
	t /= 4 - 3 * roughness;
	return F0 + (1-F0) * t;
}
inline half3 SebLagardeFresnelTerm (half3 F0, half roughness, half cosA)
{
	half t = Pow5 (1 - cosA);	// ala Schlick interpoliation
	return F0 + (max (F0, roughness) - F0) * t;
}

// NOTE: Visibility term here is the full form from Torrance-Sparrow model, it includes Geometric term: V = G / (N.L * N.V)
// This way it is easier to swap Geometric terms and more room for optimizations (except maybe in case of CookTorrance geom term)

// Cook-Torrance visibility term, doesn't take roughness into account
inline half CookTorranceVisibilityTerm (half NdotL, half NdotV,  half NdotH, half VdotH)
{
	VdotH += 1e-5f;
	half G = min (1.0, min (
		(2.0 * NdotH * NdotV) / VdotH,
		(2.0 * NdotH * NdotL) / VdotH));
	return G / (NdotL * NdotV + 1e-4f);
}

// Kelemen-Szirmay-Kalos is an approximation to Cook-Torrance visibility term
// http://sirkan.iit.bme.hu/~szirmay/scook.pdf
inline half KelemenVisibilityTerm (half LdotH)
{
	return 1.0 / (LdotH * LdotH);
}

// Modified Kelemen-Szirmay-Kalos which takes roughness into account, based on: http://www.filmicworlds.com/2014/04/21/optimizing-ggx-shaders-with-dotlh/ 
inline half ModifiedKelemenVisibilityTerm (half LdotH, half roughness)
{
	// c = sqrt(2 / Pi)
	half c = unity_LightGammaCorrectionConsts_SqrtHalfPI;
	half k = roughness * roughness * c;
	half gH = LdotH * (1-k) + k;
	return 1.0 / (gH * gH);
}

// Generic Smith-Schlick visibility term
inline half SmithVisibilityTerm (half NdotL, half NdotV, half k)
{
	half gL = NdotL * (1-k) + k;
	half gV = NdotV * (1-k) + k;
	return 1.0 / (gL * gV + 1e-4f);
}

// Smith-Schlick derived for Beckmann
inline half SmithBeckmannVisibilityTerm (half NdotL, half NdotV, half roughness)
{
	// c = sqrt(2 / Pi)
	half c = unity_LightGammaCorrectionConsts_SqrtHalfPI;
	half k = roughness * roughness * c;
	return SmithVisibilityTerm (NdotL, NdotV, k);
}

// Smith-Schlick derived for GGX 
inline half SmithGGXVisibilityTerm (half NdotL, half NdotV, half roughness)
{
	half k = (roughness * roughness) / 2; // derived by B. Karis, http://graphicrants.blogspot.se/2013/08/specular-brdf-reference.html
	return SmithVisibilityTerm (NdotL, NdotV, k);
}

inline half ImplicitVisibilityTerm ()
{
	return 1;
}

inline half RoughnessToSpecPower (half roughness)
{
#if UNITY_GLOSS_MATCHES_MARMOSET_TOOLBAG2
	// from https://s3.amazonaws.com/docs.knaldtech.com/knald/1.0.0/lys_power_drops.html
	half n = 10.0 / log2((1-roughness)*0.968 + 0.03);
#if defined(SHADER_API_PS3)
	n = max(n,-255.9370);  //i.e. less than sqrt(65504)
#endif
	return n * n;

	// NOTE: another approximate approach to match Marmoset gloss curve is to
	// multiply roughness by 0.7599 in the code below (makes SpecPower range 4..N instead of 1..N)
#else
	half m = roughness * roughness * roughness + 1e-4f;	// follow the same curve as unity_SpecCube
	half n = (2.0 / m) - 2.0;							// http://jbit.net/%7Esparky/academic/mm_brdf.pdf
	n = max(n, 1e-4f);									// prevent possible cases of pow(0,0), which could happen when roughness is 1.0 and NdotH is zero
	return n;
#endif
}

// BlinnPhong normalized as normal distribution function (NDF)
// for use in micro-facet model: spec=D*G*F
// http://www.thetenthplanet.de/archives/255
inline half NDFBlinnPhongNormalizedTerm (half NdotH, half n)
{
	// norm = (n+1)/(2*pi)
	half normTerm = (n + 1.0) * unity_LightGammaCorrectionConsts_HalfDivPI;

	half specTerm = pow (NdotH, n);
	return specTerm * normTerm;
}

// BlinnPhong normalized as reflec­tion den­sity func­tion (RDF)
// ready for use directly as specular: spec=D
// http://www.thetenthplanet.de/archives/255
inline half RDFBlinnPhongNormalizedTerm (half NdotH, half n)
{
	half normTerm = (n + 2.0) / (8.0 * UNITY_PI);
	half specTerm = pow (NdotH, n);
	return specTerm * normTerm;
}

inline half GGXTerm (half NdotH, half roughness)
{
	half a = roughness * roughness;
	half a2 = a * a;
	half d = NdotH * NdotH * (a2 - 1.f) + 1.f;
	return a2 / (UNITY_PI * d * d);
}

//-------------------------------------------------------------------------------------
/*
// https://s3.amazonaws.com/docs.knaldtech.com/knald/1.0.0/lys_power_drops.html

const float k0 = 0.00098, k1 = 0.9921;
// pass this as a constant for optimization
const float fUserMaxSPow = 100000; // sqrt(12M)
const float g_fMaxT = ( exp2(-10.0/fUserMaxSPow) - k0)/k1;
float GetSpecPowToMip(float fSpecPow, int nMips)
{
   // Default curve - Inverse of TB2 curve with adjusted constants
   float fSmulMaxT = ( exp2(-10.0/sqrt( fSpecPow )) - k0)/k1;
   return float(nMips-1)*(1.0 - clamp( fSmulMaxT/g_fMaxT, 0.0, 1.0 ));
}

	//float specPower = RoughnessToSpecPower (roughness);
	//float mip = GetSpecPowToMip (specPower, 7);
*/

// Decodes HDR textures
// handles dLDR, RGBM formats
// Modified version of DecodeHDR from UnityCG.cginc
inline half3 DecodeHDR_NoLinearSupportInSM2 (half4 data, half4 decodeInstructions)
{
	// If Linear mode is not supported we can skip exponent part

	// In Standard shader SM2.0 and SM3.0 paths are always using different shader variations
	// SM2.0: hardware does not support Linear, we can skip exponent part
	#if defined(UNITY_NO_LINEAR_COLORSPACE) && (SHADER_TARGET < 30)
		return (data.a * decodeInstructions.x) * data.rgb;
	#else
		return DecodeHDR(data, decodeInstructions);
	#endif
}

half3 Unity_GlossyEnvironment (UNITY_ARGS_TEXCUBE(tex), half4 hdr, half3 worldNormal, half roughness)
{
#if !UNITY_GLOSS_MATCHES_MARMOSET_TOOLBAG2 || (SHADER_TARGET < 30)
	float mip = roughness * UNITY_SPECCUBE_LOD_STEPS;
#else
	// TODO: remove pow, store cubemap mips differently
	float mip = pow(roughness,3.0/4.0) * UNITY_SPECCUBE_LOD_STEPS;
#endif

	half4 rgbm = SampleCubeReflection(tex, worldNormal.xyz, mip);
	return DecodeHDR_NoLinearSupportInSM2 (rgbm, hdr);
}

//-------------------------------------------------------------------------------------

// Note: BRDF entry points use oneMinusRoughness (aka "smoothness") and oneMinusReflectivity for optimization
// purposes, mostly for DX9 SM2.0 level. Most of the math is being done on these (1-x) values, and that saves
// a few precious ALU slots.


// Main Physically Based BRDF
// Derived from Disney work and based on Torrance-Sparrow micro-facet model
//
//   BRDF = kD / pi + kS * (D * V * F) / 4
//   I = BRDF * NdotL
//
// * NDF (depending on UNITY_BRDF_GGX):
//  a) Normalized BlinnPhong
//  b) GGX
// * Smith for Visiblity term
// * Schlick approximation for Fresnel
half4 BRDF1_Unity_PBS (half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness,
	half3 normal, half3 viewDir,
	UnityLight light, UnityIndirect gi)
{
	half roughness = 1-oneMinusRoughness;
	half3 halfDir = normalize (light.dir + viewDir);

	half nl = light.ndotl;
	half nh = BlinnTerm (normal, halfDir);
	half nv = DotClamped (normal, viewDir);
	half lv = DotClamped (light.dir, viewDir);
	half lh = DotClamped (light.dir, halfDir);

#if UNITY_BRDF_GGX
	half V = SmithGGXVisibilityTerm (nl, nv, roughness);
	half D = GGXTerm (nh, roughness);
#else
	half V = SmithBeckmannVisibilityTerm (nl, nv, roughness);
	half D = NDFBlinnPhongNormalizedTerm (nh, RoughnessToSpecPower (roughness));
#endif

	half nlPow5 = Pow5 (1-nl);
	half nvPow5 = Pow5 (1-nv);
	half Fd90 = 0.5 + 2 * lh * lh * roughness;
	half disneyDiffuse = (1 + (Fd90-1) * nlPow5) * (1 + (Fd90-1) * nvPow5);
	
	// HACK: theoretically we should divide by Pi diffuseTerm and not multiply specularTerm!
	// BUT 1) that will make shader look significantly darker than Legacy ones
	// and 2) on engine side "Non-important" lights have to be divided by Pi to in cases when they are injected into ambient SH
	// NOTE: multiplication by Pi is part of single constant together with 1/4 now

	half specularTerm = max(0, (V * D * nl) * unity_LightGammaCorrectionConsts_PIDiv4);// Torrance-Sparrow model, Fresnel is applied later (for optimization reasons)
	half diffuseTerm = disneyDiffuse * nl;
	
	half grazingTerm = saturate(oneMinusRoughness + (1-oneMinusReflectivity));
    half3 color =	diffColor * (gi.diffuse + light.color * diffuseTerm)
                    + specularTerm * light.color * FresnelTerm (specColor, lh)
					+ gi.specular * FresnelLerp (specColor, grazingTerm, nv);

	return half4(color, 1);
}

// Based on Minimalist CookTorrance BRDF
// Implementation is slightly different from original derivation: http://www.thetenthplanet.de/archives/255
//
// * BlinnPhong as NDF
// * Modified Kelemen and Szirmay-​Kalos for Visibility term
// * Fresnel approximated with 1/LdotH
half4 BRDF2_Unity_PBS (half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness,
	half3 normal, half3 viewDir,
	UnityLight light, UnityIndirect gi)
{
	half3 halfDir = normalize (light.dir + viewDir);

	half nl = light.ndotl;
	half nh = BlinnTerm (normal, halfDir);
	half nv = DotClamped (normal, viewDir);
	half lh = DotClamped (light.dir, halfDir);

	half roughness = 1-oneMinusRoughness;
	half specularPower = RoughnessToSpecPower (roughness);
	// Modified with approximate Visibility function that takes roughness into account
	// Original ((n+1)*N.H^n) / (8*Pi * L.H^3) didn't take into account roughness 
	// and produced extremely bright specular at grazing angles

	// HACK: theoretically we should divide by Pi diffuseTerm and not multiply specularTerm!
	// BUT 1) that will make shader look significantly darker than Legacy ones
	// and 2) on engine side "Non-important" lights have to be divided by Pi to in cases when they are injected into ambient SH
	// NOTE: multiplication by Pi is cancelled with Pi in denominator

	half invV = lh * lh * oneMinusRoughness + roughness * roughness; // approx ModifiedKelemenVisibilityTerm(lh, 1-oneMinusRoughness);
	half invF = lh;
	half specular = ((specularPower + 1) * pow (nh, specularPower)) / (unity_LightGammaCorrectionConsts_8 * invV * invF + 1e-4f); // @TODO: might still need saturate(nl*specular) on Adreno/Mali

	half grazingTerm = saturate(oneMinusRoughness + (1-oneMinusReflectivity));
    half3 color =	(diffColor + specular * specColor) * light.color * nl
    				+ gi.diffuse * diffColor
					+ gi.specular * FresnelLerpFast (specColor, grazingTerm, nv);

	return half4(color, 1);
}

// Old school, not microfacet based Modified Normalized Blinn-Phong BRDF
// Implementation uses Lookup texture for performance
//
// * Normalized BlinnPhong in RDF form
// * Implicit Visibility term
// * No Fresnel term
//
// TODO: specular is too weak in Linear rendering mode
sampler2D unity_NHxRoughness;
half4 BRDF3_Unity_PBS (half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness,
	half3 normal, half3 viewDir,
	UnityLight light, UnityIndirect gi)
{
	half LUT_RANGE = 16.0; // must match range in NHxRoughness() function in GeneratedTextures.cpp

	half3 reflDir = reflect (viewDir, normal);
	half3 halfDir = normalize (light.dir + viewDir);

	half nl = light.ndotl;
	half nh = BlinnTerm (normal, halfDir);
	half nv = DotClamped (normal, viewDir);

	// Vectorize Pow4 to save instructions
	half2 rlPow4AndFresnelTerm = Pow4 (half2(dot(reflDir, light.dir), 1-nv));  // use R.L instead of N.H to save couple of instructions
	half rlPow4 = rlPow4AndFresnelTerm.x; // power exponent must match kHorizontalWarpExp in NHxRoughness() function in GeneratedTextures.cpp
	half fresnelTerm = rlPow4AndFresnelTerm.y;

#if 1 // Lookup texture to save instructions
	half specular = tex2D(unity_NHxRoughness, half2(rlPow4, 1-oneMinusRoughness)).UNITY_ATTEN_CHANNEL * LUT_RANGE;
#else
	half roughness = 1-oneMinusRoughness;
	half n = RoughnessToSpecPower (roughness) * .25;
	half specular = (n + 2.0) / (2.0 * UNITY_PI * UNITY_PI) * pow(dot(reflDir, light.dir), n) * nl;// / unity_LightGammaCorrectionConsts_PI;
	//half specular = (1.0/(UNITY_PI*roughness*roughness)) * pow(dot(reflDir, light.dir), n) * nl;// / unity_LightGammaCorrectionConsts_PI;
#endif
	half grazingTerm = saturate(oneMinusRoughness + (1-oneMinusReflectivity));

    half3 color =	(diffColor + specular * specColor) * light.color * nl
    				+ gi.diffuse * diffColor
					+ gi.specular * lerp (specColor, grazingTerm, fresnelTerm);

	return half4(color, 1);
}


#endif // UNITY_STANDARD_BRDF_INCLUDED
