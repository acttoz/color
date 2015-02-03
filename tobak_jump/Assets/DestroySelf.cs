using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (this.transform.position.y < -6)
						Destroy (this.gameObject);
		}
}
