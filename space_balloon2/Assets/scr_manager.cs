using UnityEngine;
using System.Collections;

public class scr_manager : MonoBehaviour
{
		public bool test;
		public float undeadTime = 0;
		public GameObject oBoss, backFirst, testBack, prf_enemy, backElement, oStars;
		GameObject[] back;
		public bool isUndead = false;
//		float deadLine;
//		bool isUndead = false;
		public float enemyCreateRate;
		private Vector2 zonePosition;
//		public GameObject[] oAirs = new GameObject[3];
		public GameObject oZone, oEnergy, oStopSound, btnRestart, balloon, itemBlue, itemOrange, itemPurple, monsterB, monsterO, monsterP, monsterEffect, super_back2;
		public Sprite bStar, oStar, pStar, eStar, superBalloon4, superBalloon5;
		public float stopRateControl;
		public float[] levelRate = new float[4];
		float stopRate = 0;
		public GameObject itemEffectO, itemEffectB, effectPoint2, effectStop, itemEffectP, itemEffectBack, walls, btn_pause, prf_pause, btn_resume;
//		int timer;
		Vector2 previousBalloon, currentBalloon;
//		int gameTime = 3;
		public float itemCreateRate;
		public float zoneCreateRate;
		public int leftTime;
		bool onPlay;
		int min, sec, countScore = 0, countGem = 0;
		Sprite tempStar;
		SpriteRenderer star1, star2, star3, balloonSprite;
		GameObject existItem, createItem, btn_menu, btn_replay;
		public GameObject backStart, bgm, gauge, lv, oTimeUp;
		public GameObject effectSuper1, effectSuper2, effectPop, effectPoint, effectPointBack;
		public Vector3 backSize;
		public Vector3 balloonSize;
		string colHave1 = "n", colCreate = "n";//b;o,p
		int numHave = 0;
		GameObject[] enemy, realEnemy;
		public int superTime;
		int superTimer;
		public AudioClip create, remove, pop, bing, levelUp, go, itemSound, timesup;
		tk2dTextMesh scoreText;
		tk2dTextMesh lvText;
		tk2dTextMesh timeText, resultText, gemText;
		float score = 0;
		int gem = 0;
		public static int superLevel = 0;
		float mUp, mDown, mLeft, mRight;
		// Use this for initialization
		bool existBalloon = false;
//		bool timeStarted = false;
//		bool isScoreUp = false;

		void Start ()
		{
//				balloon.transform.localScale = new Vector3 (0, 0, 0);
				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
//				score = 1000;
				countGem = PlayerPrefs.GetInt ("NUMGEM");
				if (test)
						Instantiate (testBack, new Vector2 (0, 0), Quaternion.identity);
						
//				timer = gameTime;
//				star1 = GameObject.Find ("star1").GetComponent<SpriteRenderer> ();
//				star2 = GameObject.Find ("star2").GetComponent<SpriteRenderer> ();
//				star3 = GameObject.Find ("star3").GetComponent<SpriteRenderer> ();
				balloonSprite = balloon.GetComponentInChildren<SpriteRenderer> ();
				scoreText = GameObject.Find ("score").GetComponent<tk2dTextMesh> ();
				lvText = GameObject.Find ("lv").GetComponent<tk2dTextMesh> ();
				superTimer = superTime;
				mUp = 5.5f;
				mDown = mUp * -1;
				existBalloon = false;
				mLeft = GameObject.Find ("lv").transform.position.x;
				mRight = mLeft * -1;
//				Debug.Log ("screenSize=" + mUp + " " + mDown + " " + mLeft + " " + mRight + " ");
				currentBalloon = new Vector2 (20, 20);
		
				//				backSize = back.renderer.bounds.size; 
				//				Debug.Log (backSize);	

				//test
//				btn_menu = GameObject.Find ("btn_menu");
//				btn_replay = GameObject.Find ("btn_replay");
				InvokeRepeating ("undeadTimer", 0, 0.25f);
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				

				if (Application.platform == RuntimePlatform.Android) {
						if (Input.GetKey (KeyCode.Escape)) {
								if (onPlay) {
										onPlay = false;
										StartCoroutine (pauseGame ());
								}
				
				
								return;
				
						}
				}
		
		}

		void undeadTimer ()
		{
				if (undeadTime < 1) {
						isUndead = false;
						realEnemy = GameObject.FindGameObjectsWithTag ("realenemy");
						for (int i=0; i<realEnemy.Length; i++) {
								realEnemy [i].GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1f);
						}
			
						
						if (balloon.activeInHierarchy)
								balloon.SendMessage ("undead", false);
				} else {
						isUndead = true;
						undeadTime -= 0.25f;
						
						if (balloon.activeInHierarchy)
								balloon.SendMessage ("undead", true);
						realEnemy = GameObject.FindGameObjectsWithTag ("realenemy");
						for (int i=0; i<realEnemy.Length; i++) {
								realEnemy [i].GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.5f);
						}
				}
		}
	
		 
	
	
	
	
	
	
	
	
		/// <summary>
		/////////////////////////////////////// Games the start.//////////////////////////////////////////////
		/// </summary>
		/// /
		/// /
		/// /
		/// /
		/// /
		/// /
		///                      START
		/// 
		/// 
		/// 
		/// 
		/// 
		void gameStart ()
		{
				audio.PlayOneShot (go);
				enableTouch ();
//				timeStarted = true;
				onPlay = true;
				InvokeRepeating ("itemCreate", 1f, itemCreateRate);
				InvokeRepeating ("enemyCreate", 1f, enemyCreateRate);
				 
				Instantiate (backFirst, new Vector2 (0, 0), Quaternion.identity);
		}

		void enemyCreate ()
		{
			 
				float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
				float tempY = (Random.Range (mDown * 100, mUp * 100)) / 100;
				
					
				Instantiate (prf_enemy, new Vector2 (tempX, tempY), Quaternion.identity);
		}

		void gameReset ()
		{
				
				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
				if (existItem != null)
						Destroy (existItem);
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						Destroy (tempZone);
				CancelInvoke ("zoneCreate");
				CancelInvoke ("enemyCreate");
				oBoss.transform.tag = "boss";
				enemy = GameObject.FindGameObjectsWithTag ("enemy");
				for (int i=0; i<enemy.Length; i++) {
						Destroy (enemy [i]);
				}
				oBoss.transform.tag = "enemy";
				oBoss.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				undeadTime = 0;
				CancelInvoke ("itemCreate");
				CancelInvoke ("zoneCreate");
				CancelInvoke ("scoreCount");
				CancelInvoke ("superModeCount");
				CancelInvoke ("normalModeCount");
				gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
				onPlay = false;
				score = 0;
				scoreText.text = ": " + score;
//				timeStarted = false;
				disableTouch ();
//				resetStar ();
//				timer = gameTime;
				///level reset
				lvText.text = "Lv.1";
				superTimer = superTime;
				superLevel = 0;
				bgm.SendMessage ("superMode", 1);
				existBalloon = false;
				// back & enemy reset
//				balloon.transform.localScale = new Vector3 (0, 0, 0);
				
//				oAirs [0].SetActive (true);
//				oAirs [1].SetActive (true);
//				oAirs [2].SetActive (true);
//				oAirs [0].transform.localScale = new Vector2 (0.3460798f, 0.3460798f);
//				oAirs [1].transform.localScale = new Vector2 (0.3460798f, 0.3460798f);
//				oAirs [2].transform.localScale = new Vector2 (0.3460798f, 0.3460798f);
//				enemy [0].SendMessage ("superMode", 1);
//				enemy [1].SendMessage ("superMode", 1);
//				enemy [2].SendMessage ("superMode", 1);
				
//				balloon.GetComponentInChildren<SphereCollider> ().enabled = true;

		}

		IEnumerator pauseGame ()
		{
				onPlay = false;
				Instantiate (prf_pause, new Vector2 (0, 0), Quaternion.identity);
				
				btn_menu = GameObject.Find ("btn_menu");
				btn_replay = GameObject.Find ("btn_replay");
				btn_resume = GameObject.Find ("btn_resume");
				disableTouch ();
				yield return new WaitForSeconds (0.5f);
				enableTouch ();
				Time.timeScale = 0;
				existBalloon = false;
		}

		IEnumerator timesUp ()
		{
				onPlay = false;
				
				balloon.SetActive (false);
				existBalloon = false;
				disableTouch ();
				audio.PlayOneShot (timesup);
				Instantiate (oTimeUp, new Vector2 (0, 0), Quaternion.identity);
				resultText = GameObject.Find ("numscore").GetComponent<tk2dTextMesh> ();
				gemText = GameObject.Find ("numgem").GetComponent<tk2dTextMesh> ();
				gemText.text = "" + countGem;
				yield return new WaitForSeconds (1f);
				countScore = 0;
				btn_menu = GameObject.Find ("btn_menu");
				btn_replay = GameObject.Find ("btn_replay");
				InvokeRepeating ("resultCount", 0f, 0.01f);
		
		}

		void resultCount ()
		{
				if (score < countScore) {
						audio.PlayOneShot (itemSound);
			
						CancelInvoke ("resultCount");

						resultText.text = "" + score;
						if (score >= 1000) {
								gem = (int)score / 1000;
								InvokeRepeating ("resultGemCount", 0.5f, 0.3f);
						} else {
								enableTouch ();
						}
				} else {
						audio.PlayOneShot (bing);
						resultText.text = "" + countScore;
						countScore += 50;
				}
				

		}

		void resultGemCount ()
		{
				audio.PlayOneShot (itemSound);
				countGem++;
				gem--;
				gemText.text = "" + countGem;
				resultText.text = "" + (score -= 1000);
				
				if (gem < 1) {

						CancelInvoke ("resultGemCount");
						PlayerPrefs.SetInt ("NUMGEM", countGem);
						enableTouch ();
				}  
		
		
		}

















		/// <summary>
		/////////////////////////////// Items the create.////////////////////////////////////
		/// </summary>
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 				ITEMS
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		void itemCreate ()
		{
				float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
				if (existItem != null)
						Destroy (existItem);
//				Debug.Log ("itemCreate");
				int tempCol = Random.Range (1, 4);
				//test
//				tempCol = 1;
				switch (tempCol) {
				case 1:
						colCreate = "b";
						createItem = itemBlue;
						tempStar = bStar;
						break;

				case 2:
						createItem = itemOrange;
						colCreate = "o";
						tempStar = oStar;
						break;

				case 3:
						createItem = itemPurple;
						colCreate = "p";
						tempStar = pStar;
						break;
				}
				
				
//				float tempY = (Random.Range (mDown * 100, mUp * 100)) / 100;
				if (!isUndead)
						existItem = Instantiate (createItem, new Vector3 (tempX, 7, 0), Quaternion.identity) as GameObject;
		}

		void backCreate ()
		{
				if (superLevel > 0 && superLevel < 6) {
						float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
						Instantiate (backElement, new Vector3 (tempX, 7, 0), Quaternion.identity);
				} else if (superLevel == 6 && score > 30) {
						float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
						Instantiate (backElement, new Vector3 (tempX, -7, 0), Quaternion.identity);
				}
		}

		void zoneCreate ()
		{
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						tempZone.animation.Play ("anim_zoneOut");
				float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
				float tempY = (Random.Range (mDown * 100, mUp * 100)) / 100;
				zonePosition = new Vector2 (tempX, tempY);
				Instantiate (oZone, new Vector3 (tempX, tempY, 0), Quaternion.identity);
		}

		void zoneReset ()
		{
				CancelInvoke ("zoneCreate");
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						tempZone.animation.Play ("anim_zoneOut");
		}

		void getItem ()
		{
				switch (numHave) {
				case 0:
						undeadTime = 2.5f;
						colHave1 = colCreate;
//						star1.sprite = tempStar;
						numHave++;
						audio.PlayOneShot (itemSound);
//						StartCoroutine ("monster", colCreate);
						if (colCreate.Equals ("b"))
								Instantiate (monsterB, new Vector2 (0, 0), Quaternion.identity);
						if (colCreate.Equals ("o"))
								Instantiate (monsterO, new Vector2 (0, 0), Quaternion.identity);
						if (colCreate.Equals ("p"))
								Instantiate (monsterP, new Vector2 (0, 0), Quaternion.identity);
			//						StartCoroutine ("undead", 6f);
//						StartCoroutine ("monster", colHave1);
//						StopCoroutine ("undead",4f);
						break;
			
				case 1:
						if (colHave1 == colCreate) {
//								numHave++;
//								StartCoroutine ("getAnim", GameObject.Find ("star2"));
//						
								balloon.SendMessage ("biggerBomb", true);		
								audio.PlayOneShot (itemSound);
//								star2.sprite = tempStar;
						} else {
								
								balloon.SendMessage ("biggerBomb", false);		
								Destroy (GameObject.FindGameObjectWithTag ("monster"));
								balloon.SendMessage ("resetMonster");
								numHave = 0;
								getItem ();
						}
						break;
		
//				case 2:
//						audio.PlayOneShot (itemSound);
//						if (colHave1 == colCreate) {
//								//monster
//								star3.sprite = tempStar;
//								StartCoroutine ("monster", colCreate);
////								StopCoroutine ("undead");
//								StartCoroutine ("undead", 5f);
//								//moster
//								
//
//						} else {
//								resetStar ();
//								getItem ();
//						}
//						break;
				}
				 

		}

		void itemUse (string col)
		{
				if (col.Equals ("b")) {
//						timer += 10;
						audio.PlayOneShot (levelUp);
						Instantiate (itemEffectBack, itemEffectB.transform.position, Quaternion.identity);
				}
				if (col.Equals ("o")) {
						audio.PlayOneShot (levelUp);
//						StopCoroutine ("scoreUp");
//						StartCoroutine ("scoreUp");
						Instantiate (itemEffectBack, itemEffectO.transform.position, Quaternion.identity);
				}
				if (col.Equals ("p")) {
						audio.PlayOneShot (levelUp);
						Instantiate (itemEffectBack, itemEffectP.transform.position, Quaternion.identity);
						if (gauge.transform.localScale.y > 1.3) {
								float temp = 1.74f - gauge.transform.localScale.y;
								gauge.transform.localScale += new Vector3 (0, temp, 0);
						} else {
								gauge.transform.localScale += new Vector3 (0, 0.5f, 0);
						}
						
				}
		}

		IEnumerator scoreUp ()
		{
//				isScoreUp = true;
				itemEffectO.animation.wrapMode = WrapMode.Loop;
				itemEffectO.animation.Play ();

				GameObject.Find ("score").GetComponent<tk2dTextMesh> ().color = new Color (1, 0.5f, 0);

				yield return new WaitForSeconds (10f);
				itemEffectO.animation.wrapMode = WrapMode.Once;
				itemEffectO.animation.Stop ();
				itemEffectO.transform.localScale = new Vector2 (1, 1);
				GameObject.Find ("score").GetComponent<tk2dTextMesh> ().color = new Color (1, 1, 1);
				scoreText.text = ": " + score;
//				isScoreUp = false;
		}

		IEnumerator monster (string mColHave)
		{
				//Stop item Create
				//stop enemyTrigger
				//enemy alpha
//				Debug.Log (mColHave);

//				oStars.animation.Play ();

//				yield return new WaitForSeconds (0.5f);
//				StartCoroutine ("getAnim", GameObject.Find ("star1"));
//				StartCoroutine ("getAnim", GameObject.Find ("star2"));
//				StartCoroutine ("getAnim", GameObject.Find ("star3"));
				 
			
		
		
				if (mColHave.Equals ("b"))
						Instantiate (monsterB, new Vector2 (0, 0), Quaternion.identity);
				if (mColHave.Equals ("o"))
						Instantiate (monsterO, new Vector2 (0, 0), Quaternion.identity);
				if (mColHave.Equals ("p"))
						Instantiate (monsterP, new Vector2 (0, 0), Quaternion.identity);
//				Instantiate (monsterEffect, new Vector2 (0, 0), Quaternion.identity);
				yield return new WaitForSeconds (1f);
//				resetStar ();
		}

//		void resetStar ()
//		{
//				star1.sprite = eStar;
//				star2.sprite = eStar;
//				star3.sprite = eStar;
//				numHave = 0;
//		}














		/// <summary>
		///////////////////////////////////////////////// Create & Remove	/// </summary>////////////////////////////////////////////////////////////////////
		/// <param name="touch">Touch.</param>
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 				Create
		/// 
		/// 
		/// 
		/// 
		/// 

		void Create (Vector3 touch)
		{
//				timer--;
//				oAirs [timer].animation.Play ();
				superLevel = 0;
				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 1);
				balloon.SetActive (true);
				balloon.SendMessage ("offMonster");
		
				balloon.transform.localRotation = new Quaternion (0, 0, 0, 0);
				balloon.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
				balloon.SendMessage ("undead", false);
				InvokeRepeating ("balloonStop", 0.1f, 0.1f);
				CancelInvoke ("decreaseEnergy");
				balloon.transform.position = touch;
				if (superLevel < 2) {
						superMode (superLevel);
				} else {
						superTimer = superTime;
						InvokeRepeating ("superModeCount", 0.1f, 0.1f);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [superLevel - 2] / 10);
				}
				balloon.SendMessage ("create", superLevel);
//				Debug.Log ("create Level" + superLevel);
				existBalloon = true;
				audio.PlayOneShot (create);
		
		}
	
		IEnumerator Remove (int num)
		{
				oStopSound.audio.Stop ();
				oEnergy.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
				CancelInvoke ("scoreCount");
				lv.SendMessage ("levelUp");
				lvText.text = "Lv.1";
				CancelInvoke ("superModeCount");
				CancelInvoke ("normalModeCount");
				stopRate = stopRateControl;
				oEnergy.animation.Stop ();
				zoneReset ();
				
				switch (num) {
				case 1:
						superLevel = 6;
			
//						deadLine = score - 20;
						 
						superMode (superLevel);
						balloon.SendMessage ("cancel", 1);

			//						if (audio.isPlaying)
						audio.Stop ();
						audio.PlayOneShot (remove);
			//decrease energy
						if (superLevel < 5)
								InvokeRepeating ("decreaseEnergy", 0.2f, 0.2f);
						
						break;
			
				case 2:
						existBalloon = false;
						balloon.transform.localScale = new Vector3 (0, 0, 0);
						audio.Stop ();
						audio.PlayOneShot (pop);
						balloon.SendMessage ("cancel", 2);
						undeadTime = 0;
			///level reset
						lvText.text = "Lv.1";
						gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
						superTimer = superTime;
						bgm.SendMessage ("superMode", 1);

			// back & enemy reset
			
						 
//						enemy [0].SendMessage ("superMode", 1);
//						enemy [1].SendMessage ("superMode", 1);
//						enemy [2].SendMessage ("superMode", 1);

			
						GameObject ep = (GameObject)GameObject.Instantiate (effectPop);
						ep.transform.position = balloon.transform.position;
//			ep.renderer.sortingLayerName = "ui";
					 
						balloon.SetActive (false);
						StartCoroutine ("timesUp");
						break;
				}
		
		
				yield return new WaitForSeconds (0.2f);
				if (existBalloon) {
						existBalloon = false;
						balloon.transform.localScale = new Vector3 (0, 0, 0);
			
						
			
			
						//						if (num == 1)
						balloon.SetActive (false);
//						if (timer == 0)
//								StartCoroutine ("timesUp");
			
						
					
						 
						//---------------
			
			
						
				}
		
				
		
				//				Debug.Log ("removeTimer");
		
		}

		void decreaseEnergy ()
		{
				if (gauge.transform.localScale.y > 0.3f) {
						gauge.transform.localScale -= new Vector3 (0, superTimer / 3000f, 0);

				}  
		}














		/// <summary>
		/////////////////////////////////////////////// Supers the mode count.///////////////////////////////////////////////////////////////////////////
		/// </summary>
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 						SUPER
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		void balloonStop ()
		{
				if (existBalloon && superLevel < 5) {
						
						previousBalloon = zonePosition;
						currentBalloon = balloon.transform.position;
						Vector2 tempVector = currentBalloon - previousBalloon;
						float zoneLimit = 2f;
						if (-1 * zoneLimit < tempVector.x && tempVector.x < zoneLimit && -1 * zoneLimit < tempVector.y && tempVector.y < zoneLimit) {
								balloon.SendMessage ("stopBalloon", true);
								oEnergy.animation.Play ();
								if (!oStopSound.audio.isPlaying)
										oStopSound.audio.Play ();
//								Instantiate (effectStop, balloon.transform.position, Quaternion.identity);
								stopRate = stopRateControl;
						} else {
								balloon.SendMessage ("stopBalloon", false);
								oEnergy.animation.Stop ();
								oEnergy.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
								oStopSound.audio.Stop ();
								stopRate = 0;
						}
				} else if (existBalloon && superLevel == 5) {
						balloon.SendMessage ("stopBalloon", false);
						oEnergy.animation.Stop ();
						oStopSound.audio.Stop ();
						stopRate = 0;
				}  
		}

		void superModeCount ()
		{
				
				if (gauge.transform.localScale.y > 1.75f) {
						CancelInvoke ("superModeCount");
						gauge.transform.localScale = new Vector3 (1.75f, 1.75f, 1);
			
						if (superLevel < 5) {
								gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
								superLevel++;
//								Debug.Log ("supermode(" + superLevel);
								superMode (superLevel);
						}
				} else {
						gauge.transform.localScale += new Vector3 (0, superTimer / 5000f * stopRate, 0);
				}
		}
	
		void normalModeCount ()
		{
				superTimer--;
		
				if (superTimer < 0) {
							
							
						Debug.Log ("normalCancel");
						CancelInvoke ("normalModeCount");
						superLevel++;
						if (superLevel < 4)
								superMode (superLevel);
							
		
		
		
				}
		
		
		}
	
		void superMode (int num)
		{
				zonePosition = new Vector2 (100, 100);
				balloon.transform.localRotation = new Quaternion (0, 0, 0, 0);
				CancelInvoke ("backCreate");
				CancelInvoke ("balloonStop");
				CancelInvoke ("normalModeCount");
				balloon.SendMessage ("stopBalloon", false);
				Debug.Log ("supermode" + num);
				stopRate = 0;
				if (num == 0) {
						superTimer = 3;
						superLevel = 0;
						InvokeRepeating ("normalModeCount", 0.1f, 1f);
				} else {
						zoneReset ();
						if (num == 1)
								InvokeRepeating ("zoneCreate", 0.1f, zoneCreateRate);
						else
								InvokeRepeating ("zoneCreate", 2f, zoneCreateRate);
						superTimer = superTime;
						InvokeRepeating ("superModeCount", 0.1f, 0.2f);
				}
		
				balloon.SendMessage ("superMode", num);
			 

//				enemy [0].SendMessage ("superMode", num);
//				enemy [1].SendMessage ("superMode", num);
//				enemy [2].SendMessage ("superMode", num);
				bgm.SendMessage ("superMode", num);
		
				//********************score
				CancelInvoke ("scoreCount");
				switch (num) {
			
				case 1:
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 8);
						break;
				case 2:
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 8);
						undeadTime = 4;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.2";
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						audio.PlayOneShot (levelUp);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						break;
				case 3:
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 8);
						undeadTime = 4;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.3";
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						break;
			
				 

				case 4:
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 12);
						undeadTime = 4;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.4";
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (numHave == 0)
								balloonSprite.sprite = superBalloon4;
						break;

				case 5:
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 20);
						GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
						if (tempZone != null)
								Destroy (tempZone);
						CancelInvoke ("zoneCreate");
						undeadTime = 4;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.5";
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (numHave == 0)
								balloonSprite.sprite = superBalloon5;
						break;
				case 6:
						InvokeRepeating ("scoreCount", 0.1f, 0.1f);
						InvokeRepeating ("backCreate", 0, levelRate [1] * 8);
						break;
				default:
						break;
			
			
				}
				InvokeRepeating ("balloonStop", 0.5f, 0.1f);
		}
	
		void scoreCount ()
		{
				if (superLevel < 6) {
						score += 1;
				} else {
						score -= 1;
				}

				scoreText.text = " :  " + score / 10 + " balloon";
				if (score < 0) {
						CancelInvoke ("scoreCount");
						score = 0;
						scoreText.text = " :  " + "0 balloon";
						StartCoroutine ("timesUp");
				}
				 
		}
	
		void enableTouch ()
		{
				GetComponent<FingerDownDetector> ().enabled = true;
				GetComponent<FingerUpDetector> ().enabled = true;
				GetComponent<DragRecognizer> ().enabled = true;
		}
	
		void disableTouch ()
		{
				GetComponent<FingerDownDetector> ().enabled = false;
				GetComponent<FingerUpDetector> ().enabled = false;
				GetComponent<DragRecognizer> ().enabled = false;
		}
	
		void getBalloonMSG (int num)
		{
				switch (num) {
			
				case 1:
						CancelInvoke ("balloonStop");
						StartCoroutine (Remove (2));
						break;
				case 2:
						score += 10;
						break;
				case 3:
						score += 50;
						break;
				case 4:
						numHave = 0;
						break;
				case 5:
						undeadTime = 1.5f;
						break;
				default:
						break;
			
			
				}
		}
		/************************ Control **********************/
	
	
	
	
		int dragFingerIndex = -1;
	
		void OnDrag (DragGesture gesture)
		{
				// first finger
				FingerGestures.Finger finger = gesture.Fingers [0];
		
		
				if (existBalloon) {
						if (gesture.Phase == ContinuousGesturePhase.Started) {
								// remember which finger is dragging balloon
								dragFingerIndex = finger.Index;
				
								// spawn some particles because it's cool.
								//				 SpawnParticles (balloon);
						} else if (finger.Index == dragFingerIndex) {  // gesture in progress, make sure that this event comes from the finger that is dragging our balloon
								if (gesture.Phase == ContinuousGesturePhase.Updated) {
										// update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
										Vector3 touchXY = GetWorldPos (gesture.Position);
										balloon.transform.position = touchXY;
								} else {
										// reset our drag finger index
										dragFingerIndex = -1;
								}
						}
				}
		}
	
		void OnFingerDown (FingerDownEvent e)
		{
		
				 
				if (e.Selection == btn_menu) {
						btn_menu.GetComponent<SpriteRenderer> ().color = Color.yellow;
				}
				if (e.Selection == btn_replay) {
						btn_replay.GetComponent<SpriteRenderer> ().color = Color.yellow;
//												Application.LoadLevel (0);
				}
				if (!existBalloon && onPlay && e.Selection != btn_pause) {
						existBalloon = true;
						Create (GetWorldPos (e.Position));
				}

				if (e.Selection == btn_pause && onPlay) {
						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
						//							 					Application.LoadLevel (0);
				}

				if (e.Selection == btn_resume) {
						btn_resume.GetComponent<SpriteRenderer> ().color = Color.yellow;

						//												Application.LoadLevel (0);
				}
				

				//				Debug.Log ("click");
		}
	 
		void OnFingerUp (FingerUpEvent e)
		{
				if (e.Selection == btn_menu) {
						btn_menu.GetComponent<SpriteRenderer> ().color = Color.white;
						Time.timeScale = 1.0f;
//						gameReset ();
						Application.LoadLevel (1);
				}
				if (e.Selection == btn_resume) {
						btn_resume.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						onPlay = true;
						Time.timeScale = 1.0f;
				}
				if (e.Selection == btn_replay) {
						btn_replay.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_timesup(Clone)"));
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
						gameReset ();
						//												Application.LoadLevel (0);
				}
				if (e.Selection == btn_pause && onPlay) {
						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
						StartCoroutine (pauseGame ());

						//												Application.LoadLevel (0);
				}
				if (existBalloon && onPlay) {
						CancelInvoke ("balloonStop");
						StartCoroutine (Remove (1));			
			
				}
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
