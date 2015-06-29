/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - FIRE -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Light_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_OffsetX ("OffsetX", Range(0,128)) = 0
_OffsetY ("OffsetY", Range(0,128)) = 0
_NumberX ("NumberX", Range(0,128)) = 0
_RadiusX ("RadiusX", Range(0,128)) = 0
_Color ("Tint", Color) = (1,1,1,1)
_Intensity ("Intensity", Range(0,128)) = 0
_Intensity2 ("Intensity2", Range(0,128)) = 0
_BrightnessX ("BrightnessX", Range(0,128)) = 0
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
#pragma glsl
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
float4 _Color;
float _IntensityX;
float _OffsetX;
float _OffsetY;
float _Intensity;
float _Intensity2;
float _RadiusX;
float _BrightnessX;
float _WaveTimeX;
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
	fixed4 mainAlpha = tex2D(_MainTex, i.texcoord);
	float2 p=i.texcoord-float2(_OffsetX,_OffsetY);
  	float3 c = _BrightnessX/length(float2(p.x,p.y))*_RadiusX;
	c= lerp(float3(0,0,0),c,_Intensity2);
	float4 mainColor;
	mainColor.rgb= lerp(mainAlpha,c,_Intensity);
	mainColor.rgb*=c;
	mainColor=(mainColor*_Color);
	
	mainColor.a = mainAlpha.a*(1-_Alpha);
	
return mainColor;
}

ENDCG
}
}
Fallback "Sprites/Default"
}