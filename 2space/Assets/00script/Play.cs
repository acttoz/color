using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour
{
		public GameObject balloon;
		private int tempLevel;
		public int scoreRate;
		public float stopRateControl;
		public int superTime;
		public float[] enemyRates;
		public int[] scoreRates;
//		int failTime = 3;
//		bool seenTutorial1 = false;
		Component admob;
		public Sprite[] sItems;
		public float[] levelRate = new float[4];
		float stopRate = 0;
		public GameObject itemShield;
		public int spaceId = 0;
		public GameObject[] monsterIcons;
		public Sprite[] ufos;
		public GameObject[] spaces = new GameObject[13];
		public GameObject[] stars = new GameObject[6];
		public float[] spacesHeight = new float[12];
		public bool test;
		public float undeadTime = 0;
		public GameObject mPoint, oBGM, oTutorial1, enemyPop, prf_FailTimer, oFailTimer, warn_boss, prf_boss, lightSpeed, oBoss, backFirst, testBack, prf_enemy, backElement, backStars, oStars, mainCamera;
		GameObject[] back;
		public bool isUndead = false;
//		float deadLine;
//		bool isUndead = false;
		private Vector2 zonePosition;
//		public GameObject[] oAirs = new GameObject[3];
		public GameObject oZone, oEnergy, oStopSound, btnRestart, btnNext, itemBlue, itemOrange, itemPurple, monsterEffect, super_back2;
		public Sprite bStar, oStar, pStar, eStar, superBalloon4, superBalloon5;
		public GameObject itemEffectO, itemEffectB, effectPoint2, effectStop, itemEffectP, itemEffectBack, btn_pause, prf_pause, btn_resume;
		GameObject existItem, createItem, btn_menu, btn_replay;
//		int timer;
		Vector2 previousBalloon, currentBalloon;
//		int gameTime = 3;
		public int leftTime;
		bool onPlay;
		Sprite tempStar;
		SpriteRenderer balloonSprite;
		public GameObject backStart, bgm, gauge, lv, oTimeUp;
		public GameObject effectSuper1, effectSuper2, effectPop, effectPoint, effectPointBack;
		public Vector3 backSize;
		public Vector3 balloonSize;
		int numHave = 0;
		GameObject[] enemy, realEnemy;
		int superTimer;
		public AudioClip create, remove, pop, bing, levelUp, go, itemSound, timesup;
		tk2dTextMesh scoreText;
		tk2dTextMesh lvText;
		tk2dTextMesh timeText, resultText, gemText1;
		float mScore = 0;
		int gem = 0;
		public static Play mInstance;

		public Play ()
		{
				mInstance = this;
		}
	
		public static Play instance {
				get {
						if (mInstance == null)
								new Play ();
						return mInstance;
				}
		}

		public Vector2 balloonPosition {
				get {
						return balloon.transform.position;
				}
		}

		public void reset ()
		{
				gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
		
		}

		public void Create (Vector3 touch)
		{
				 	
//				timer--;
//				oAirs [timer].animation.Play ();
//				Level.instance.superLevel = 1;
//				if (item1 == 1)
//						Level.instance.superLevel = 2;
//		Level.instance.superLevel = 5;
		
				//TEST
//				Level.instance.superLevel = 5;
//				Level.instance.superLevel = 3;
				Instantiate (balloon, touch, Quaternion.identity);
				InvokeRepeating ("balloonStop", 0.1f, 0.1f);
//				balloon.SendMessage ("create", Level.instance.superLevel);
//				Debug.Log ("create Level" + Level.instance.superLevel);

		
		}
		
		void Start ()
		{
			
//				countGem1 = PlayerPrefs.GetInt ("NUMGEM1");
				scoreText = GameObject.Find ("score").GetComponent<tk2dTextMesh> ();
				lvText = GameObject.Find ("lv").GetComponent<tk2dTextMesh> ();
				MANAGER.instance.gameReset ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				if (Application.platform == RuntimePlatform.Android) {
						if (Input.GetKey (KeyCode.Escape)) {
								if (onPlay) {
										onPlay = false;
										MANAGER.instance.state = "PAUSE";
								}
				
				
								return;
				
						}
				}
		
		}

		void balloonStop ()
		{
				if (MANAGER.instance.state.Equals ("PLAY")) {
			
						previousBalloon = zonePosition;
						currentBalloon = balloon.transform.position;
						Vector2 tempVector = currentBalloon - previousBalloon;
						float zoneLimit = 1.8f;
			
						if (-1 * zoneLimit < tempVector.x && tempVector.x < zoneLimit && -1 * zoneLimit < tempVector.y && tempVector.y < zoneLimit) {
								//								balloon.SendMessage ("stopBalloon", true);
								oEnergy.animation.Play ();
							
								//								Instantiate (effectStop, balloon.transform.position, Quaternion.identity);
								stopRate = stopRateControl;
						} else {
								//								balloon.SendMessage ("stopBalloon", false);
								oEnergy.animation.Stop ();
								oEnergy.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
								stopRate = 0;
						}
				}
				//		else if (MANAGER.instance.state.Equals ("PLAY") && Level.instance.superLevel == levelLimit) {
				//						//						balloon.SendMessage ("stopBalloon", false);
				//						oEnergy.animation.Stop ();
				//						oStopSound.audio.Stop ();
				//						stopRate = 0;
				//				}  
		}
	
		void superModeCount ()
		{
		
				if (gauge.transform.localScale.y > 1.75f) {
						CancelInvoke ("superModeCount");
						gauge.transform.localScale = new Vector3 (1.75f, 1.75f, 1);
			
						//						if (Level.instance.superLevel < levelLimit) {
						gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
						Level.instance.superLevel++;
						//								Debug.Log ("supermode(" + Level.instance.superLevel);
						Level.instance.superMode (Level.instance.superLevel);
						//						}
				} else {
			
						gauge.transform.localScale += new Vector3 (0, superTimer / 5000f * stopRate, 0);
						if (stopRate != 0)
								Instantiate (itemEffectBack, balloon.transform.position, Quaternion.identity);
			
				}
		}
		/************************ Control **********************/
		bool isCounting = false;
		int dragFingerIndex = -1;
	
		void OnDrag (DragGesture gesture)
		{
				// first finger
				FingerGestures.Finger finger = gesture.Fingers [0];
		
		
				if (MANAGER.instance.state.Equals ("PLAY")) {
						if (gesture.Phase == ContinuousGesturePhase.Started) {
								// remember which finger is dragging balloon
								dragFingerIndex = finger.Index;
				
								// spawn some particles because it's cool.
								//				 SpawnParticles (balloon);
						} else if (finger.Index == dragFingerIndex) {  // gesture in progress, make sure that this event comes from the finger that is dragging our balloon
								if (gesture.Phase == ContinuousGesturePhase.Updated) {
										// update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
									 
										balloon.transform.position = GetWorldPos (gesture.Position);
								} else {
										// reset our drag finger index
										dragFingerIndex = -1;
								}
						}
				}
		}
	
		void OnTap (TapGesture e)
		{
				Debug.Log (e.Selection.name);
				if (e.Selection.name == "btn_menu") {
						//						btn_menu.GetComponent<SpriteRenderer> ().color = Color.yellow;
						Time.timeScale = 1.0f;
						//						Game.instance.gameReset();
						Application.LoadLevel (1);
				}
				if (e.Selection.name == "btn_replay") {
						//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.yellow;
						int adNum;
						adNum = PlayerPrefs.GetInt ("ADTIME", 0);
						adNum++;
						PlayerPrefs.SetInt ("ADTIME", adNum);
			
						//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
						balloon.transform.localScale = new Vector3 (0, 0, 0);
						balloon.SetActive (false);
						MANAGER.instance.gameReset ();
						//												Application.LoadLevel (0);
				}
				if (e.Selection.name == "btn_next") {
			
				}
		
		
		
				if (e.Selection.name == "btn_pause" && MANAGER.instance.state.Equals ("IDLE")) {
						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
						//							 					Application.LoadLevel (0);
						MANAGER.instance.state = "PAUSE";
				}
				//				if (e.Selection == btn_pause && onCount) {
				//						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
				//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
				//						StartCoroutine (pauseGame ());
				//						//							 					Application.LoadLevel (0);
				//				}
		
				if (e.Selection.name == "btn_resume") {
						//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.yellow;
						//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
			
						//												Application.LoadLevel (0);
				}
		
		
		
				//				Debug.Log ("click");
		}
	
		void OnFingerDown (FingerDownEvent e)
		{
				//				if (onToast) {
				//						if (e.Selection == monsterIcons [0]) {
				//
				//								if (!selectedMonster1)
				//										selectedMonsterNum++;
				//								selectedMonster1 = true;
				//								e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				//				
				//						}
				//						if (e.Selection == monsterIcons [1]) {
				//								if (!selectedMonster2)
				//										selectedMonsterNum++;
				//								selectedMonster2 = true;
				//								e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				//				
				//						}
				//						if (e.Selection == monsterIcons [2]) {
				//								if (!selectedMonster3)
				//										selectedMonsterNum++;
				//								selectedMonster3 = true;
				//								e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				//				
				//						}
				//				}
		
				if (MANAGER.instance.state.Equals ("IDLE") && e.Selection.name != "btn_pause") {
						MANAGER.instance.state = "PLAY";
						Play.instance.Create (GetWorldPos (e.Position));
				}
		
		}
		//FINGERUP
		void OnFingerUp (FingerUpEvent e)
		{
				//				if (e.Selection == btn_menu) {
				//			
				//				}
				//				if (e.Selection == btnNext) {
				//			
				//				}
				//				if (e.Selection == btn_resume) {
				//			
				//				}
				//				if (e.Selection == btn_replay) {
				//			
				//						//												Application.LoadLevel (0);
				//				}
				//				if (e.Selection == btn_pause && onPlay) {
				//			
				//			
				//						//												Application.LoadLevel (0);
				//				}
				//				if (existBalloon && onPlay) {
				//						CancelInvoke ("balloonStop");
				//						StartCoroutine (Remove (1));			
				//			
				//				}
				//		balloonRemove ();
				//				Debug.Log ("release");
		}
	
		public static Vector3 GetWorldPos (Vector2 screenPos)
		{
				Ray ray = Camera.main.ScreenPointToRay (screenPos);
		
				// we solve for intersection with z = 0 plane
				float t = -ray.origin.z / ray.direction.z;
		
				return ray.GetPoint (t);
		}

	
}
