﻿using UnityEngine;
using System.Collections;

public class scr_toStart : MonoBehaviour
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
				if (STATE._STATE.Equals ("WAIT")) {
						STATE._STATE = "START";
						Destroy (this.gameObject);
				}

		}
}
