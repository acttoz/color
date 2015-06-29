/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Circle Fade -1.2- by VETASOFT 2014
//////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Circle Fade")]

public class EasySprite_Circle_Fade : MonoBehaviour {


	[Range(0, 1)]
	public float _Alpha = 1f;
	[Range(-0.2f, 1f)]
	public float _Offset = 0.5f;
	[Range(0, 1)]
	public int _InOut = 0;

	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Circle_Fade_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_Offset", _Offset);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Circle_Fade_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_Offset", _Offset);
		renderer.sharedMaterial.SetInt("_InOut", _InOut);

	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
