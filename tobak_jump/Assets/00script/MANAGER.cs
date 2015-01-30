using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
		private int mScore;
		private string STATE = "READY";
		public static MANAGER mInstance;
		public GameObject prf_ui_ready;

		public MANAGER ()
		{
				mInstance = this;
		}
	
		public static MANAGER instance {
				get {
						if (mInstance == null)
								new MANAGER ();
						return mInstance;
				}
		}

		void Start ()
		{
				
		}

		public string state {
				get {
						return STATE;
				}
				set {
						STATE = value;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				switch (STATE) {
				case  "READY":
//						manager.ready ();
						ready ();
						STATE = "WAIT";
						break;
				case  "WAIT":

						break;
				case  "START":
						STATE = "IDLE";
						break;
				case  "IDLE":

						break;
				case  "JUMPING":
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

		public int score {
				get {
						return mScore;
				}set {
						mScore = value;
				}
		}

		void ready ()
		{
				Instantiate (prf_ui_ready, new Vector2 (0, 0), Quaternion.identity);
				gameReset ();
		}

	
		//RESET
		void gameReset ()
		{
				 
		}

		void gameStart ()
		{
		}
	
		public void gameFail ()
		{	
			 
		}
	
		 
	
	
	 
	
		//SCORE
		void scoreCount ()
		{
				 
		
		}
	
}
