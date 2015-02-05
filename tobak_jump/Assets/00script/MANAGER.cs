using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
		private int mScore;
		public float fireTime;
		public string STATE = "IDLE";
		public static MANAGER mInstance;
		public GameObject prf_ui_ready, prf_ui_fail, prf_player, oBack, oFires, oToast, oToast_send;
		tk2dTextMesh stateText;
		public string inputText;

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
				case  "toast":
			
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

		public void touch (int direction)
		{
				Player.instance.touch (direction);
				Words.instance.jump (direction);
				MANAGER.instance.state = "JUMPING";
				oBack.SendMessage ("move");
				moveFire ();
		}

		void moveFire ()
		{
				StopCoroutine ("MoveObject");
				
				StartCoroutine ("MoveObject");
		
		}
	
		IEnumerator MoveObject ()
		{
				Vector3 endPos = new Vector3 (0, 5.5f, 0);
				Vector3 startPos = new Vector3 (0, 0, 0);
				Vector3 currentPos = oFires.transform.position;
				float i = 0.0f;
				float rate = 1.0f / 0.3f;
				while (i < 1.0f) {
						i += Time.deltaTime * rate;
						oFires.transform.position = Vector3.Lerp (oFires.transform.position, startPos, i);
						yield return null; 
				}
				yield return new WaitForSeconds (1f);
				i = 0.0f;
				rate = 1.0f / fireTime;
				while (i < 1.0f) {
						i += Time.deltaTime * rate;
						oFires.transform.position = Vector3.Lerp (startPos, endPos, i);
						yield return null; 
				}
				fail ();
				Player.instance.failAction ();
		
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
				StopCoroutine ("MoveObject");
		}

	
		//RESET
		void gameReset ()
		{
				Words.instance.reset ();
				mScore = 0;
				Instantiate (prf_player, new Vector2 (0, 0), Quaternion.identity);
				oBack.transform.position = new Vector2 (0, 0);
				oBack.GetComponent<MovingBack> ().position = 0;
				oFires.transform.position = new Vector2 (0, 0);
		}

		public void rankSubmit ()
		{
				if (!inputText.Equals (""))
						GetComponent<Rank> ().postRank (inputText, mScore);
				else
						toast (1);
		}

		public void toast (int num)
		{
				switch (num) {
				case 1:
						Instantiate (oToast, new Vector2 (0, 0), Quaternion.identity);
						break;
				case 2:
						Instantiate (oToast_send, new Vector2 (0, 0), Quaternion.identity);
						break;
				}
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
