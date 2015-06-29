////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Gradient  -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Gradient")]

public class EasySprite_Gradient : MonoBehaviour {

	public Color _Color = new Color (0f, 0f, 0f, 1f);
	public Color _Color2 = new Color (1f, 1f, 1f, 1f);
	[Range(0, 1)]
	public float _Alpha = 1f;
	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Gradient_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetColor("_Color", _Color);
		renderer.sharedMaterial.SetColor("_Color2", _Color2);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Gradient_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}		
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetColor("_Color", _Color);
		renderer.sharedMaterial.SetColor("_Color2", _Color2);
	}

	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}
}
