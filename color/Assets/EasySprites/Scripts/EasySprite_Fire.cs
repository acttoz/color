////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Fire  -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Fire")]
public class EasySprite_Fire : MonoBehaviour {


	[Range(0, 1)]
	public float _Alpha = 1f;
	[Range(32f, 256f)]
	public float _Offset = 80f;
	[Range(0.005f, 0.009f)]
	public float _Distance = 0.006f;
	[Range(0f, 12.56f)]
	public float OffSet_WaveTime=5f;
	[Range(0f, 12.56f)]
	public float _WaveTime = 2f;

	public bool AutoPlayWave=false;
	[Range(0f, 10f)]
	public float AutoPlaySpeed=5f;
	//public bool WarpMode=false;


	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Fire_EasyS2D"));
		//tempMaterial.mainTexture.wrapMode= TextureWrapMode.Repeat;

		renderer.sharedMaterial = tempMaterial;
	
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_OffsetX", _Offset);
		renderer.sharedMaterial.SetFloat("_DistanceX", _Distance);
		renderer.sharedMaterial.SetFloat("_WaveTimeX", _WaveTime);
		if (AutoPlayWave) _WaveTime = OffSet_WaveTime;
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Fire_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_OffsetX", _Offset);
		renderer.sharedMaterial.SetFloat("_DistanceX", _Distance);
		renderer.sharedMaterial.SetFloat("_WaveTimeX", _WaveTime);
		if (AutoPlayWave) _WaveTime += (AutoPlaySpeed*Random.Range(0.5f,2f) ) * Time.deltaTime;
		if (_WaveTime > 12.56f) _WaveTime = 0f;
	
	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
