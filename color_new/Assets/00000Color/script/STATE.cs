using UnityEngine;
using System.Collections;

public class STATE : MonoBehaviour
{
 
		public static string _STATE = "READY";
		public static bool isTouched = false;
		public static int mats = 0;//colored mat
		public static int matsAll = 0;//number of all mats
		public static int buzz = 0;//number of buzz
		public static int buzzScore = 0;//number of buzz
		public static int stars = 3;
		public static int LEVEL = 3;
		public static int SCORE = 0;
		private GameObject oCamera;
		tk2dTextMesh stateText;
		MANAGER manager;
		// Use this for initialization
		void Start ()
		{
				oCamera = GameObject.FindWithTag ("MainCamera");
				STATE._STATE = "READY";
				
//				Debug.Log ("" + matArray);
				stateText = GetComponentInChildren<tk2dTextMesh> ();
				manager = GetComponent<MANAGER> ();
				
		}
	 
	
		// Update is called once per frame
		void Update ()
		{
				stateText.text = _STATE;
				switch (_STATE) {
				case  "READY":
						manager.ready ();
						_STATE = "WAIT";
						break;
				case  "WAIT":
						break;
				case  "START":
						manager.reset ();

//						cameraHover (true);
						_STATE = "gIDLE";
						break;
				case  "NEXT":
						break;
				case  "gIDLE":
						break;
				case  "gTOUCH":
						break;
				case  "gSUCCESS":
//						LEVEL = Random.Range (1, 4);
//						cameraHover (false);
						manager.success ();
						_STATE = "WAIT";
						break;
				case  "gFAIL":
//						cameraHover (false);
						manager.fail ();
						_STATE = "WAIT";
						break;
				
				}
		}

		public void cameraHover (bool isHover)
		{
				if (isHover) {
						Debug.Log ("cameraHover");
//						oCamera.transform.position = new Vector3 (-1.5f, 1.5f, -50);
				}
				if (!isHover) {
						Debug.Log ("cameraHoverNot");
						oCamera.transform.position = new Vector3 (0, 0, -50);
				}
		}

 
}
