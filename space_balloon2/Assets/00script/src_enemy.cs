﻿using UnityEngine;
using System.Collections;

public class src_enemy : MonoBehaviour
{
//	Random.Range(1, 5);
		public bool isBoss;
		public GameObject pop;
//		bool exist = true;
		string color;
		public Sprite[] sprite = new Sprite[3];
		public float speed;
		int xTemp = 1;
		int yTemp = 1;
		Vector3 velocity;
		int superLevel = 1;
		public float increaseRate;
		// Use this for initialization
		void Awake ()
		{
				if (!isBoss) {
						speed = Random.Range (5, 12) / 10f;
//						switch (Random.Range (1, 3)) {
//						case 1:
//								color = "b";
//								this.GetComponentInChildren<SpriteRenderer> ().sprite = sprite [0];
//								break;
//						case 2:
//								color = "o";
//								this.GetComponentInChildren<SpriteRenderer> ().sprite = sprite [1];
//								break;
//						case 3:
//								color = "p";
//								this.GetComponentInChildren<SpriteRenderer> ().sprite = sprite [2];
//								break;
//
//						}
				}
//				Debug.Log ("" + speed);
				if (Random.Range (0, 2) == 0) {
						xTemp = -1;
				}
				if (Random.Range (0, 2) == 0) {
						yTemp = -1;
				}
				rigidbody.velocity = new Vector3 (xTemp * speed, yTemp * speed, 0);
		}

		void Start ()
		{

		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		void superMode (int num)
		{

				
				switch (num) {
				
				case 1:
						if (superLevel == 2)
								rigidbody.velocity -= rigidbody.velocity.normalized / 10 * 20 * increaseRate;
						if (superLevel == 3)
								rigidbody.velocity -= rigidbody.velocity.normalized / 10 * 40 * increaseRate;
						superLevel = 1;
						break;
				case 2:
						superLevel = 2;
						rigidbody.velocity = rigidbody.velocity + rigidbody.velocity.normalized / 10 * 20 * increaseRate;
//						Debug.Log ("enemy_superlevel2");
						break;
				case 3:
						superLevel = 3;
						rigidbody.velocity += rigidbody.velocity.normalized / 10 * 20 * increaseRate;
//						Debug.Log ("enemy_superlevel3");
						break;
				
				default:
						break;
				
				
				}
				
//				Debug.Log ("enemy_velocity" + rigidbody.velocity.normalized);
			
		}

		void OnTriggerStay (Collider myTrigger)
		{
				if (!isBoss) {
						if (myTrigger.transform.tag == "bomb") {
								Instantiate (pop, transform.position, Quaternion.identity);
								Destroy (this.gameObject);
				
				
						}
//						if (myTrigger.transform.tag == "bomb_o" && color.Equals ("o")) {
//				
//								Instantiate (pop, transform.position, Quaternion.identity);
//								Destroy (this.gameObject);
//				
//				
//						}
//						if (myTrigger.transform.tag == "bomb_p" && color.Equals ("p")) {
//				
//								Instantiate (pop, transform.position, Quaternion.identity);
//								Destroy (this.gameObject);
//				
//				
//						}
				}
		
		}

		void OnTriggerEnter (Collider myTrigger)
		{
				if (!isBoss) {
						if (myTrigger.transform.tag == "bomb") {
								Instantiate (pop, transform.position, Quaternion.identity);
								Destroy (this.gameObject);
				
				
						}
						//						if (myTrigger.transform.tag == "bomb_o" && color.Equals ("o")) {
						//				
						//								Instantiate (pop, transform.position, Quaternion.identity);
						//								Destroy (this.gameObject);
						//				
						//				
						//						}
						//						if (myTrigger.transform.tag == "bomb_p" && color.Equals ("p")) {
						//				
						//								Instantiate (pop, transform.position, Quaternion.identity);
						//								Destroy (this.gameObject);
						//				
						//				
						//						}
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