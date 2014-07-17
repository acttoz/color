using UnityEngine;
using System.Collections;

public class scr_item : MonoBehaviour
{
		// Use this for initialization
		public float speed;

		void Start ()
		{
		}

		void Awake ()
		{
		
				rigidbody.velocity = new Vector3 (0, -speed, 0);

		}
		// Update is called once per frame
		void Update ()
		{
	
		}

		

}
