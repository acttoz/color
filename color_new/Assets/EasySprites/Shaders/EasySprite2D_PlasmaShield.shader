﻿/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - PlasmaShield -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/PlasmaShield" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Size ("Size", Range(4,128)) = 4
_Color ("Tint", Color) = (1,1,1,1)
_Offset ("Offset", Range(4,128)) = 4
_Alpha ("Alpha", Range (0,1)) = 1.0
_TimeX ("TimeX", Range(0,1)) = 0

}

SubShader
{
Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0
#include "UnityCG.cginc"

struct appdata_t
{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
half2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
fixed4 color    : COLOR;
};

sampler2D _MainTex;
float _Offset;
fixed _Alpha;
fixed4 _Color;
float _Size;
fixed _TimeX;


v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}

inline float mod(float x,float modu) 
{
  return x - floor(x * (1.0 / modu)) * modu;
}   

fixed4 rainbow(float t) 
{
	t=mod(t,1.0);
	fixed tx = t * _Size;
	fixed r = clamp(tx - 2.0, 0.0, 1.0) + clamp(2.0 - tx, 0.0, 1.0);
	return fixed4(_Color.r, _Color.g, _Color.b,1-r+(1.-_Color.a));
}

float4 plasma(float2 uv)
{
	uv *= _Offset;
	float a = 1.1 + _TimeX * 2.25;
	float x1=2.0*uv.x;
	float n = sin(a + x1) + sin(a - x1) + sin(a + 2.0 * uv.y) + sin(a + 5.0 * uv.y);
	n = mod(((5.0 + n) / 5.0), 1.0);
	float nx=tex2D(_MainTex, uv);
	n += nx.r * 0.2 + nx.g * 0.4 + nx.b * 0.2;
	return rainbow(n);
}




float4 frag (v2f i) : COLOR
{
	float alpha = tex2D(_MainTex, i.texcoord).a;
	
	float4 sortie=plasma(i.texcoord);
	sortie.a=sortie.a*alpha-_Alpha;
	return sortie;	

}
ENDCG
}
}
Fallback "Sprites/Default"

}