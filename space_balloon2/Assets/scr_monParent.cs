using UnityEngine;
using System.Collections;

public class scr_monParent : MonoBehaviour
{
 
		public float speed;
		GameObject oBalloon;
		public GameObject bombO;
		public int isOrange;
		// Use this for initialization
		void Start ()
		{
				oBalloon = GameObject.FindGameObjectWithTag ("balloon");
		}
	
		// Update is called once per frame
		void Update ()
		{
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, oBalloon.transform.position, step);
				if (isOrange == 1 && transform.position == oBalloon.transform.position) {
						GameObject.Find ("GAMEMANAGER").SendMessage ("monsterOrange");
						Instantiate (bombO, transform.position, Quaternion.identity);
						Destroy (this.gameObject);

				}
		}

		
}

