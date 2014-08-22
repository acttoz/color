using UnityEngine;
using System.Collections;

public class Btn_Main : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnFingerDown ()
	{
		GetComponent<tk2dSprite> ().spriteId=GetComponent<tk2dSprite>().GetSpriteIdByName("tomain2");
		Application.LoadLevelAsync(0);
	}
}
