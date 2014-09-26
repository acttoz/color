﻿/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Clipping -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

Shader "EasySprite2D/Clip_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_ClipLeft ("Clipping Left", Range(0,1)) = 1
_ClipRight ("Clipping Right", Range(0,1)) = 1
_ClipUp ("Clipping Up", Range(0,1)) = 1
_ClipDown ("Clipping Down", Range(0,1)) = 1
_Alpha ("Alpha", Range (0,1)) = 1.0
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
float _Size;
float _ClipLeft;
float _ClipRight;
float _ClipUp;
float _ClipDown;
fixed _Alpha;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}

float4 frag (v2f i) : COLOR
{

fixed4 mainColor = tex2D(_MainTex, i.texcoord);

if ( i.texcoord.y > _ClipUp) mainColor = float4(0,0,0,0);
if ( i.texcoord.y < 1-_ClipDown) mainColor = float4(0,0,0,0);
if ( i.texcoord.x > _ClipRight) mainColor = float4(0,0,0,0);
if ( i.texcoord.x < 1-_ClipLeft) mainColor = float4(0,0,0,0);

mainColor.a = mainColor.a-_Alpha;
return mainColor;
}
ENDCG
}
}
Fallback "Sprites/Default"
}