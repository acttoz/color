/////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Grass  -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Grass")]

public class EasySprite_Grass : MonoBehaviour {


	[Range(0, 1)]
	public float _Alpha = 1f;
	[Range(0f, 128f)]
	public float _OffsetX = 10f;
	[Range(0f, 128f)]
	public float _OffsetY = 20f;
	[Range(0f, 1f)]
	public float _DistanceX = 0.04f;
	[Range(0f, 1f)]
	public float _DistanceY = 0.07f;
	[Range(0f, 6.28f)]
	public float _WaveTimeX = 0f;
	[Range(0f, 6.28f)]
	public float _WaveTimeY = 0f;

	public bool AutoPlayWaveX=false;
	[Range(0f, 5f)]
	public float AutoPlaySpeedX=5f;
	public bool AutoPlayWaveY=false;
	[Range(0f, 50f)]
	public float AutoPlaySpeedY=5f;
	//public bool WarpMode=false;


	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Grass_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_OffsetX", _OffsetX);
		renderer.sharedMaterial.SetFloat("_OffsetY", _OffsetY);
		renderer.sharedMaterial.SetFloat("_DistanceX", _DistanceX);
		renderer.sharedMaterial.SetFloat("_DistanceY", _DistanceY);
		renderer.sharedMaterial.SetFloat("_WaveTimeX", _WaveTimeX);
		renderer.sharedMaterial.SetFloat("_WaveTimeY", _WaveTimeY);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Grass_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_OffsetX", _OffsetX);
		renderer.sharedMaterial.SetFloat("_OffsetY", _OffsetY);
		renderer.sharedMaterial.SetFloat("_DistanceX", _DistanceX);
		renderer.sharedMaterial.SetFloat("_DistanceY", _DistanceY);
		renderer.sharedMaterial.SetFloat("_WaveTimeX", _WaveTimeX);
		renderer.sharedMaterial.SetFloat("_WaveTimeY", _WaveTimeY);
		if (AutoPlayWaveX) _WaveTimeX += AutoPlaySpeedX * Time.deltaTime;
		if (AutoPlayWaveY) _WaveTimeY += AutoPlaySpeedY * Time.deltaTime;
		if (_WaveTimeX > 6.28f) _WaveTimeX = 0f;
		if (_WaveTimeY > 6.28f) _WaveTimeY = 0f;

	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
