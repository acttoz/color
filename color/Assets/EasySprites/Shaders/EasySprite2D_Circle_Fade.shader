/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Circle Fade -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Circle_Fade_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_Offset ("Offset", Range(-1,1)) = 0.5
_InOut ("InOut", Range(0,1)) = 0.5
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
float _InOut;
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

float2 uv = i.texcoord.xy;
		float4 tex = tex2D(_MainTex, uv);
		float alpha = tex.a;
		float2 center = float2(0.5,0.5);
		float dist = 1.0 - smoothstep( _Offset,_Offset+0.15, length(center-uv) );
		
		float c;
		
		if (_InOut==0) { c = dist; } else { c= 1-dist; }
	
		tex.a = alpha*c-_Alpha;

return tex;
}
ENDCG
}
}
Fallback "Sprites/Default"

}