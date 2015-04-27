#ifndef UNITY_STANDARD_CONFIG_INCLUDED
#define UNITY_STANDARD_CONFIG_INCLUDED

// Define Specular cubemap constants
#define UNITY_SPECCUBE_LOD_EXPONENT (1.5)
#define UNITY_SPECCUBE_LOD_STEPS (7) // TODO: proper fix for different cubemap resolution needed. My assumptions were actually wrong!

// Energy conservation for Specular workflow is Monochrome. For instance: Red metal will make diffuse Black not Cyan
#define UNITY_CONSERVE_ENERGY 1
#define UNITY_CONSERVE_ENERGY_MONOCHROME 1

// High end platforms support Box Projection and Blending
#define UNITY_SPECCUBE_BOX_PROJECTION ( !defined(SHADER_API_MOBILE) && (SHADER_TARGET >= 30) )
#define UNITY_SPECCUBE_BLENDING ( !defined(SHADER_API_MOBILE) && (SHADER_TARGET >= 30) )

#define UNITY_SAMPLE_FULL_SH_PER_PIXEL 0

#define UNITY_GLOSS_MATCHES_MARMOSET_TOOLBAG2 1
#define UNITY_BRDF_GGX 0

// Orthnormalize Tangent Space basis per-pixel
// Necessary to support high-quality normal-maps. Compatible with Maya and Marmoset.
// However xNormal expects oldschool non-orthnormalized basis - essentially preventing good looking normal-maps :(
// Due to the fact that xNormal is probably _the most used tool to bake out normal-maps today_ we have to stick to old ways for now.
// 
// Disabled by default, until xNormal has an option to bake proper normal-maps.
#define UNITY_TANGENT_ORTHONORMALIZE 0

#endif // UNITY_STANDARD_CONFIG_INCLUDED
