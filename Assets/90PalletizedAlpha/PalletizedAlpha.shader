Shader "PushyPixels/PalletizedAlpha"
{
	Properties
	{
		_MainTex ( "Main Texture", 2D ) = "white" {}
		_SecondTex ( "Second Texture", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
CGPROGRAM
#pragma exclude_renderers ps3 xbox360 flash
#pragma fragmentoption ARB_precision_hint_fastest
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"


// uniforms
uniform sampler2D _MainTex;
uniform float4 _MainTex_ST;
uniform sampler2D _SecondTex;
uniform float4 _SecondTex_ST;


struct vertexInput
{
	float4 vertex : POSITION; // position (in object coordinates, i.e. local or model coordinates)
	float4 texcoord : TEXCOORD0;  // 0th set of texture coordinates (a.k.a. “UV”; between 0 and 1)
};


struct fragmentInput
{
	float4 pos : SV_POSITION;
	half2 uv : TEXCOORD0;
};


fragmentInput vert( vertexInput i )
{
	fragmentInput o;
	o.pos = mul( UNITY_MATRIX_MVP, i.vertex );
	o.uv = TRANSFORM_TEX( i.texcoord, _MainTex );
	return o;
}


half4 frag( fragmentInput i ) : COLOR
{
	return tex2D( _SecondTex, TRANSFORM_TEX(tex2D( _MainTex, i.uv ), _SecondTex));
}

ENDCG
		} // end Pass
	} // end SubShader
	
	FallBack "Diffuse"
}
