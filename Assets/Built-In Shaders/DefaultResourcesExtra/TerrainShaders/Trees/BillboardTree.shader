Shader "Hidden/TerrainEngine/BillboardTree" {
	Properties {
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	}
	
	SubShader {
		Tags { "Queue" = "Transparent-100" "IgnoreProjector"="True" "RenderType"="TreeBillboard" }
		
		Pass {
			ColorMask rgb
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"
			#pragma fragment frag

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR0;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata_tree_billboard v) {
				v2f o;
				TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);	
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv.x = v.texcoord.x;
				o.uv.y = v.texcoord.y > 0;
				o.color = v.color;
				return o;
			}

			sampler2D _MainTex;
			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 col = tex2D( _MainTex, input.uv);
				col.rgb *= input.color.rgb;
				clip(col.a);
				return col;
			}
			ENDCG			
		}
	}

	Fallback Off
}