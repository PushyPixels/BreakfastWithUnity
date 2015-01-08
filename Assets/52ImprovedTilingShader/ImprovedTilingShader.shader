Shader "Diffuse - Improved Tiling" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_UVScale ("Second layer scale", Range(-1,-16)) = -7
	_Desaturation ("Second layer desaturation percentage", Range(0,1)) = 0.25
	_Brightness ("Final brightness adjustment", Range(1,4)) = 2
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;
float _UVScale;
float _Desaturation;
float _Brightness;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o)
{
	fixed4 c1 = tex2D(_MainTex, IN.uv_MainTex) * _Color * _Brightness;
	fixed4 c2 = tex2D(_MainTex, IN.uv_MainTex/_UVScale) * _Color * _Brightness;
	
	o.Albedo = c1*lerp(c2,Luminance(c2),_Desaturation);
}
ENDCG
}

Fallback "VertexLit"
}
