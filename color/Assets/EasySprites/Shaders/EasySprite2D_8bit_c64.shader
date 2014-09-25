/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - 8 Bits C64 -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////


Shader "EasySprite2D/8Bitc64_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Size ("Size", Range(0,1)) = 0
_Offset ("Offset", Range(0,1)) = 0
_Offset2 ("Offset2", Range(0,1)) = 0
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
float _Size;
float _Offset;
float _Offset2;
fixed _Alpha;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}

float compare(float3 a, float3 b)
{
a=a*a*a;
b=b*b*b;
float3 diff = (a - b);
return dot(diff, diff);
}

inline float mod(float x,float modu)
{
return x - floor(x * (1.0 / modu)) * modu;
}

float4 frag (v2f i) : COLOR
{

float2 q  = i.texcoord;
float2 pixelSize=64*_Size;
pixelSize.x/=3;
float2 c = floor(q * pixelSize)/pixelSize;

float alpha = tex2D(_MainTex, c).a;
float3 src = tex2D(_MainTex, c).rgb;
if (alpha<0.95) alpha=0;

src*=_Offset2;

float3 dst0 = float3(0);
float3 dst1 = float3(0);
float best0 = 1e3;
float best1 = 1e3;

#define TCOLOR(R, G, B) { const float3 tst = float3(R, G, B); float err = compare(src, tst); if (err < best0) { best1 = best0; dst1 = dst0; best0 = err; dst0 = tst; } }
TCOLOR(0.,0.,0.);
TCOLOR(1.,1.,1.);
TCOLOR(0.62890625,0.30078125,0.26171875);
TCOLOR(0.4140625,0.75390625,0.78125);
TCOLOR(0.6328125,0.33984375,0.64453125);
TCOLOR(0.359375,0.67578125,0.37109375);
TCOLOR(0.30859375,0.265625,0.609375);
TCOLOR(0.79296875,0.8359375,0.53515625);
TCOLOR(0.63671875,0.40625,0.2265625);
TCOLOR(0.4296875,0.32421875,0.04296875);
TCOLOR(0.796875,0.49609375,0.4609375);
TCOLOR(0.38671875,0.38671875,0.38671875);
TCOLOR(0.54296875,0.54296875,0.54296875);
TCOLOR(0.60546875,0.88671875,0.61328125);
TCOLOR(0.5390625,0.49609375,0.80078125);
TCOLOR(0.68359375,0.68359375,0.68359375);
#undef TCOLOR

best0 = sqrt(best0); best1 = sqrt(best1);
float4 FragColor = float4(mod(c.x + c.y, 2.0) > (1+best1 / (best0 + best1)) ? dst1 : dst0, 1.0);
return FragColor*alpha*(1-_Alpha);
}
ENDCG
}
}
Fallback "Sprites/Default"

}