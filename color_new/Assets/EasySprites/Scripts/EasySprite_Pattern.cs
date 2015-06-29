/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Pattern -1.2- by VETASOFT 2014
//////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Pattern")]

public class EasySprite_Pattern : MonoBehaviour {

	[Range(0, 1)]
	public float _Alpha = 1f;
	public Texture2D __MainTex2; 
	Material tempMaterial;
	public float _OffsetX;
	public float _OffsetY;

	public bool _AutoScrollX;
	public float _AutoScrollSpeedX;
	public bool _AutoScrollY;
	public float _AutoScrollSpeedY;
	private float _AutoScrollCountX;
	private float _AutoScrollCountY;

	private Texture2D image;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Pattern_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_OffsetX",_OffsetX);
		renderer.sharedMaterial.SetFloat("_OffsetY",_OffsetY);
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		if (__MainTex2)	__MainTex2.wrapMode= TextureWrapMode.Repeat;
		renderer.sharedMaterial.SetTexture ("_MainTex2", __MainTex2);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{ 
			tempMaterial = new Material(Shader.Find("EasySprite2D/Pattern_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
			if (__MainTex2)	__MainTex2.wrapMode= TextureWrapMode.Repeat;
			renderer.sharedMaterial.SetTexture ("_MainTex2", __MainTex2);
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);

		if ((_AutoScrollX == false) && (_AutoScrollY == false))
		{
			renderer.sharedMaterial.SetFloat("_OffsetX",_OffsetX);
			renderer.sharedMaterial.SetFloat("_OffsetY",_OffsetY);
		}

		if ((_AutoScrollX == true) && (_AutoScrollY == false))
	    {
			_AutoScrollCountX+=_AutoScrollSpeedX*Time.deltaTime;
			renderer.sharedMaterial.SetFloat("_OffsetX",_AutoScrollCountX);
			renderer.sharedMaterial.SetFloat("_OffsetY",_OffsetY);
		}
		if ((_AutoScrollX == false) && (_AutoScrollY == true))
		{
			_AutoScrollCountY+=_AutoScrollSpeedY*Time.deltaTime;
			renderer.sharedMaterial.SetFloat("_OffsetX",_OffsetX);
			renderer.sharedMaterial.SetFloat("_OffsetY",_AutoScrollCountY);
		}
		if ((_AutoScrollX == true) && (_AutoScrollY == true))
		{
			_AutoScrollCountX+=_AutoScrollSpeedX*Time.deltaTime;
			renderer.sharedMaterial.SetFloat("_OffsetX",_AutoScrollCountX);
			_AutoScrollCountY+=_AutoScrollSpeedY*Time.deltaTime;
			renderer.sharedMaterial.SetFloat("_OffsetY",_AutoScrollCountY);
		}
		if (_AutoScrollCountX > 1) _AutoScrollCountX = 0;
		if (_AutoScrollCountX < -1) _AutoScrollCountX = 0;
		if (_AutoScrollCountY > 1) _AutoScrollCountY = 0;
		if (_AutoScrollCountY < -1) _AutoScrollCountY = 0;
	}

	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
