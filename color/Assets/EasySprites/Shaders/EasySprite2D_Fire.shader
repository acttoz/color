/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - FIRE -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Fire_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_OffsetX ("OffsetX", Range(0,128)) = 0
_OffsetY ("OffsetY", Range(0,128)) = 0
_DistanceX ("DistanceX", Range(0,1)) = 0
_WaveTimeX ("WaveTimeX", Range(0,12.56)) = 0
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
float _OffsetX;
float _DistanceX;
float _WaveTimeX;
fixed _Alpha;


v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
float2 p = IN.texcoord;
p.x= p.x+sin(p.y*_OffsetX*_WaveTimeX)*_DistanceX;
p.y= p.y+cos(p.x*_OffsetX*_WaveTimeX)*_DistanceX;
OUT.texcoord = p;
OUT.color = IN.color;
return OUT;
}


inline float2 mod(float2 x,float modu) 
{
return x - floor(x * (1.0 / modu)) * modu;
}

float4 frag (v2f i) : COLOR
{
fixed4 mainAlpha = tex2D(_MainTex, i.texcoord);
float2 p=i.texcoord;

mod(p.x,1.0);
mod(p.y,1.0);

float4 mainColor = tex2D(_MainTex, mod(p+float2(1,-_WaveTimeX/3),2.0));
mainColor += tex2D(_MainTex, mod(p+float2(-0.1,1-_WaveTimeX/6),2.0));
mainColor += tex2D(_MainTex, mod(p+float2(0,1-_WaveTimeX/2),2.0));
mainColor += tex2D(_MainTex, mod(p+float2(0.1,1-_WaveTimeX/4),2.0));
mainColor = mainColor/2;
mainColor.a =(mainColor.a-_Alpha)*mainAlpha.a*2;

return mainColor*(1-p.y);
}

ENDCG
}
}
Fallback "Sprites/Default"
}