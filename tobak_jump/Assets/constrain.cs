using UnityEngine;
using System.Collections;

public class constrain : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		void constrainPosition ()
		{
				transform.parent.position = transform.position;
		}
}
