// Toony Colors Pro+Mobile Shaders
// (c) 2013, Jean Moreno

Shader "Toony Colors Pro/Normal/OneDirLight/BasicAlphaTransparent"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_Alpha ("TotalAlpha", Range (0.0,1.0)) = 1.0
		
		//COLORS
		_Color ("Highlight Color", Color) = (0.8,0.8,0.8,1)
		_SColor ("Shadow Color", Color) = (0.0,0.0,0.0,1)
		
	}
	
	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 200
		Blend One One//SrcAlpha OneMinusSrcAlpha
		Pass {
        ZWrite On
        ColorMask 0
    	}
		
		
		CGPROGRAM
		
		#include "TGP_Include.cginc"
		
		//nolightmap nodirlightmap		LIGHTMAP
		//noforwardadd					ONLY 1 DIR LIGHT (OTHER LIGHTS AS VERTEX-LIT)
		#pragma surface surf ToonyColors nolightmap nodirlightmap noforwardadd alpha
		
		sampler2D _MainTex;
		float _Alpha;
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			//float fFinalAlpha = c.a * _Alpha;
			o.Albedo = c.rgb;
			o.Alpha = _Alpha * c.a;			
		}
		ENDCG
	}
	
	Fallback "VertexLit"
}
