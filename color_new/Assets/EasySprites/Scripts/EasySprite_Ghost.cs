////////////////////////////////////////////////////////////////
/// EASY 2D SPRITES - Ghosting -1.2- by VETASOFT 2014
/////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("Easy Sprites 2D/Ghost")]
public class EasySprite_Ghost : MonoBehaviour {

	[Range(0, 1)]
	public float _Alpha = 1f;
	[Range(0f, 1f)]
	public float _offset = 0f;
	[Range(0f, 1f)]
	public float _ClipLeft = 0f;
	[Range(0f, 1f)]
	public float _ClipRight = 0f;
	[Range(0f, 1f)]
	public float _ClipUp = 0f;
	[Range(0f, 1f)]
	public float _ClipDown = 0f;

	Material tempMaterial;

	void Start () 
	{
		tempMaterial = new Material(Shader.Find("EasySprite2D/Ghost_EasyS2D"));
		renderer.sharedMaterial = tempMaterial;
		renderer.sharedMaterial.SetFloat("_ClipLeft", 1-_ClipLeft);
		renderer.sharedMaterial.SetFloat("_ClipRight", 1-_ClipRight);
		renderer.sharedMaterial.SetFloat("_ClipUp", 1-_ClipUp);
		renderer.sharedMaterial.SetFloat("_ClipDown", 1-_ClipDown);
		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_offset", _offset);
	}

	void Update () 
	{
		#if UNITY_EDITOR
		if (Application.isPlaying!=true)
		{
			tempMaterial = new Material(Shader.Find("EasySprite2D/Ghost_EasyS2D"));
			renderer.sharedMaterial = tempMaterial;
		}
		#endif

		renderer.sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
		renderer.sharedMaterial.SetFloat("_ClipLeft", 1-_ClipLeft);
		renderer.sharedMaterial.SetFloat("_ClipRight", 1-_ClipRight);
		renderer.sharedMaterial.SetFloat("_ClipUp", 1-_ClipUp);
		renderer.sharedMaterial.SetFloat("_ClipDown", 1-_ClipDown);
		renderer.sharedMaterial.SetFloat("_offset", _offset);

	}
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true) && (Application.isLoadingLevel == false))
			renderer.sharedMaterial.shader=Shader.Find("Sprites/Default");
		
	}

}
