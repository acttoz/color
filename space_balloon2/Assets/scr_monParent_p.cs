using UnityEngine;
using System.Collections;

public class scr_monParent_p : MonoBehaviour
{
 
		public float speed;
		GameObject oBalloon;
		// Use this for initialization
		void Start ()
		{
				oBalloon = GameObject.Find ("boss");
		}
	
		// Update is called once per frame
		void Update ()
		{
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, oBalloon.transform.position, step);
				if (transform.position == oBalloon.transform.position) {
						oBalloon.SendMessage ("monsterP");
						Destroy (this.gameObject);
				}
		}

		
}

