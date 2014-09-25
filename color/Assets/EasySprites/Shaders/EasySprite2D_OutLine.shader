/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Outline -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Outline_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_OutLineSpread ("Outline Spread", Range(0,0.01)) = 0.007
_Color ("Tint", Color) = (1,1,1,1)
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
float _OutLineSpread;
fixed4 _Color;

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

fixed4 mainColor = (tex2D(_MainTex, i.texcoord+float2(-_OutLineSpread,_OutLineSpread))
+ tex2D(_MainTex, i.texcoord+float2(_OutLineSpread,-_OutLineSpread))
+ tex2D(_MainTex, i.texcoord+float2(_OutLineSpread,_OutLineSpread))
+ tex2D(_MainTex, i.texcoord-float2(_OutLineSpread,_OutLineSpread)));

mainColor.rgb = _Color.rgb;

fixed4 addcolor = tex2D(_MainTex, i.texcoord);

if(mainColor.a > 0.95) { mainColor = _Color; }
if(addcolor.a > 0.95) { mainColor = addcolor; }

mainColor.a = mainColor.a-(1-_Color.a);

return mainColor;
}
ENDCG
}
}
Fallback "Sprites/Default"

}