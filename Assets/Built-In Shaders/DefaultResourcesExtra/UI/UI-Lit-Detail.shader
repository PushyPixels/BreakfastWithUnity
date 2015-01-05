Shader "UI/Lit/Detail"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Specular ("Specular Color", Color) = (0,0,0,0)
		_MainTex ("Diffuse (RGB), Alpha (A)", 2D) = "white" {}
		_MainBump ("Diffuse Bump Map", 2D) = "bump" {}
		_DetailTex ("Detail (RGB)", 2D) = "white" {}
		_DetailBump ("Detail Bump Map", 2D) = "bump" {}
		_Strength ("Detail Strength", Range(0.0, 1.0)) = 0.2
		_Shininess ("Shininess", Range(0.01, 1.0)) = 0.2
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType"="Plane"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		CGPROGRAM
			#pragma surface surf PPL alpha vertex:vert

			#include "UnityCG.cginc"
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				fixed4 color : COLOR;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
	
			struct Input
			{
				half2 uv_MainTex;
				half2 uv2_DetailTex;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _MainBump;
			sampler2D _DetailTex;
			sampler2D _DetailBump;

			float4 _DetailTex_TexelSize;
			fixed4 _Color;
			fixed4 _Specular;
			half _Strength;
			half _Shininess;
				
			void vert (inout appdata_t v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				v.texcoord1.xy *= _DetailTex_TexelSize.xy;
				v.color *= _Color;
			}
				
			void surf (Input IN, inout SurfaceOutput o)
			{
				fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 detail = tex2D(_DetailTex, IN.uv2_DetailTex);

				// Mix normals by just averaging the data and then doing unpack.
				// Much cheaper than unpacking both and then averaging -
				// not 100% the same result but fairly close. And neither of
				// these is "proper" normal blending anyway.
				fixed4 nmMain = tex2D(_MainBump, IN.uv_MainTex);
				fixed4 nmDetail = tex2D(_DetailBump, IN.uv2_DetailTex);
				fixed3 normal = UnpackNormal((nmMain + nmDetail) * 0.5);

				col.rgb = lerp(col.rgb, col.rgb * detail.rgb, detail.a * _Strength);
				col *= IN.color;

				o.Albedo = col.rgb;
				o.Normal = normal;
				o.Specular = _Specular.a;
				o.Gloss = _Shininess;
				o.Alpha = col.a;
			}

			half4 LightingPPL (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half shininess = s.Gloss * 250.0 + 4.0;

			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

				// Phong shading model
				half reflectiveFactor = max(0.0, dot(-viewDir, reflect(lightDir, s.Normal)));

				// Blinn-Phong shading model
				//half reflectiveFactor = max(0.0, dot(s.Normal, normalize(lightDir + viewDir)));
				
				half diffuseFactor = max(0.0, dot(s.Normal, lightDir));
				half specularFactor = pow(reflectiveFactor, shininess) * s.Specular;

				half4 c;
				c.rgb = (s.Albedo * diffuseFactor + _Specular.rgb * specularFactor) * _LightColor0.rgb;
				c.rgb *= (atten * 2.0);
				c.a = s.Alpha;
				clip (c.a - 0.01);
				return c;
			}
		ENDCG
	}
}
