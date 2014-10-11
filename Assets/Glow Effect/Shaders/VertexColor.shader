// A simple unlit additive shader. _Glow* properties are not used in this shader but are used by the replacement shader.
Shader "Glow Effect/Vertex Color"
{
	Properties
	{
		_GlowTex ("Glow Texture", 2D) = "white" {}
		_GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
		_GlowColorMult ("Glow Color Multiplier", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags { "RenderType" = "Glow" "Queue" = "Geometry" }
		
        Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			uniform half4 _GlowColor;
			uniform half4 _GlowColorMult;

			struct appdata_color
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
			};

			struct v2f {
				half4 pos : SV_POSITION;
				half4 color : COLOR;
			};	
			
			v2f vert (appdata_color v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		       	o.color = v.color;				
				return o;
			}
				
			half4 frag(v2f i) : COLOR
			{
				return i.color;
			}
			
			ENDCG
		}
	}
	
	Fallback "Diffuse"
	//CustomEditor "GlowMaterialInspector"
}