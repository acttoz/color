/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Pixel -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Pixel_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Offset ("Offset", Range(4,128)) = 4
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
float _Offset;
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
fixed4 colorx = tex2D(_MainTex, i.texcoord);
fixed alpha = colorx.a;
float2 p=i.texcoord;
p= floor(p * _Offset)/_Offset;
fixed4 mainColor = tex2D(_MainTex, p);
mainColor.a = alpha-_Alpha;

return mainColor;
}
ENDCG
}
}
Fallback "Sprites/Default"

}