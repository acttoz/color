using UnityEngine;
using System.Collections;

public class BtnMainStart : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTap ()
	{
		GetComponent<tk2dSprite> ().spriteId = GetComponent<tk2dSprite> ().GetSpriteIdByName ("btn2");
		Application.LoadLevelAsync (1);
	}
}
