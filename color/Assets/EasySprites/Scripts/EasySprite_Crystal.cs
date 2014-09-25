////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Crystal  -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Crystal")]
public class EasySprite_Crystal : MonoBehaviour {

	[Range(0, 1)]
	public float _Alpha = 1.0f;
	public float _OffsetX =1.0f;
	public float _OffsetY =1.0f;
	Material tempMaterial;

	[Range(0, 8)]
	public float _Factor=6.0f;
	public bool _Animate;
	[Range(0, 500)]
	public float _Speed=100f;
	[Range(0, 10)]
	public float _Distance=1f;
	private float _AutoSin;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Crystal_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_OffsetX",_OffsetX);
		if (_Factor == 0) _Factor = 0.0001f;
		renderer.sharedMaterial.SetFloat("_Factor",8-_Factor);
		renderer.sharedMaterial.SetFloat("_OffsetY",_OffsetY);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Crystal_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		_AutoSin+=Time.deltaTime*(_Speed);
		_AutoSin = _AutoSin % 360.0f;

		if (_Animate==true) _OffsetX=_Distance*Mathf.Sin (_AutoSin*Mathf.PI/180.0f);
		renderer.sharedMaterial.SetFloat("_OffsetX",_OffsetX);
		if (_Factor == 0) _Factor = 0.0001f;
		renderer.sharedMaterial.SetFloat("_Factor",8-_Factor);
		renderer.sharedMaterial.SetFloat("_OffsetY",_OffsetY);
	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
