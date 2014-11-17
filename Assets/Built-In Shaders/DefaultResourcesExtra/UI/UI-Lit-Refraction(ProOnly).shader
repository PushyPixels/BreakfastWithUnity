Shader "UI/Lit/Refraction (Pro Only)"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Specular ("Specular Color", Color) = (0,0,0,0)
		_MainTex ("Diffuse (RGB), Alpha (A)", 2D) = "white" {}
		_MainBump ("Diffuse Bump Map", 2D) = "bump" {}
		_Mask ("Mask (Specularity, Shininess, Refraction)", 2D) = "black" {}
		_Shininess ("Shininess", Range(0.01, 1.0)) = 0.2
		_Focus ("Focus", Range(-100.0, 100.0)) = -100.0
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
	}
	
	SubShader
	{
		LOD 400

		GrabPass
		{
			Name "BASE"
			Tags { "LightMode" = "Always" }
		}

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
		AlphaTest Greater 0
		ColorMask [_ColorMask]

		CGPROGRAM
			#pragma target 3.0
			#pragma surface surf PPL alpha vertex:vert
				
			#include "UnityCG.cginc"
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord1 : TEXCOORD0;
				fixed4 color : COLOR;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
	
			struct Input
			{
				float4 vertex : SV_POSITION;
				float4 texcoord1 : TEXCOORD0;
				float4 proj : TEXCOORD1;
				fixed4 color : COLOR;
			};

			sampler2D _GrabTexture;
			sampler2D _MainTex;
			sampler2D _MainBump;
			sampler2D _Mask;

			float4 _MainTex_ST;
			float4 _MainBump_ST;
			float4 _Mask_ST;
			half4 _GrabTexture_TexelSize;

			fixed4 _Color;
			fixed4 _Specular;
			half _Shininess;
			half _Focus;
				
			void vert (inout appdata_t v, out Input o)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord1.xy = TRANSFORM_TEX(v.texcoord1, _MainTex);
				o.texcoord1.zw = TRANSFORM_TEX(v.texcoord1, _MainBump);
				o.color = v.color;

#ifdef UNITY_HALF_TEXEL_OFFSET
				o.vertex.xy -= (_ScreenParams.zw-1.0);
#endif

			#if UNITY_UV_STARTS_AT_TOP
				o.proj.xy = (float2(o.vertex.x, -o.vertex.y) + o.vertex.w) * 0.5;
			#else
				o.proj.xy = (float2(o.vertex.x, o.vertex.y) + o.vertex.w) * 0.5;
			#endif
				o.proj.zw = o.vertex.zw;
			}
				
			void surf (Input IN, inout SurfaceOutput o)
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord1.xy);
				half3 normal = UnpackNormal(tex2D(_MainBump, IN.texcoord1.zw));
				half3 mask = tex2D(_Mask, IN.texcoord1.xy);

				float2 offset = normal.xy * _GrabTexture_TexelSize.xy * _Focus;
				IN.proj.xy = offset * IN.proj.z + IN.proj.xy;
				half4 ref = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(IN.proj));

				col.rgb = lerp(col.rgb, ref.rgb, mask.b);
				col *= _Color * IN.color;
					
				o.Albedo = col.rgb;
				o.Normal = normalize(normal);
				o.Specular = _Specular.a * mask.r;
				o.Gloss = _Shininess * mask.g;
				o.Alpha = col.a;
			}

			half4 LightingPPL (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 nNormal = normalize(s.Normal);
				half shininess = s.Gloss * 250.0 + 4.0;

			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

				// Phong shading model
				half reflectiveFactor = max(0.0, dot(-viewDir, reflect(lightDir, nNormal)));

				// Blinn-Phong shading model
				//half reflectiveFactor = max(0.0, dot(nNormal, normalize(lightDir + viewDir)));
				
				half diffuseFactor = max(0.0, dot(nNormal, lightDir));
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
	Fallback "UI/Lit/Transparent"
}
