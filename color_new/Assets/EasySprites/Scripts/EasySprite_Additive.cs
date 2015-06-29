/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Additive -1.2- by VETASOFT 2014
//////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Additive")]

public class EasySprite_Additive : MonoBehaviour {


	[Range(0, 1)]
	public float _Alpha = 1f;

	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Additive_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Additive_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
	
	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
