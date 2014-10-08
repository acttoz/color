using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
 
		public static string STATE = "READY";
		public static float mUp, mDown, mLeft, mRight;
		public static bool isTouched = false;
		public static int mats = 0;
		public static int matsAll = 0;
		public static int stars = 3;
		tk2dTextMesh stateText;
		Play manager;
		// Use this for initialization
		void Start ()
		{
				

				mUp = 12;
				mLeft = -7;
				mDown = mUp * -1;
				mRight = mLeft * -1;
				STATE = "IDLE";
				stateText = GameObject.Find ("state").GetComponent<tk2dTextMesh> ();
				manager = GetComponent<Play> ();

				
		}
	
		// Update is called once per frame
		void Update ()
		{
				stateText.text = STATE;
				switch (STATE) {
				case  "READY":
//						manager.ready ();
						STATE = "WAIT";
						break;
				case  "WAIT":

						break;
				case  "START":
//						manager.reset ();
						STATE = "IDLE";
						break;
				case  "NEXT":
						break;
				case  "IDLE":

						break;
				case  "PLAY":
			
						break;
				case  "UNDEAD":
						break;
				case  "TOUCH":
						break;
				case  "SUCCESS":
//						manager.success ();
						STATE = "WAIT";
						break;
				case  "FAIL":
//						manager.fail ();
						STATE = "WAIT";
						break;
				
				}
		}

 
}
