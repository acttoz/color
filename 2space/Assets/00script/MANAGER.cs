using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
		private string STATE = "READY";
		public static float mUp, mDown, mLeft, mRight;
//		public static int  = 0;
//		public static int matsAll = 0;
//		public static int stars = 3;
		tk2dTextMesh stateText;
		GameObject bgm;
		public GameObject   backFirst, backStart, oTimeUp, effectPop, mainCamera;
		public Color colSky;
		int tempLevel;
		int mScore = 0;
		int spaceId = 0;
		public int[] spaceHeight;
		public int scoreRate;
		public tk2dTextMesh scoreText;
		public static MANAGER mInstance;

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
				

				mUp = 12;
				mLeft = -7;
				mDown = mUp * -1;
				mRight = mLeft * -1;
				STATE = "IDLE";
				stateText = GameObject.Find ("state").GetComponent<tk2dTextMesh> ();

				
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

		public int score {
				get {
						return mScore;
				}set {
						mScore = value;
				}
		}
	
		void gameStart ()
		{
				Instantiate (backFirst, new Vector2 (0, 0), Quaternion.identity);
		}
	
		//RESET
		public void gameReset ()
		{
				int q = 1;
				bgm = GameObject.Find ("BGM");
				bgm.SendMessage ("superMode", q);
				Level.instance.superLevel = 0;
				tempLevel = Level.instance.superLevel;
				mainCamera.camera.clearFlags = CameraClearFlags.SolidColor;
		
				Background.instance.reset ();
				Item.instance.reset ();
				Enemy.instance.reset ();
				Level.instance.reset ();
		
				mainCamera.camera.backgroundColor = colSky;
		
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						Destroy (tempZone);
		
		
				for (int i=0; i<10; i++) {
						Enemy.instance.InitEnemy ();
				}
		
				mScore = 0;
		
				MANAGER.instance.state = "IDLE";
				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
		
		}
	
		public void gameFail ()
		{	
				GameObject otimesup = Instantiate (oTimeUp, new Vector2 (0, 0), Quaternion.identity) as GameObject;
				otimesup.transform.parent = GameObject.Find ("UI").transform;
				otimesup.transform.localPosition = new Vector2 (0, 0);
		}
	
		public IEnumerator Remove ()
		{
				
				GameObject ep = (GameObject)GameObject.Instantiate (effectPop);
				ep.transform.position = Play.instance.balloonPosition;
				yield return new WaitForSeconds (0.5f);
				gameFail ();
		}
	
	
	
	 
	
		//SCORE
		void scoreCount ()
		{
				score += scoreRate;
				scoreText.text = " :  " + score;
		
		
				if (mScore > spaceHeight [spaceId]) {
						Background.instance.planet (spaceId);
						spaceId++;
				}
		
		}
	
}
