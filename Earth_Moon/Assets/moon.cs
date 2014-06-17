using UnityEngine;
using System.Collections;

public class moon : MonoBehaviour
{
		public GameObject LookAtTo;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.LookAt (LookAtTo.transform.position);
		}
}
