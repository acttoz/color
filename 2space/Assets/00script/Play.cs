using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour
{

		int questEnemy = 0;
		int questTarget = 0;
		public GameObject oEnemyNum;
		public bool testItem = false;
		public float enemyCreateRate;
		public float itemCreateRate;
		public float zoneCreateRate;
		private int tempLevel;
		public int scoreRate;
		public float stopRateControl;
		public int superTime;
		public int levelLimit;
		public float[] enemyRates;
		public int[] scoreRates;
		int numGem;
		int enemyNum = 0;
//		int failTime = 3;
		bool onCount = false;
//		bool seenTutorial1 = false;
		Component admob;
		public Sprite[] sItems;
		public float[] levelRate = new float[4];
		float stopRate = 0;
		int item1, item2, item3, item4;
		public GameObject itemShield;
		bool isShield = false;
		public int spaceId = 0;
		public Color colSky;
		public GameObject[] monsterIcons;
		public Sprite[] ufos;
		public GameObject[] spaces = new GameObject[13];
		public GameObject[] stars = new GameObject[6];
		public float[] spacesHeight = new float[12];
		public bool test;
		int killNum = 0;
		public bool isMonster = false;
		public float undeadTime = 0;
		public GameObject mPoint, oBGM, oTutorial1, enemyPop, prf_FailTimer, oFailTimer, warn_boss, prf_boss, lightSpeed, oBoss, backFirst, testBack, prf_enemy, backElement, backStars, oStars, mainCamera;
		GameObject[] back;
		public bool isUndead = false;
//		float deadLine;
//		bool isUndead = false;
		private Vector2 zonePosition;
//		public GameObject[] oAirs = new GameObject[3];
		public GameObject oZone, oEnergy, oStopSound, btnRestart, btnNext, balloon, itemBlue, itemOrange, itemPurple, monsterEffect, super_back2;
		public Sprite bStar, oStar, pStar, eStar, superBalloon4, superBalloon5;
		public GameObject itemEffectO, itemEffectB, effectPoint2, effectStop, itemEffectP, itemEffectBack, btn_pause, prf_pause, btn_resume;
		GameObject existItem, createItem, btn_menu, btn_replay;
//		int timer;
		Vector2 previousBalloon, currentBalloon;
//		int gameTime = 3;
		public int leftTime;
		bool onPlay;
		int min, sec, countScore = 0, countGem1 = 0;
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
		float score = 0;
		int gem = 0;
		public static int superLevel = 0;
 
		void gameStart ()
		{
				audio.PlayOneShot (go);
				enableTouch ();
				//				timeStarted = true;
				onPlay = true;
		
				Instantiate (backFirst, new Vector2 (0, 0), Quaternion.identity);
		}
 
		//RESET
		void gameReset ()
		{
				balloon.transform.position = new Vector2 (0, 0);
				
		
				isCounting = false;
				int q = 0;
				bgm.SendMessage ("superMode", q);
				superLevel = 1;
				if (item1 == 1)
						superLevel = 2;
				tempLevel = superLevel;
				StopCoroutine ("enemyCreate");
				CancelInvoke ("backCreate");
				GameObject[] temp2;
				temp2 = GameObject.FindGameObjectsWithTag ("bombparent");
				for (int i=0; i<temp2.Length; i++) {
						Destroy (temp2 [i]);
				}

				//boss reset
				GameObject[] tempBoss;
				tempBoss = GameObject.FindGameObjectsWithTag ("boss");
				for (int i=0; i<tempBoss.Length; i++) {
						Destroy (tempBoss [i]);
				}

				if (Level.instance.getLevel () != 6 && Level.instance.getLevel () != 7)
						oBoss = Instantiate (prf_boss, new Vector2 (0, 0), Quaternion.identity) as GameObject;

				mainCamera.camera.clearFlags = CameraClearFlags.SolidColor;
		
				killNum = 0;
		
				enemyNum = 0;
				item1 = PlayerPrefs.GetInt ("ITEM1", 0);
				item2 = PlayerPrefs.GetInt ("ITEM2", 0);
				item3 = PlayerPrefs.GetInt ("ITEM3", 0);
				item4 = PlayerPrefs.GetInt ("ITEM4", 0);
				superLevel = 0;
	 
				if (item2 == 1)
						isShield = true;
//				if (oBoss != null)
//						oBoss.transform.localScale = new Vector2 (1.2f, 1.2f);
				//				oBoss.GetComponent<SphereCollider> ().radius = 0.34f;
//		MGR_Item item = new MGR_Item ();
				Item.instance.resetStar (0);
				isMonster = false;
				spaceId = 0;
				mainCamera.camera.backgroundColor = colSky;
				GameObject[] temp;
				temp = GameObject.FindGameObjectsWithTag ("back");
				for (int i=0; i<temp.Length; i++) {
						Destroy (temp [i]);
				}
				if (GameObject.Find ("prf_warn(Clone)") != null)
						Destroy (GameObject.Find ("prf_warn(Clone)"));
				if (GameObject.FindGameObjectWithTag ("bomb_b") != null)
						Destroy (GameObject.FindGameObjectWithTag ("bomb_b"));
				if (GameObject.FindGameObjectWithTag ("bomb_o") != null)
						Destroy (GameObject.FindGameObjectWithTag ("bomb_o"));
				if (GameObject.FindGameObjectWithTag ("bomb_p") != null)
						Destroy (GameObject.FindGameObjectWithTag ("bomb_p"));
				balloon.GetComponent<SpriteRenderer> ().sortingOrder = -1;
				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0);
				if (existItem != null)
						Destroy (existItem);
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						Destroy (tempZone);
				CancelInvoke ("zoneCreate");
				CancelInvoke ("enemyCreate");
//				oBoss.transform.tag = "boss";
				enemy = GameObject.FindGameObjectsWithTag ("enemy");
				if (Value.isQuest)
						oEnemyNum.GetComponent<tk2dTextMesh> ().text = 0 + "";
				questEnemy = 0;
				for (int i=0; i<enemy.Length; i++) {
						Destroy (enemy [i]);
				}
				if (Level.instance.getLevel () == 11) {
						for (int i=0; i<(questTarget+2); i++)
								Enemy.instance.InitEnemy ();

				}
				if (Level.instance.getLevel () == 14) {
						for (int i=0; i<questTarget; i++)
								Enemy.instance.InitEnemy ();
			
				}
				if (Level.instance.getLevel () == 15) {
						for (int i=0; i<questTarget; i++)
								Enemy.instance.InitBoss ();
				}
				if (Level.instance.getLevel () == 16) {
						questEnemy = 0;
						for (int i=0; i<10; i++) {
								Enemy.instance.InitEnemy ();
								questEnemy++;
								oEnemyNum.GetComponent<tk2dTextMesh> ().text = questEnemy + "";
						}
				}
				if (Value.questNum == 4 || Value.questNum == 5) {
						questEnemy = 20;
						oEnemyNum.GetComponent<tk2dTextMesh> ().text = questEnemy + "";
			
				}
//				oBoss.transform.tag = "enemy";
//				oBoss.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				undeadTime = 0;
				CancelInvoke ("itemCreate");
				CancelInvoke ("zoneCreate");
				CancelInvoke ("scoreCount");
				CancelInvoke ("superModeCount");
				CancelInvoke ("normalModeCount");
				gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
				onPlay = false;
				score = 0;
				superLevel = 1;
				if (item1 == 1)
						superLevel = 2;
				scoreText.text = ": " + score;
				//				timeStarted = false;
				disableTouch ();
				//				MANAGER.Item. resetStar ();
				//				timer = gameTime;
				///level reset
				lvText.text = "Lv.1";
				superTimer = superTime;
		
				bgm.SendMessage ("superMode", 1);
				MANAGER.STATE = "IDLE";
				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
//		gameStart ();
//				if (Level.instance.getLevel()> 1)
						
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

//		void ruReady ()
//		{
//				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
//		}

		
	
		IEnumerator pauseGame ()
		{
				Instantiate (prf_pause, new Vector2 (0, 0), Quaternion.identity);
		
				btn_menu = GameObject.Find ("btn_menu");
				btn_replay = GameObject.Find ("btn_replay");
				btn_resume = GameObject.Find ("btn_resume");
				disableTouch ();
				yield return new WaitForSeconds (0.5f);
				enableTouch ();
				Time.timeScale = 0;
		}
	
		IEnumerator timesUp ()
		{

				onPlay = false;
				superLevel = 0;
				balloon.transform.localScale = new Vector3 (0, 0, 0);
				balloon.SetActive (false);
				disableTouch ();
				audio.PlayOneShot (timesup);
				GameObject otimesup = Instantiate (oTimeUp, new Vector2 (0, 0), Quaternion.identity) as GameObject;
				otimesup.transform.parent = GameObject.Find ("UI").transform;
				otimesup.transform.localPosition = new Vector2 (0, 0);
				resultText = GameObject.Find ("numscore").GetComponent<tk2dTextMesh> ();
				gemText1 = GameObject.Find ("numgem").GetComponent<tk2dTextMesh> ();
//				gemText2 = GameObject.Find ("numgem2").GetComponent<tk2dTextMesh> ();
//				gemText3 = GameObject.Find ("numgem3").GetComponent<tk2dTextMesh> ();
				gemText1.text = "" + countGem1;
//				gemText2.text = "" + countGem2;
//				gemText3.text = "" + countGem3;
				yield return new WaitForSeconds (1f);
				countScore = 0;
				btn_menu = GameObject.Find ("btn_menu");
				btn_replay = GameObject.Find ("btn_replay");
				btnNext = GameObject.Find ("btn_next");
				InvokeRepeating ("resultCount", 0f, 0.01f);
				numGem = PlayerPrefs.GetInt ("NUMGEM", 0);
		}
		//RESULT
		void resultCount ()
		{
				if (score < countScore) {
						audio.PlayOneShot (itemSound);
			
						CancelInvoke ("resultCount");
			
						resultText.text = "" + score;
//						if (score >= 100000) {
//								gem = (int)score / 100000;
//								InvokeRepeating ("resultGemCount3", 0.5f, 0.3f);
//						} else if (score >= 10000) {
//								gem = (int)score / 10000;
//								InvokeRepeating (	"resultGemCount2", 0.5f, 0.3f);
//						} else 
			
						if (score >= 1000) {
								gem = (int)score / 1000;
								if (gem < 50) {
										InvokeRepeating ("resultGemCount1", 0.5f, 0.1f);
								} else {
										InvokeRepeating ("resultGemCount1", 0.5f, 0.01f);
								}
						} else {
								enableTouch ();
						}
				} else {
						audio.PlayOneShot (bing);
						resultText.text = "" + countScore;
						countScore += 1000;
				}
		
		
		}
	
//		void resultGemCount3 ()
//		{
//				audio.PlayOneShot (itemSound);
//				countGem3++;
//				gem--;
//				gemText3.text = ": " + countGem3;
//				resultText.text = "" + (score -= 100000) / 10;
//				numGem = numGem + 100;
//				PlayerPrefs.SetInt ("NUMGEM", numGem);
//				if (gem < 1) {
//			
//						CancelInvoke ("resultGemCount3");
//						PlayerPrefs.SetInt ("NUMGEM3", countGem3);
//
//						if (score >= 10000) {
//								gem = (int)score / 10000;
//								InvokeRepeating ("resultGemCount2", 0.5f, 0.3f);
//						} else if (score >= 1000) {
//								gem = (int)score / 1000;
//								InvokeRepeating ("resultGemCount1", 0.5f, 0.3f);
//						} else {
//								enableTouch ();
//						}
//				}  
//		
//		
//		}

//		void resultGemCount2 ()
//		{
//				audio.PlayOneShot (itemSound);
//				countGem2++;
//				if (countGem2 > 9) {
//						countGem3++;
//						gemText3.text = ": " + countGem3;
//						PlayerPrefs.SetInt ("NUMGEM3", countGem3);
//						countGem2 = 0;
//				}
//				gem--;
//				gemText2.text = ": " + countGem2;
//				resultText.text = "" + (score -= 10000) / 10;
//				numGem = numGem + 10;
//				PlayerPrefs.SetInt ("NUMGEM", numGem);
//				if (gem < 1) {
//			
//						CancelInvoke ("resultGemCount2");
//						PlayerPrefs.SetInt ("NUMGEM2", countGem2);
//						if (score >= 1000) {
//								gem = (int)score / 1000;
//								InvokeRepeating ("resultGemCount1", 0.5f, 0.3f);
//						} else {
//								enableTouch ();
//						}
//				}  
//		
//		
//		}

		void resultGemCount1 ()
		{
				audio.PlayOneShot (bing);
//				countGem1++;
//		if(countGem1>9){
//			countGem2++;
//			gemText2.text = ": " + countGem2;
//			PlayerPrefs.SetInt ("NUMGEM2", countGem2);
//			countGem1=0;
//		}
				gem--;
				numGem = numGem + 1;
				gemText1.text = "" + numGem;
				resultText.text = "" + (score -= 1000);
				PlayerPrefs.SetInt ("NUMGEM", numGem);
				if (gem < 1) {
			
						CancelInvoke ("resultGemCount1");
//						PlayerPrefs.SetInt ("NUMGEM1", countGem1);
						
						audio.PlayOneShot (itemSound);
						enableTouch ();
						
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
		
		//BACK
		void backCreate ()
		{
				if (superLevel > 0 && superLevel < 20) {
						float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100f;
						if (spaceId < 2) {
								Instantiate (backElement, new Vector3 (tempX, (MANAGER.mUp + 3), 0), Quaternion.identity);
						} else {
//				Debug.Log(superLevel);
								if (superLevel > 5) {
										Instantiate (lightSpeed, new Vector3 (tempX, 8, 0), Quaternion.identity);
								} else {
										Instantiate (stars [Random.Range (0, 6)], new Vector3 (tempX, (MANAGER.mUp + 3), 0), Quaternion.identity);
								}
						}
				} else if (superLevel == 20 && score > 30) {
					
						float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100;
						if (spaceId < 2) {
								Instantiate (backElement, new Vector3 (tempX, -8, 0), Quaternion.identity);
						} else {
								Instantiate (stars [Random.Range (0, 6)], new Vector3 (tempX, -8, 0), Quaternion.identity);
						}
				} 
		}

		void zoneCreate ()
		{
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						tempZone.animation.Play ("anim_zoneOut");
				float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100;
				float tempY = (Random.Range (MANAGER.mDown * 100, MANAGER.mUp * 100)) / 100;
				if (Level.instance.getLevel () > 1) {
						zonePosition = new Vector2 (tempX, tempY);

						GameObject obj = Instantiate (oZone, new Vector3 (tempX, tempY, 0), Quaternion.identity) as GameObject;
						if (item4 == 1)
								obj.transform.localScale = new Vector2 (1.5f, 1.5f);
				}
		}

		void zoneReset ()
		{
				CancelInvoke ("zoneCreate");
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						Destroy (tempZone);
		}

		
		

//		void monsterOrange ()
//		{
//				isMonster = false;
//				if (superLevel < levelLimit) {
//						superLevel++;
//						superMode (superLevel);
//				}
//		}

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

		












		/// <summary>
		///////////////////////////////////////////////// Create & Remove	/// </summary>////////////////////////////////////////////////////////////////////
		/// <param name="touch">Touch.</param>
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 				CREATE
		/// 
		/// 
		/// 
		/// 
		/// 

		void Create (Vector3 touch)
		{
				if (Value.isQuest && (Value.questNum == 4 || Value.questNum == 5)) {
			
			
						InvokeRepeating ("seconds", 0f, 1f);
				}		
//				timer--;
//				oAirs [timer].animation.Play ();
//				superLevel = 1;
//				if (item1 == 1)
//						superLevel = 2;
//		superLevel = 5;
		
				//TEST
//				superLevel = 5;
//				superLevel = 3;
				superLevel = tempLevel;
				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 1);
				balloon.SetActive (true);
				isMonster = false;
				balloon.SendMessage ("offMonster");
		
				balloon.transform.localRotation = new Quaternion (0, 0, 0, 0);
				balloon.GetComponentInChildren<SpriteRenderer> ().color = Color.red;
				balloon.SendMessage ("undead", false);
				InvokeRepeating ("balloonStop", 0.1f, 0.1f);
				CancelInvoke ("decreaseEnergy");
				balloon.transform.position = touch;
				superMode (superLevel);
						
				balloon.SendMessage ("create", superLevel);
//				Debug.Log ("create Level" + superLevel);
				audio.PlayOneShot (create);
		
		}
		//REMOVE
		IEnumerator Remove (int num)
		{
				oStopSound.audio.Stop ();
				oEnergy.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
				CancelInvoke ("scoreCount");
				CancelInvoke ("superModeCount");
				CancelInvoke ("normalModeCount");
				stopRate = 0;
				oEnergy.animation.Stop ();
				zoneReset ();
				
				switch (num) {
				case 1:
//						superLevel = 0;
			
//						deadLine = score - 20;
						 
//						superMode (superLevel);
//						balloon.SendMessage ("cancel", 1);
						balloon.SendMessage ("startCoundDown");
						onCount = true;
//						isCounting = true;
						onPlay = false;

			//						if (audio.isPlaying)
						audio.Stop ();
						audio.PlayOneShot (remove);
			//decrease energy
//						if (superLevel < levelLimit)
//								InvokeRepeating ("decreaseEnergy", 0.2f, 0.2f);
//						
//						oFailTimer= Instantiate (prf_FailTimer, new Vector2 (0, 0), Quaternion.identity) as GameObject;
						break;
			
				case 2:
						balloon.SendMessage ("cancel", 2);
						undeadTime = 0;
						audio.Stop ();
						audio.PlayOneShot (pop);
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
					 
						balloon.transform.localScale = new Vector3 (0, 0, 0);
						balloon.SetActive (false);
						StartCoroutine ("timesUp");
						break;
				}
		
		
				yield return new WaitForSeconds (0.2f);
//						balloon.transform.localScale = new Vector3 (0, 0, 0);
			
						
			
			
				//						if (num == 1)
//						balloon.SetActive (false);
//						if (timer == 0)
//								StartCoroutine ("timesUp");
			
						
					
						 
				//---------------
			
			
						
		}
		
				
		
		//				Debug.Log ("removeTimer");
		


		void timeOut ()
		{
				onPlay = false;
				StartCoroutine ("timesUp");

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
		/// STOP
		void balloonStop ()
		{
				if (MANAGER.STATE.Equals("PLAY") && superLevel < levelLimit) {
						
						previousBalloon = zonePosition;
						currentBalloon = balloon.transform.position;
						Vector2 tempVector = currentBalloon - previousBalloon;
						float zoneLimit = 1.8f;

						if (item4 == 1)
								zoneLimit = 3.5f;
						if (-1 * zoneLimit < tempVector.x && tempVector.x < zoneLimit && -1 * zoneLimit < tempVector.y && tempVector.y < zoneLimit) {
//								balloon.SendMessage ("stopBalloon", true);
								oEnergy.animation.Play ();
								if (!oStopSound.audio.isPlaying)
										oStopSound.audio.Play ();
//								Instantiate (effectStop, balloon.transform.position, Quaternion.identity);
								stopRate = stopRateControl;
						} else {
//								balloon.SendMessage ("stopBalloon", false);
								oEnergy.animation.Stop ();
								oEnergy.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
								oStopSound.audio.Stop ();
								stopRate = 0;
						}
		} else if (MANAGER.STATE.Equals("PLAY") && superLevel == levelLimit) {
//						balloon.SendMessage ("stopBalloon", false);
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
			
						if (superLevel < levelLimit) {
								gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
								superLevel++;
//								Debug.Log ("supermode(" + superLevel);
								superMode (superLevel);
						}
				} else {
			
						gauge.transform.localScale += new Vector3 (0, superTimer / 5000f * stopRate, 0);
						if (stopRate != 0)
								Instantiate (itemEffectBack, balloon.transform.position, Quaternion.identity);

				}
		}
	
		void normalModeCount ()
		{
				superTimer--;
		
				if (superTimer < 0) {
							
							
						CancelInvoke ("normalModeCount");
						superLevel++;
						if (superLevel < 4)
								superMode (superLevel);
							
		
		
		
				}
		
		
		}
	
		void superMode (int num)
		{
				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				if (!onPlay)
						return;

				if (num == levelLimit) {
						CancelInvoke ("zoneCreate");
						GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
						if (tempZone != null)
								Destroy (tempZone);
				}
				zonePosition = new Vector2 (100, 100);
				CancelInvoke ("backCreate");
				CancelInvoke ("balloonStop");
				CancelInvoke ("normalModeCount");
//				balloon.SendMessage ("stopBalloon", false);
				Debug.Log ("supermode" + num);
				stopRate = 0;
//				stopRate = stopRateControl;
				scoreRate = 1;
				if (num == 0) {
						superTimer = 3;
						superLevel = 1;
//						InvokeRepeating ("normalModeCount", 0.1f, 1f);
				} else if (Level.instance.getLevel () != 6 && Level.instance.getLevel () != 7 && Level.instance.getLevel () != 8) {
						zoneReset ();
						if (num == 1)
								InvokeRepeating ("zoneCreate", 0.1f, zoneCreateRate);
						else if (num != levelLimit)
								InvokeRepeating ("zoneCreate", 2f, zoneCreateRate);
						superTimer = superTime;
						InvokeRepeating ("superModeCount", 0.1f, 0.2f);
				}
		
				balloon.SendMessage ("superMode", num);
			 

//				enemy [0].SendMessage ("superMode", num);
//				enemy [1].SendMessage ("superMode", num);
//				enemy [2].SendMessage ("superMode", num);
				bgm.SendMessage ("superMode", num);
				if (num > 1 && num < 11 && !isCounting) {

						//levelUp Effect
						isUndead = true;
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						GameObject objUI = GameObject.Find ("UI");
						GameObject obj1 = Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity) as GameObject;
						obj1.transform.parent = objUI.transform;
						obj1.transform.localPosition = new Vector2 (0, 0);
						audio.PlayOneShot (levelUp);
						GameObject obj2 = Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity) as GameObject;
						obj2.transform.parent = objUI.transform;
						obj2.transform.localPosition = new Vector2 (0, 0);
						if (num > 5) {
								enemyBomb ();
								turnLightSpeed (120);
						}
				}
				isCounting = false;

				//********************score
				CancelInvoke ("scoreCount");
				switch (num) {
				case 0:
						InvokeRepeating ("scoreCount", 0.1f, levelRate [0]);
						InvokeRepeating ("backCreate", 0, levelRate [0] * 4);
						
						break;
				case 1:
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 4);
						break;
				case 2:
//						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 4);
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv.2";
		
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
			//Level.instance.getLevel2
						if (Level.instance.getLevel () == 2 && onPlay) {
								PlayerPrefs.SetInt ("2", 1);
								timeOut ();
						}
						
						break;
				case 3:
//						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 4);
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv.3";
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
			//Level.instance.getLevel3
						if (Level.instance.getLevel () == 3 && onPlay) {
				 
								PlayerPrefs.SetInt ("3", 1);
								timeOut ();
								
						}
						break;
			
				 

				case 4:
//						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 6);
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv.4";
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						if (!isMonster)
								balloonSprite.sprite = superBalloon4;
			//Level.instance.getLevel4
						if (Level.instance.getLevel () == 4 && onPlay) {
								PlayerPrefs.SetInt ("4", 1);
								timeOut ();
						}
						break;

				case 5:
//						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 10);
						
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv.5";
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						if (!isMonster)
								balloonSprite.sprite = superBalloon5;
			//Level.instance.getLevel5
						if (Level.instance.getLevel () == 5 && onPlay) {
								PlayerPrefs.SetInt ("5", 1);
								timeOut ();
						}
						break;
				case 6:
						InvokeRepeating ("backCreate", 0, levelRate [4] * 5);
						scoreRate = scoreRates [num - 6];
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv.6";


//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						if (!isMonster)
								balloonSprite.sprite = ufos [0];
			//Level.instance.getLevel5
						 
						break;
				case 7:
						InvokeRepeating ("backCreate", 0, levelRate [4] * 5);
						scoreRate = scoreRates [num - 6];
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv." + num;
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//Level.instance.getLevel5
			
						break;
				case 8:
						InvokeRepeating ("backCreate", 0, levelRate [4] * 7);
						scoreRate = scoreRates [num - 6];
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv." + num;
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//Level.instance.getLevel5
			
						break;
				case 9:
						InvokeRepeating ("backCreate", 0, levelRate [4] * 10);
						scoreRate = scoreRates [num - 6];
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv." + num;
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//Level.instance.getLevel5
			
						break;
				case 10:
						InvokeRepeating ("backCreate", 0, levelRate [4] * 10);
						scoreRate = scoreRates [num - 6];
						balloon.SendMessage ("undead", true);
						lvText.text = "Lv." + num;
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//Level.instance.getLevel5
			
						break;
		 
				case 20:
						InvokeRepeating ("scoreCount", 0.1f, 0.1f);
						InvokeRepeating ("backCreate", 0, levelRate [1] * 4);
						break;
				default:
						break;
			
			
				}
	
				InvokeRepeating ("balloonStop", 0.5f, 0.1f);
		}

		void enemyBomb ()
		{
				enemy = GameObject.FindGameObjectsWithTag ("enemy");
				for (int i=0; i<enemy.Length; i++) {
						Instantiate (effectPointBack, enemy [i].transform.position, Quaternion.identity);
						Instantiate (mPoint, enemy [i].transform.position, Quaternion.identity);
						Destroy (enemy [i]);
				}
		}

		void turnLightSpeed (int num)
		{
				int i = 0;
				while (i<num) {
						Instantiate (lightSpeed, new Vector2 (((Random.Range (-40, 41)) / 10f), ((Random.Range (-60, 61)) / 10f)), Quaternion.identity);
						i++;
				}
		}

		//SCORE
		void scoreCount ()
		{
				if (superLevel != 20) {
						if (onPlay)
								score += scoreRate;
				} else {
						if (onPlay)
								score -= 1;
				}

				scoreText.text = " :  " + score;
				if (score < 0) {
						CancelInvoke ("scoreCount");
						score = 0;
						scoreText.text = " :  " + "0";
						superLevel = 0;
//						StartCoroutine ("timesUp");
				}
				//Level.instance.getLevel1
				if (Level.instance.getLevel () == 1 && onPlay && score > 20) {
						PlayerPrefs.SetInt ("1", 1);
						timeOut ();
				}
				if (Level.instance.getLevel () == 9 && onPlay && score > 1000) {
						PlayerPrefs.SetInt ("9", 1);
						timeOut ();
					
				}
//				if (score > spacesHeight [spaceId] && spaceId == 0) {
//						mainCamera.animation.Play ("anim_maincamera");
//						spaceId = 1;
//				}
				if (score > spacesHeight [spaceId] && spaceId == 0) {
						
						spaceId = 2;
						mainCamera.animation.Play ();
						enemyCreateRate = enemyRates [spaceId];
				}

				if (score > spacesHeight [spaceId] && spaceId == 2) {
						//setellite
						spaceId = 3;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
						
				}

				if (score > spacesHeight [spaceId] && spaceId == 3) {
						//moon
						spaceId = 4;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						

						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 4) {
						//spaceman
						spaceId = 5;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
						//               BOSS

						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 5) {
						//sun
						spaceId = 6;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 6) {
						spaceId = 7;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
			
						enemyCreateRate = enemyRates [spaceId];
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 7) {
						spaceId = 8;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);

				}
				if (score > spacesHeight [spaceId] && spaceId == 8) {
						spaceId = 9;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 9) {
						spaceId = 10;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
			
						enemyCreateRate = enemyRates [spaceId];
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 10) {
						spaceId = 11;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
				}
				if (score > spacesHeight [spaceId] && spaceId == 11) {
						spaceId = 12;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						//               BOSS
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
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
						if (isShield) {

								audio.PlayOneShot (levelUp);
								GameObject ep = (GameObject)GameObject.Instantiate (effectPop);
								ep.transform.position = balloon.transform.position;
								isUndead = true;
								balloon.SendMessage ("undead", true);
								undeadTime = 2f;
								balloon.SendMessage ("shield");
								isShield = false;
								Instantiate (itemShield, new Vector2 (0, 0), Quaternion.identity);
//								Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
				
						} else {
								CancelInvoke ("balloonStop");
								GameObject[] temp = GameObject.FindGameObjectsWithTag ("monster");
								for (int i=0; i<temp.Length; i++) {
										Destroy (temp [i]);
								}
								StartCoroutine (Remove (2));
						}
						break;
				case 2:
						score += 10;
						break;
				case 3:
						score += 50;
						break;
				case 4:
						Item.instance.resetStar (0);
						break;
				case 5:
						isUndead = true;
						if (onPlay)
								balloon.SendMessage ("undead", true);
						undeadTime = 1.5f;
						isMonster = false;
						break;
				case 6:
			//Level.instance.getLevel6,7
						killNum++;
						if (Level.instance.getLevel () == 6 && onPlay && killNum > 4) {
								onPlay = false;
								PlayerPrefs.SetInt ("6", 1);
								StartCoroutine ("timesUp");
						}
						if (Level.instance.getLevel () == 7 && onPlay && killNum > 4) {
								onPlay = false;
								PlayerPrefs.SetInt ("7", 1);
								StartCoroutine ("timesUp");
						}
						break;
				case 7:
			//Level.instance.getLevel8
						if (oBoss != null) 
								Destroy (oBoss);
						if (Level.instance.getLevel () == 8 && onPlay) {
								onPlay = false;
								PlayerPrefs.SetInt ("8", 1);
								StartCoroutine ("timesUp");
						}
						break;
				case 8:
						isMonster = false;
						break;
				case 9:
			//stop Timer
						if (oFailTimer != null)
								Destroy (oFailTimer);
						break;
				case 10:
			//stop Timer
						StartCoroutine ("timesUp");
	
						break;
				case 11:
			//point 100
						score += 100;
						scoreText.text = " :  " + score;
						if (Level.instance.getLevel () == 11) {
								questEnemy++;
								oEnemyNum.GetComponent<tk2dTextMesh> ().text = "" + questEnemy;
				
								if (questEnemy >= questTarget && onPlay) {
										Value.questLevel++;
										timeOut ();
								}
						}
						break;
				case 12:
			//point 500
						score += 500;
						scoreText.text = " :  " + score;
						if (Value.isQuest) {
								questEnemy++;
								oEnemyNum.GetComponent<tk2dTextMesh> ().text = questEnemy + "";
						}
						if (Value.isQuest && Value.questNum == 3 && Value.quests [2] [Value.questLevel] == questEnemy) {
								Value.questLevel++;
								timeOut ();
						}
						if (Value.isQuest && Value.questNum == 3 && Value.quests [2] [Value.questLevel] != questEnemy) {
								Instantiate (warn_boss, new Vector2 (0, 0), Quaternion.identity);
						}
			
			
						break;
				default:
						break;
			
			
				}
		}

		void Start ()
		{
				if (GameObject.FindGameObjectWithTag ("BGM") == null)
						Instantiate (oBGM, new Vector2 (0, 0), Quaternion.identity);
				bgm = GameObject.FindGameObjectWithTag ("BGM");
				admob = GetComponent<GoogleMobileAdsScript> ();
				admob.SendMessage ("RequestInterstitial");
				//		score = 200;
				//				PlayerPrefs.SetInt ("Level.instance.getLevel", 9);
				////				PlayerPrefs.SetInt ("Level.instance.getLevel", 2);
				item1 = PlayerPrefs.GetInt ("ITEM1", 0);
				item2 = PlayerPrefs.GetInt ("ITEM2", 0);
				item3 = PlayerPrefs.GetInt ("ITEM3", 0);
				item4 = PlayerPrefs.GetInt ("ITEM4", 0);
				levelLimit = PlayerPrefs.GetInt ("LIMIT", 5);
				////				PlayerPrefs.SetInt ("ITEM1", 1);
				////				PlayerPrefs.SetInt ("ITEM2", 1);
				////				PlayerPrefs.SetInt ("ITEM3", 1);
				////				PlayerPrefs.SetInt ("ITEM4", 1);
				////				item1 = 1;
				////				item2 = 1;
				////				item3 = 1;
				////				item4 = 1;
				if (item1 == 1)
						superLevel = 2;
				if (item2 == 1)
						isShield = true;
				if (item3 == 1)
						GameObject.Find ("Balloon").transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
				//				//				balloon.transform.localScale = new Vector3 (0, 0, 0);
				//
				//				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
				////				score = 1000;
				countGem1 = PlayerPrefs.GetInt ("NUMGEM1");
//				countGem2 = PlayerPrefs.GetInt ("NUMGEM2");
//				countGem3 = PlayerPrefs.GetInt ("NUMGEM3");
				//				if (test)
				//						Instantiate (testBack, new Vector2 (0, 0), Quaternion.identity);
				//						
				////				timer = gameTime;
				
				balloonSprite = balloon.GetComponentInChildren<SpriteRenderer> ();
				scoreText = GameObject.Find ("score").GetComponent<tk2dTextMesh> ();
				lvText = GameObject.Find ("lv").GetComponent<tk2dTextMesh> ();
				//				superTimer = superTime;
				
				////				Debug.Log ("screenSize=" + MANAGER.mUp + " " + MANAGER.mDown + " " + MANAGER.mLeft + " " + MANAGER.mRight + " ");
				currentBalloon = new Vector2 (20, 20);
				//		
				//				//				backSize = back.renderer.bounds.size; 
				//				//				Debug.Log (backSize);	
				//
				//				//test
				//				btn_menu = GameObject.Find ("btn_menu");
				//				btn_replay = GameObject.Find ("btn_replay");
				InvokeRepeating ("undeadTimer", 0, 0.25f);
				gameReset ();
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

		/************************ Control **********************/
		bool isCounting = false;
		int dragFingerIndex = -1;
	
		void OnDrag (DragGesture gesture)
		{
				// first finger
				FingerGestures.Finger finger = gesture.Fingers [0];
		
		
				if (MANAGER.STATE.Equals ("PLAY")) {
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
	
		void OnTap (TapGesture e)
		{
				Debug.Log (e.Selection.name);
				if (e.Selection.name == "btn_menu") {
//						btn_menu.GetComponent<SpriteRenderer> ().color = Color.yellow;
						Time.timeScale = 1.0f;
						//						gameReset ();
						bgm.SendMessage ("superMode", 1);
						Application.LoadLevel (1);
				}
				if (e.Selection.name == "btn_replay") {
//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.yellow;
						int adNum;
						adNum = PlayerPrefs.GetInt ("ADTIME", 0);
						adNum++;
						PlayerPrefs.SetInt ("ADTIME", adNum);
						if (adNum > 7 && adNum % 2 == 0)
								admob.SendMessage ("ShowInterstitial");
						
//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
						balloon.transform.localScale = new Vector3 (0, 0, 0);
						balloon.SetActive (false);
						gameReset ();
//												Application.LoadLevel (0);
				}
				if (e.Selection.name == "btn_next") {

				}
				

		
				if (e.Selection.name == "btn_pause" && onPlay) {
//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
						StartCoroutine (pauseGame ());
						//							 					Application.LoadLevel (0);
				}
//				if (e.Selection == btn_pause && onCount) {
//						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
//						StartCoroutine (pauseGame ());
//						//							 					Application.LoadLevel (0);
//				}

				if (e.Selection == btn_resume) {
//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.yellow;
//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						onPlay = true;
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

				if (!MANAGER.STATE.Equals ("PLAY") && onPlay && e.Selection != btn_pause) {
						MANAGER.STATE = "PLAY";
						Create (GetWorldPos (e.Position));
				}
				if (!MANAGER.STATE.Equals ("PLAY") && !onPlay && e.Selection == balloon && onCount) {
						onPlay = true;
						//			balloon.SendMessage("stopCount");
						onCount = false;
						MANAGER.STATE = "PLAY";
						
						Create (GetWorldPos (e.Position));
				}
		}
		//FINGERUP
		void OnFingerUp (FingerUpEvent e)
		{
				if (e.Selection == btn_menu) {
						
				}
				if (e.Selection == btnNext) {
						
				}
				if (e.Selection == btn_resume) {
						
				}
				if (e.Selection == btn_replay) {
						
						//												Application.LoadLevel (0);
				}
				if (e.Selection == btn_pause && onPlay) {
						

						//												Application.LoadLevel (0);
				}
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
