using UnityEngine;
using System.Collections;

public class OnStart : MonoBehaviour
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
		GameObject.Find ("GAMEMANAGER").SendMessage ("onStart");
		
	}
}
