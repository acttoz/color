using UnityEngine;
using System.Collections;

public class BackBtn : MonoBehaviour
{
		public bool noTrail = false;
		public string loadTo;

		// Use this for initialization
		void Start ()
		{
				if (noTrail == true) {
//						GameObject[] trails = GameObject.Find ("OrbitTrail");
//						for (int i=0; i<trails.Length; i++) {
//								Destroy (trails [i]);
//						}
				}
				loadTo = "main";
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) {

						if (loadTo.Equals ("main")) {
								Application.LoadLevel (0);
						} else {
								Application.Quit ();
						}
		


				}
		}
}
