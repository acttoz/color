using UnityEngine;
using System.Collections;

public class MANAGER2 : MonoBehaviour
{
		GameObject CAMERA;
		// Use this for initialization
		void Start ()
		{
				CAMERA = GameObject.FindGameObjectWithTag ("MainCamera2");
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void change ()
		{
				CAMERA.SendMessage ("change");
		}
}
