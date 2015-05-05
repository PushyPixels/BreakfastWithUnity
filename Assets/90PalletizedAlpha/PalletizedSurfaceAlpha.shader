Shader "PushyPixels/PalletizedSurfaceAlpha" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_SecondTex ( "Second Texture", 2D ) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha:blend

uniform sampler2D _MainTex;
uniform sampler2D _SecondTex;
uniform float4 _SecondTex_ST;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D( _SecondTex, TRANSFORM_TEX(tex2D( _MainTex, IN.uv_MainTex ), _SecondTex));
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}
