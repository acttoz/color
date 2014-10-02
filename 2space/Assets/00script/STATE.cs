using UnityEngine;
using System.Collections;

public class STATE : MonoBehaviour
{
 
		public static string _STATE = "READY";
		public static bool isTouched = false;
		public static int mats = 0;
		public static int matsAll = 0;
		public static int stars = 3;
		tk2dTextMesh stateText;
		scr_manager manager;
		// Use this for initialization
		void Start ()
		{
				STATE._STATE = "READY";
				stateText = GetComponentInChildren<tk2dTextMesh> ();
				manager = GetComponent<scr_manager> ();
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				stateText.text = _STATE;
				switch (_STATE) {
				case  "READY":
//						manager.ready ();
						_STATE = "WAIT";
						break;
				case  "WAIT":

						break;
				case  "START":
//						manager.reset ();
						_STATE = "gIDLE";
						break;
				case  "NEXT":
						break;
				case  "gIDLE":
						break;
				case  "gTOUCH":
						break;
				case  "gSUCCESS":
//						manager.success ();
						_STATE = "WAIT";
						break;
				case  "gFAIL":
//						manager.fail ();
						_STATE = "WAIT";
						break;
				
				}
		}

 
}
