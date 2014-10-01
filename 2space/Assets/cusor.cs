using UnityEngine;
using System.Collections;

public class cusor : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void inputKey (string text)
		{
				GetComponentInChildren<tk2dTextMesh> ().text = text;
		}
}
