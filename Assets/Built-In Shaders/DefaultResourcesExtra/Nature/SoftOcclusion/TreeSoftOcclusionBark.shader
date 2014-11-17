Shader "Nature/Tree Soft Occlusion Bark" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Main Texture", 2D) = "white" {}
		_BaseLight ("Base Light", Range(0, 1)) = 0.35
		_AO ("Amb. Occlusion", Range(0, 10)) = 2.4
		
		// These are here only to provide default values
		_Scale ("Scale", Vector) = (1,1,1,1)
		_SquashAmount ("Squash", Float) = 1
	}
	
	SubShader {
		Tags {
			"IgnoreProjector"="True"
			"RenderType" = "TreeOpaque"
		}

		Pass {
			Lighting On
		
			CGPROGRAM
			#pragma vertex bark
			#pragma fragment frag 
			#pragma glsl_no_auto_normalization
			#include "SH_Vertex.cginc"
			
			sampler2D _MainTex;
			
			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 col = input.color;
				col.rgb *= 2.0f * tex2D( _MainTex, input.uv.xy).rgb;
				return col;
			}
			ENDCG
		}
		
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			Fog {Mode Off}
			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma glsl_no_auto_normalization
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"
			
			struct v2f { 
				V2F_SHADOW_CASTER;
			};
			
			struct appdata {
			    float4 vertex : POSITION;
			    fixed4 color : COLOR;
			};
			v2f vert( appdata v )
			{
				v2f o;
				TerrainAnimateTree(v.vertex, v.color.w);
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
			
			float4 frag( v2f i ) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG	
		}
	}
	
	Dependency "BillboardShader" = "Hidden/Nature/Tree Soft Occlusion Bark Rendertex"
	Fallback Off
}
