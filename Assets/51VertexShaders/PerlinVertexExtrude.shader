Shader "Perlin Normal Extrusion" {
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
      #pragma target 3.0
      #pragma surface surf Lambert vertex:vert
      
		int perm(int d)
		{
			d = d % 256;
			float2 t = float2(d%16,d/16)/15.0;
			return t*255; 
		}

		float fade(float t) { return t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }

		float lerp(float t,float a,float b) { return a + t * (b - a); }

		float grad(int hash,float x,float y,float z)
		{
			int h	= hash % 16;										// & 15;
			float u = h<8 ? x : y;
			float v = h<4 ? y : (h==12||h==14 ? x : z);
			return ((h%2) == 0 ? u : -u) + (((h/2)%2) == 0 ? v : -v); 	// h&1, h&2 
		}

		float noise(float x, float y,float z)
		{	
			int X = (int)floor(x) % 256;	// & 255;
			int Y = (int)floor(y) % 256;	// & 255;
			int Z = (int)floor(z) % 256;	// & 255;
			
			x -= floor(x);
			y -= floor(y);
			z -= floor(z);
		      
			float u = fade(x);
			float v = fade(y);
			float w = fade(z);
			
			int A	= perm(X  	)+Y;
			int AA	= perm(A	)+Z;
			int AB	= perm(A+1	)+Z; 
			int B	= perm(X+1	)+Y;
			int BA	= perm(B	)+Z;
			int BB	= perm(B+1	)+Z;
				
			return lerp(w, lerp(v, lerp(u, grad(perm(AA  ), x  , y  , z   ),
		                                   grad(perm(BA  ), x-1, y  , z   )),
		                           lerp(u, grad(perm(AB  ), x  , y-1, z   ),
		                                   grad(perm(BB  ), x-1, y-1, z   ))),
		                   lerp(v, lerp(u, grad(perm(AA+1), x  , y  , z-1 ),
		                                   grad(perm(BA+1), x-1, y  , z-1 )),
		                           lerp(u, grad(perm(AB+1), x  , y-1, z-1 ),
		                                   grad(perm(BB+1), x-1, y-1, z-1 ))));
		}
		
		float3 _PerlinScale;
		float3 _PerlinOffset;
		float3 _PerlinSpeed;
      
      struct Input {
          float2 uv_MainTex;
      };
      float _Amount;
      
      void vert (inout appdata_full v)
      {
          v.vertex.xyz += v.normal * _Amount *
          noise(v.vertex.x*_PerlinScale.x + _PerlinOffset.x + _Time*_PerlinSpeed.x,
          v.vertex.y*_PerlinScale.y + _PerlinOffset.y + _Time*_PerlinSpeed.y,
          v.vertex.z*_PerlinScale.z + _PerlinOffset.z + _Time*_PerlinSpeed.z);
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