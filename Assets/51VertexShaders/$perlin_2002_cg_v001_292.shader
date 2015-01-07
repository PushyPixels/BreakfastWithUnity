Shader "Perlin_2002_Cg_v001"
{
	Properties 
	{				
		ambient_color 	("Ambient",  Color) = (0.5, 0.5, 0.5)
		diffuse_color 	("Diffuse",  Color) = (1, 1, 1)
		specular_color 	("Specular", Color) = (0.5,0.5,0.5)
		shine 			("Shine", Float) = 32
		
		color_map 	("color map" , 2D) = "white" {} 
		permutation ("permutation table" , 2D) = "white" {} 
		
		scale_x 	("scale_x", Float) = 15
		scale_y 	("scale_y", Float) = 15
		shift_x 	("shift_x", Float) = 0
		shift_y 	("shift_y", Float) = 0
		
		gain 	("gain", Float) = 1
		setup 	("setup", Float) = 0.5
		
		lightpos ("lightpos", Vector) = (-10, 10, 10)

	}	
	SubShader 
	{
		Pass 
		{			
			
CGPROGRAM //--------------
#pragma target 3.0	
#pragma vertex 	 vertex_shader
#pragma fragment fragment_shader
#pragma profileoption MaxTexIndirections=16

uniform sampler2D permutation;

int perm(int d)
{
	d = d % 256;
	float2 t = float2(d%16,d/16)/15.0;
	return tex2D(permutation,t).r *255;
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

struct a2v
{
	float4 vertex	: POSITION;
	float4 color	: COLOR;
	float2 texcoord : TEXCOORD;
	float3 normal	: NORMAL;
	float4 tangent	: TANGENT;
};

struct v2f
{
	float4 position : POSITION;
	float2 texcoord : TEXCOORD;
	float3 normal	: TEXCOORD1;
	float3 eye		: TEXCOORD2;
	float3 light	: TEXCOORD3;
};

uniform float3 lightpos;

v2f vertex_shader(a2v IN) 
{ 
	v2f OUT;

	float4 pos = float4(IN.vertex.xyz, 1.0);
	OUT.position 	= mul(glstate.matrix.mvp, pos);
	OUT.texcoord 	= IN.texcoord.xy;	
	

	float3x3 modelviewrot	=float3x3(glstate.matrix.modelview[0]);

	float3 IN_bINormal 		= cross( IN.normal, IN.tangent.xyz )*IN.tangent.w;	
	float3 tangent			= mul(modelviewrot,IN.tangent.xyz);
	float3 bINormal			= mul(modelviewrot,IN_bINormal.xyz);
	float3 normal			= mul(modelviewrot,IN.normal);
	float3x3 tangentspace	= float3x3(IN.tangent.xyz,IN_bINormal,IN.normal);
	
	float3 vpos	=mul(glstate.matrix.modelview[0],pos).xyz;		
	OUT.eye		=mul(tangentspace,vpos);
	OUT.light	=mul(tangentspace,lightpos.xyz-vpos);		
	//OUT.light=mul(tangentspace,glstate.light[0].position.xyz -vpos);	
	
	OUT.normal 	= normal;	// <---- this is wrong !!
	
	return OUT; 
}

uniform sampler2D color_map;
uniform float4 ambient_color;
uniform float4 diffuse_color;
uniform float4 specular_color;
uniform float shine;
uniform float scale_x;
uniform float scale_y;
uniform float shift_x;
uniform float shift_y;
uniform float gain;
uniform float setup;

float4 fragment_shader( v2f IN, out float4 finalcolor): COLOR
{	
	float nx 	= IN.texcoord.x*scale_x +shift_x;
	float ny 	= IN.texcoord.y*scale_y +shift_y;
	
	// well get Arithmetic instruction limit of 512 exceeded...
	// ... if apply the "z" value other than whole number :p
	float ns 	= noise(nx, ny, 0)*gain + setup;
	
	float4 c 	= tex2D(color_map,IN.texcoord)*ns;
	
	float3 L 	= normalize(IN.light);
	float3 V 	= normalize(IN.eye);
	float diff 	= saturate(dot(L,IN.normal));
	float spec 	= saturate(dot(normalize(L-V),IN.normal));
	float att 	= 1.0 - max(0,L.z); 
	att = 1.0 - att*att;
	
	return c*ambient_color + att*(c*diffuse_color*diff +specular_color*pow(spec,shine)*ns);
}

ENDCG //--------------
		} // Pass
	} // SubShader
} // Shader