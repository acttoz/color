using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
		private int mScore;
		public string STATE = "IDLE";
		public static MANAGER mInstance;
		public GameObject prf_ui_ready, prf_ui_fail, prf_player;
		tk2dTextMesh stateText;

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
				stateText = GetComponent<tk2dTextMesh> ();
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
						Words.instance.reset ();
						STATE = "IDLE";
						break;
				case  "IDLE":
						break;
				case  "JUMPING":
						break;
				case  "SUCCESS":
						success ();
						STATE = "IDLE";
						break;
				case  "FAIL":
						fail ();
						STATE = "WAIT";
						break;
				
				}
				stateText.text = "" + mScore;
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
				Instantiate (prf_ui_ready, new Vector3 (0, 0, -10), Quaternion.identity);
				gameReset ();
		}

		void success ()
		{
				mScore += 10;
		}

		void fail ()
		{
				Instantiate (prf_ui_fail, new Vector3 (0, 0, -10), Quaternion.identity);
		}

	
		//RESET
		void gameReset ()
		{
				Words.instance.reset ();
				mScore = 0;
				Instantiate (prf_player, new Vector2 (0, 0), Quaternion.identity);
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
