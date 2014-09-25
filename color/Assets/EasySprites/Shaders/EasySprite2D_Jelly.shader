/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Wave -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////
Shader "EasySprite2D/Jelly_EasyS2D" {
Properties
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_OffsetX ("OffsetX", Range(0,128)) = 0
_OffsetY ("OffsetY", Range(0,128)) = 0
_DistanceX ("DistanceX", Range(0,1)) = 0
_DistanceY ("DistanceY", Range(0,1)) = 0
_WaveTimeX ("WaveTimeX", Range(0,360)) = 0
_WaveTimeY ("WaveTimeY", Range(0,360)) = 0
_AlphaIn ("_AlphaIn", Range (0,1)) = 1.0
_Alpha ("Alpha", Range (0,1)) = 1.0
}


SubShader
{

Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off


Pass
{
CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members grabUV)
#pragma exclude_renderers d3d11 xbox360
#pragma vertex vert
#pragma fragment frag
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
float4 grabUV;
};


sampler2D _MainTex;
float _OffsetX;
float _OffsetY;
float _DistanceX;
float _DistanceY;
float _WaveTimeX;
float _WaveTimeY;
float _Alpha;
float _AlphaIn;

v2f vert(appdata_t IN)
{
v2f OUT; 
OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
float2 p=IN.texcoord;
p.x= p.x+sin(p.y*_OffsetX+_WaveTimeX)*_DistanceX;
p.y= p.y+cos(p.x*_OffsetY+_WaveTimeY)*_DistanceY;
OUT.texcoord = p;
OUT.color = IN.color;
float4 hpos = mul (UNITY_MATRIX_MVP, IN.vertex+1);

OUT.grabUV = ComputeGrabScreenPos(hpos)+IN.vertex.x*IN.vertex.y*2+1;
return OUT;
}

fixed4 frag(v2f IN) : COLOR
{
float2 p=IN.texcoord;
fixed4 mainColor2 = tex2D(_MainTex, p+float2(0.0,sin(_WaveTimeX)/40));
p.x= p.x+sin(p.y*_OffsetX+_WaveTimeX)*_DistanceX;
p.y= p.y+cos(p.x*_OffsetY+_WaveTimeY)*_DistanceY;
fixed4 mainColor = tex2D(_MainTex, p*0.9+float2(0.05,0.05));

float dist = 1.0 - smoothstep( 0.2,0.55, length(float2(0.5,0.5)-p));

mainColor.a=(1-dist/2)*mainColor2.a;
mainColor.rgb=lerp(mainColor.rgb,mainColor2.rgb,_AlphaIn);
mainColor.rgb = mainColor.rgb*mainColor.rgb+(tex2Dproj( _MainTex, UNITY_PROJ_COORD(IN.grabUV))/7.15);
mainColor.a = (mainColor.a +mainColor2.a)/2;

mainColor.a=mainColor.a*(1-_Alpha);
return mainColor;
}
ENDCG
}
}
Fallback "Sprites/Default"

}