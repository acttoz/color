/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - GrayScale -1.2- by VETASOFT 2014
//////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/GrayScale")]

public class EasySprite_GrayScale : MonoBehaviour {

	[Range(0, 1)]
	public float _EffectAmount =1f; 
	[Range(0, 1)]
	public float _Alpha = 1f;
	Material tempMaterial;
	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/GrayScale_EasyS2D"));

		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_EffectAmount", _EffectAmount);
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
	}
	

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
	//		if (renderer.sharedMaterial==null) renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
	//		tempMaterial = new Material(renderer.sharedMaterial);
	//		tempMaterial.shader = Shader.Find("EasySprite2D/GrayScale_EasyS2D");
			tempMaterial = new Material(Shader.Find("EasySprite2D/GrayScale_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif
		renderer.sharedMaterial.SetFloat("_EffectAmount", _EffectAmount);
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
