﻿using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
		public float limit;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (this.transform.position.y < limit)
						Destroy (this.gameObject);
		}
}