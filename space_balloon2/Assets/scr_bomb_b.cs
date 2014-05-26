﻿using UnityEngine;
using System.Collections;

public class scr_bomb_b : MonoBehaviour
{
		public int direction;
		Vector3 mDirection;
		public float speed;
		//     2
		//  1     3
		//     4 

		// Use this for initialization
		void Start ()
		{
				switch (direction) {
				case 1:
						mDirection = new Vector3 (-speed, 0, 0);
						break;
				case 2:
						mDirection = new Vector3 (0, speed, 0);
						break;
				case 3:
						mDirection = new Vector3 (speed, 0, 0);
						break;
				case 4:
						mDirection = new Vector3 (0, -speed, 0);
						break;

				}
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position -= mDirection;

		}
}