﻿using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour
{

		float speed = 2;
		int xTemp = 1;
		int yTemp = 1;
		// Use this for initialization
		void Start ()
		{
	
		}

		void Awake ()
		{
				if (Random.Range (0, 2) == 0) {
						xTemp = -1;
				}
				if (Random.Range (0, 2) == 0) {
						yTemp = -1;
				}
				rigidbody.velocity = new Vector3 (xTemp * speed, yTemp * speed, 0);
//				rigidbody.velocity = new Vector3 (2f, 2f, 0);
		}
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter (Collider myTrigger)
		{
		 
				if (myTrigger.tag == "touch") {
						Destroy (myTrigger);
			STATE._STATE="READY";
						
				}
				if (myTrigger.gameObject.name == "down") {
			
						rigidbody.velocity = new Vector3 (rigidbody.velocity.x, -rigidbody.velocity.y, 0);
			
			
				}
		
				if (myTrigger.gameObject.name == "up") {
			
						rigidbody.velocity = new Vector3 (rigidbody.velocity.x, -rigidbody.velocity.y, 0);
			
			
				}
				if (myTrigger.gameObject.name == "left") {
			
						rigidbody.velocity = new Vector3 (-rigidbody.velocity.x, rigidbody.velocity.y, 0);
			
			
				}
				if (myTrigger.gameObject.name == "right") {
			
						rigidbody.velocity = new Vector3 (-rigidbody.velocity.x, rigidbody.velocity.y, 0);
			
			
				}
		}
}