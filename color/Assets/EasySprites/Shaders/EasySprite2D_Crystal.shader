/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Crystal -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

Shader "EasySprite2D/Crystal_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Alpha ("Alpha", Range (0,1)) = 1.0
_Factor ("Factor", Range (0.1,16)) = 1.0
_OffsetX ("OffsetX", Range (0,1)) = 0
_OffsetY ("OffsetY", Range (0,1)) = 0
}

SubShader
{
Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off
CGPROGRAM
#pragma surface surf Lambert vertex:vert 

struct Input
{
float2 uv_MainTex;
float4 grabUV;
};


sampler2D _MainTex;
fixed _Alpha;
float _OffsetX;
float _Factor;
float _OffsetY;


void vert (inout appdata_full v, out Input o)
{
float4 hpos = mul (UNITY_MATRIX_MVP, v.vertex+_OffsetY);
o.grabUV = ComputeGrabScreenPos(hpos)+v.vertex.x*v.vertex.y*2+_OffsetX;
}


void surf (Input IN, inout SurfaceOutput o)
{

fixed4 b = tex2D(_MainTex, IN.uv_MainTex);
o.Emission =b.rgb*b.rgb+(tex2Dproj( _MainTex, UNITY_PROJ_COORD(IN.grabUV))/_Factor);
o.Alpha = b.a-_Alpha;

}
ENDCG

}
Fallback "Sprites/Default"

}