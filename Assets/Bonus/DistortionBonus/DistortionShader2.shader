Shader "Custom/DistortionShader2" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DistortionFrequency ("Distortion frequency", float) = 50
		_DistortionSpeed ("Distortion speed", float) = 1
		_DistortionScale ("Distortion scale", float) = 0.1
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
			float x = IN.uv_MainTex.x-0.5;
			float y = IN.uv_MainTex.y-0.5;
			float dist = x*x + y*y; //Technically incorrect, you can use sqrt here for correct value but it adds more computational cost "sqrt(x*x + y*y);"
			half4 c = tex2D (_MainTex, IN.uv_MainTex + float2(_DistortionScale,0)*sin((_Time*_DistortionSpeed+dist)*_DistortionFrequency));
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
