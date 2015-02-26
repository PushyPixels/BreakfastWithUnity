Shader "Custom/DistortionShader" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DistortionFrequency ("Distortion frequency", Range(1,256)) = 50
		_DistortionScale ("Distortion scale", Range(0,1)) = 0.1
		_DistortionSpeed ("Distortion speed", Range(0,10)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float _DistortionFrequency;
		float _DistortionScale;
		float _DistortionSpeed;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex + float2(_DistortionScale,0)*sin((_Time*_DistortionSpeed+IN.uv_MainTex.y)*_DistortionFrequency));
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
