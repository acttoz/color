﻿using UnityEngine;
using System.Collections;

public class buzzes : MonoBehaviour
{
		GameObject oScore;
		public GameObject oEffectScore;
		bool getScore = false;
		private float speed = 15;
		// Use this for initialization
		void Start ()
		{
				oScore = GameObject.Find ("score");	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (getScore) {
						float step = speed * Time.deltaTime;

						transform.position = Vector3.MoveTowards (transform.position, new Vector3 (3.3f, 2.4f, 0), step);
						if (transform.position == new Vector3 (3.3f, 2.4f, 0)) {
								Instantiate (oEffectScore, this.transform.position, Quaternion.identity);
								Destroy (this.gameObject);
						}
				}
	
	
		}

		void score ()
		{
				getScore = true;
		}
}

