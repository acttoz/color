﻿using UnityEngine;
using System.Collections;

public class Pdh_active : MonoBehaviour {

	public GameObject MyContent;


	
	public bool myBool;
	
	// Use this for initialization
	void Start () {
	
		myBool = true;
		
		MyContent.SetActive(false);
		
		
	}


	// Update is called once per frame
	void Update () {
		
		if(myBool == true)
		{
		
			MyContent.SetActive(false);
		}
		
		if(myBool == false)
		{

			MyContent.SetActive (true);
		}



	}
	
	void ShowContent22()
	{
		myBool =! myBool;
		 
		
	}
	
	
}