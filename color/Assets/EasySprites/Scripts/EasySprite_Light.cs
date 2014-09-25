////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Fire  -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Light")]
public class EasySprite_Light : MonoBehaviour {


	[Range(0, 1)]
	public float _Alpha = 1f;
	[Range(0f, 1f)]
	public float _Intensity = 0.5f;
	[Range(0f, 1f)]
	public float _IntensityAlpha = 0f;

	public Color _Color;

	[Range(-2f, 3f)]
	public float _OffsetX = 0.5f;
	[Range(-2f, 3f)]
	public float _OffsetY = 0.5f;
	[Range(0.001f, 1.000f)]
	public float _RadiusX = 0.25f;
	[Range(0.001f, 10f)]
	public float _BrightnessX = 0.02f;




	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Light_EasyS2D"));
		//tempMaterial.mainTexture.wrapMode= TextureWrapMode.Repeat;

		renderer.sharedMaterial = tempMaterial;
	
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_OffsetX", _OffsetX);
		renderer.sharedMaterial.SetFloat("_OffsetY", _OffsetY);
		renderer.sharedMaterial.SetColor("_Color", _Color);
		renderer.sharedMaterial.SetFloat("_Intensity2", 1-_Intensity);
		renderer.sharedMaterial.SetFloat("_Intensity", _IntensityAlpha);
		renderer.sharedMaterial.SetFloat("_RadiusX", _RadiusX);
		renderer.sharedMaterial.SetFloat("_BrightnessX", _BrightnessX);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Light_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetColor("_Color", _Color);
		renderer.sharedMaterial.SetFloat("_OffsetX", _OffsetX);
		renderer.sharedMaterial.SetFloat("_OffsetY", _OffsetY);
		renderer.sharedMaterial.SetFloat("_Intensity2", 1-_Intensity);
		renderer.sharedMaterial.SetFloat("_Intensity", _IntensityAlpha);
		renderer.sharedMaterial.SetFloat("_RadiusX", _RadiusX);
		renderer.sharedMaterial.SetFloat("_BrightnessX", _BrightnessX);
	
	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
