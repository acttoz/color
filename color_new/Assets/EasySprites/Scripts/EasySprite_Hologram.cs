/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Hologram 1 -1.2- by VETASOFT 2014
//////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Hologram Additive")]

public class EasySprite_Hologram : MonoBehaviour {


	[Range(0, 1)]
	public float _Alpha = 1f;
	[Range(0, 4)]
	public float Distortion = 1.0f;
	private float _TimeX = 0;
	[Range(0, 3)]
	public float Speed = 1;
	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Hologram_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		_TimeX+=Time.deltaTime*Speed;
		if (_TimeX>100)  _TimeX=0;
		renderer.sharedMaterial.SetFloat("_Distortion", Distortion);
		renderer.sharedMaterial.SetFloat("_TimeX", _TimeX);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Hologram_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		_TimeX+=Time.deltaTime*Speed;
		if (_TimeX>100)  _TimeX=0;
		renderer.sharedMaterial.SetFloat("_Distortion", Distortion);
		renderer.sharedMaterial.SetFloat("_TimeX", _TimeX);

	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
