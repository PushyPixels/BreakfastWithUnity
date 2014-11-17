Shader "Hidden/Nature/Tree Soft Occlusion Leaves Rendertex" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_HalfOverCutoff ("0.5 / Alpha cutoff", Range(0,1)) = 1.0
		_BaseLight ("Base Light", Range(0, 1)) = 0.35
		_AO ("Amb. Occlusion", Range(0, 10)) = 2.4
		_Occlusion ("Dir Occlusion", Range(0, 20)) = 7.5
		
		// These are here only to provide default values
		_Scale ("Scale", Vector) = (1,1,1,1)
		_SquashAmount ("Squash", Float) = 1
	}
	SubShader {

		Tags { "Queue" = "Transparent-99" }
		Cull Off
		Fog { Mode Off}
		
		Pass {
			Lighting On
			ZWrite On

			CGPROGRAM
			#pragma vertex leaves
			#pragma fragment frag
			#pragma glsl_no_auto_normalization
			#define USE_CUSTOM_LIGHT_DIR 1
			#include "SH_Vertex.cginc"
			
			sampler2D _MainTex;
			fixed _Cutoff;
			
			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 col = tex2D( _MainTex, input.uv.xy);
				col.rgb *= 2.0f * input.color.rgb;
				clip(col.a - _Cutoff);
				col.a = 1;
				return col;
			}
			ENDCG
		}
	}
	
	Fallback Off
}
