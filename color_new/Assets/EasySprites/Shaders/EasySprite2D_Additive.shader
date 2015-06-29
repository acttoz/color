/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Pixel -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Additive_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Alpha ("Alpha", Range (0,1)) = 1.0
}

SubShader
{
Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
ZWrite Off Blend SrcAlpha One Cull Off


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
fixed4 Sprite = tex2D(_MainTex, i.texcoord);
Sprite.a = Sprite.a-_Alpha;

return Sprite;
}
ENDCG
}
}
Fallback "Sprites/Default"

}