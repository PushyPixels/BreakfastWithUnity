Shader "Simplex Normal Extrusion" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Extrusion Amount", Range(-1,1)) = 0.5
      _PerlinScale ("Perlin Scale", Vector) = (1, 1, 1)
      _PerlinOffset ("Perlin Offset", Vector) = (0, 0, 0)
      _PerlinSpeed ("Perlin Speed", Vector) = (0, 0, 0)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      #include "noiseSimplex.cginc"
      
		float3 _PerlinScale;
		float3 _PerlinOffset;
		float3 _PerlinSpeed;
      
      struct Input {
          float2 uv_MainTex;
      };
      float _Amount;
      
      void vert (inout appdata_full v)
      {
          v.vertex.xyz += v.normal * _Amount * snoise(v.vertex.xyz * _PerlinScale + _PerlinOffset + _Time*_PerlinSpeed);
      }
      
      sampler2D _MainTex;
      
      void surf (Input IN, inout SurfaceOutput o)
      {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }