Shader "SimpleTransparent" {
 Properties {
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 }

 SubShader {
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 100
     
     ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha, One One
 
     Pass {
         Lighting Off
         SetTexture [_MainTex]
         {
             constantColor [_Color]
             Combine texture * constant, texture * constant
         }
     }
 }
}