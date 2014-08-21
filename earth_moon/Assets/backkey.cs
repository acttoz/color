using UnityEngine;
using System.Collections;

public class backkey : MonoBehaviour
{
		public string LEVEL;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) {

						if (LEVEL.Equals ("EXIT")) {
								Application.Quit ();
						} else {
								Application.LoadLevel (LEVEL);
						}


				}
		}
}
