using UnityEngine;
using System.Collections;

public class scr_manager : MonoBehaviour
{
		bool onToast = false;
		public float enemyCreateRate;
		public float itemCreateRate;
		public float zoneCreateRate;
		public int scoreRate;
		public float stopRateControl;
		public int superTime;
		public int levelLimit;
		public float[] enemyRates;
		public int[] scoreRates;
		int LEVEL;
		int numGem;
		int enemyNum = 0;
//		int failTime = 3;
		bool onCount = false;
//		bool seenTutorial1 = false;
		Component admob;
		public Sprite[] sItems;
		public float[] levelRate = new float[4];
		float stopRate = 0;
		int selectedMonsterNum = 0;
		int item1, item2, item3, item4;
		public GameObject itemShield;
		bool isShield = false;
		public int spaceId = 0;
		public Color colSky;
		public GameObject[] monsterIcons;
		public Sprite[] ufos;
		bool selectedMonster1 = false;
		bool selectedMonster2 = false;
		bool selectedMonster3 = false;
		public GameObject[] spaces = new GameObject[13];
		public GameObject[] stars = new GameObject[6];
		public float[] spacesHeight = new float[12];
		public bool test;
		int killNum = 0;
		public bool isMonster = false;
		public float undeadTime = 0;
		public GameObject oBGM, oTutorial1, enemyPop, prf_FailTimer, oFailTimer, warn_boss, prf_boss, lightSpeed, oBoss, backFirst, testBack, prf_enemy, backElement, backStars, oStars, mainCamera;
		GameObject[] back;
		public bool isUndead = false;
//		float deadLine;
//		bool isUndead = false;
		private Vector2 zonePosition;
//		public GameObject[] oAirs = new GameObject[3];
		public GameObject oZone, oEnergy, oStopSound, btnRestart, btnNext, balloon, itemBlue, itemOrange, itemPurple, monsterB, monsterO, monsterP, monsterEffect, super_back2;
		public Sprite bStar, oStar, pStar, eStar, superBalloon4, superBalloon5;
		public GameObject itemEffectO, itemEffectB, effectPoint2, effectStop, itemEffectP, itemEffectBack, btn_pause, prf_pause, btn_resume;
//		int timer;
		Vector2 previousBalloon, currentBalloon;
//		int gameTime = 3;
		public int leftTime;
		bool onPlay;
		int min, sec, countScore = 0, countGem1 = 0, countGem2 = 0, countGem3 = 0;
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
		int superTimer;
		public AudioClip create, remove, pop, bing, levelUp, go, itemSound, timesup;
		tk2dTextMesh scoreText;
		tk2dTextMesh lvText;
		tk2dTextMesh timeText, resultText, gemText1;
		float score = 0;
		int gem = 0;
		public static int superLevel = 0;
		float mUp, mDown, mLeft, mRight;

		// Use this for initialization
		bool existBalloon = false;
//		bool timeStarted = false;
//		bool isScoreUp = false;
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
		///                     RESET
		/// 
		/// 
		/// 
		void gameStart ()
		{
				audio.PlayOneShot (go);
//		GameObject.Find ("ADS").SendMessage ("showAd");
				enableTouch ();
				//				timeStarted = true;
				onPlay = true;
				InvokeRepeating ("itemCreate", 1f, itemCreateRate);
				StartCoroutine ("enemyCreate");
//				InvokeRepeating ("enemyCreate", 1f, enemyCreateRate);
		
				Instantiate (backFirst, new Vector2 (0, 0), Quaternion.identity);
		}
		//ENEMY
		IEnumerator enemyCreate ()
		{
				while (true) {
						float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
						float tempY = (Random.Range (mDown * 100, mUp * 100)) / 100;
		
						enemyNum++;
						if (LEVEL == 5 && enemyNum > 2)
								StopCoroutine ("enemyCreate");
						if (LEVEL == 4 && enemyNum > 1)
								StopCoroutine ("enemyCreate");
						if (LEVEL == 3 && enemyNum > 1)
								StopCoroutine ("enemyCreate");
						if (LEVEL == 2 && enemyNum > 0)
								StopCoroutine ("enemyCreate");
						if (LEVEL == 1 && enemyNum > 0)
								StopCoroutine ("enemyCreate");
		
		
						Instantiate (prf_enemy, new Vector2 (tempX, tempY), Quaternion.identity);
						yield return new WaitForSeconds (enemyCreateRate / 1.2f);
			
				}
		}
		//RESET
		void gameReset ()
		{
				StopCoroutine ("enemyCreate");
				if (oBoss == null) {
						oBoss = Instantiate (prf_boss, new Vector2 (0, 0), Quaternion.identity) as GameObject;
				}
				oBoss.SendMessage ("resetLife");
				CancelInvoke ("backCreate");
				GameObject[] temp2;
				temp2 = GameObject.FindGameObjectsWithTag ("bombparent");
				for (int i=0; i<temp2.Length; i++) {
						Destroy (temp2 [i]);
				}
				mainCamera.camera.clearFlags = CameraClearFlags.SolidColor;
				LEVEL = PlayerPrefs.GetInt ("LEVEL", 1);
		
				killNum = 0;
		
				enemyNum = 0;
				item1 = PlayerPrefs.GetInt ("ITEM1", 0);
				item2 = PlayerPrefs.GetInt ("ITEM2", 0);
				item3 = PlayerPrefs.GetInt ("ITEM3", 0);
				item4 = PlayerPrefs.GetInt ("ITEM4", 0);
				superLevel = 0;
	 
				if (item2 == 1)
						isShield = true;
				if (oBoss != null)
						oBoss.transform.localScale = new Vector2 (1.2f, 1.2f);
				//				oBoss.GetComponent<SphereCollider> ().radius = 0.34f;
				resetStar (0);
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
				for (int i=0; i<enemy.Length; i++) {
						Destroy (enemy [i]);
				}
//				oBoss.transform.tag = "enemy";
				oBoss.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				undeadTime = 0;
				CancelInvoke ("itemCreate");
				CancelInvoke ("zoneCreate");
				CancelInvoke ("scoreCount");
				CancelInvoke ("superModeCount");
				CancelInvoke ("normalModeCount");
				gauge.transform.localScale = new Vector3 (1.75f, 0.3f, 1);
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
		
				bgm.SendMessage ("superMode", 1);
				existBalloon = false;
				Instantiate (backStart, new Vector2 (0, 0), Quaternion.identity);
//				if (LEVEL > 1)
						
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

		void monsterChoice ()
		{
				if (LEVEL > 8) {
						onToast = true;
						enableTouch ();
						Instantiate (monsterIcons [3], new Vector2 (0, 0), Quaternion.identity);
						monsterIcons [0] = GameObject.Find ("btn1");
						monsterIcons [1] = GameObject.Find ("btn2");
						monsterIcons [2] = GameObject.Find ("btn3");
						selectedMonsterNum = 0;
						selectedMonster1 = false;
						selectedMonster2 = false;
						selectedMonster3 = false;
						for (int i=0; i<3; i++) {
								monsterIcons [i].GetComponent<SpriteRenderer> ().color = new Color (0.5f, 0.5f, 0.5f, 1);
						}
				} else {
						if (PlayerPrefs.GetInt ("TUTO1", 0) == 0) {
								PlayerPrefs.SetInt ("TUTO1", 1);
								Instantiate (oTutorial1, new Vector2 (0, 0), Quaternion.identity);
						} else {
								gameStart ();
						}

				}
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
				superLevel = 0;
				existBalloon = false;
				disableTouch ();
				audio.PlayOneShot (timesup);
				Instantiate (oTimeUp, new Vector2 (0, 0), Quaternion.identity);
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
				balloon.SetActive (false);
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
		void itemCreate ()
		{
				if (oBoss == null)
						oBoss = GameObject.FindGameObjectWithTag ("boss");
		
				float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
				if (existItem != null)
						Destroy (existItem);
				int tempCol = Random.Range (1, 5);
				//test
//				tempCol = 3;

				
				switch (tempCol) {
				case 1:
						if (selectedMonster1) {
								colCreate = "b";
								createItem = itemBlue;
								tempStar = bStar;
						} else {
								createItem = itemOrange;
								colCreate = "o";
								tempStar = oStar;
						}
						break;

				case 2:
						if (selectedMonster2) {
								if (superLevel == levelLimit) {
										if (selectedMonster1) {
												createItem = itemBlue;
												colCreate = "b";
												tempStar = bStar;
										} else {
												createItem = itemPurple;
												colCreate = "p";
												tempStar = pStar;
										}
								} else {
										createItem = itemOrange;
										colCreate = "o";
										tempStar = oStar;
								}
						} else {
								createItem = itemBlue;
								colCreate = "b";
								tempStar = bStar;
						}
						break;

				case 3:
						if (selectedMonster3) {

								if (oBoss != null) {
										createItem = itemPurple;
										colCreate = "p";
										tempStar = pStar;
								} else {
										if (selectedMonster1) {
												createItem = itemBlue;
												colCreate = "b";
												tempStar = bStar;
										} else {
												createItem = itemOrange;
												colCreate = "o";
												tempStar = oStar;
										}
								}
						} else {
								createItem = itemBlue;
								colCreate = "b";
								tempStar = bStar;
						}
						break;
				case 4:
						if (selectedMonster3) {
								if (oBoss != null) {
										createItem = itemPurple;
										colCreate = "p";
										tempStar = pStar;
								} else {
										if (selectedMonster1) {
												createItem = itemBlue;
												colCreate = "b";
												tempStar = bStar;
										} else {
												createItem = itemOrange;
												colCreate = "o";
												tempStar = oStar;
										}
								}
						} else {
								createItem = itemOrange;
								colCreate = "o";
								tempStar = oStar;
						}
						break;
				 
		
				}
				
				switch (LEVEL) {
				case 6:
						createItem = itemOrange;
						colCreate = "o";
						tempStar = oStar;
						break;
				case 7:
						colCreate = "b";
						createItem = itemBlue;
						tempStar = bStar;
						break;
				case 8:
						createItem = itemPurple;
						colCreate = "p";
						tempStar = pStar;
						break;
				default:
						break;
				}
//				float tempY = (Random.Range (mDown * 100, mUp * 100)) / 100;
				if (LEVEL > 5) {
						if (!isUndead && !isMonster) {
								existItem = Instantiate (createItem, new Vector3 (tempX, 7, 0), Quaternion.identity) as GameObject;

						}
				}
		}
		//BACK
		void backCreate ()
		{
				if (superLevel > 0 && superLevel < 20) {
						float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100f;
						if (spaceId < 2) {
								Instantiate (backElement, new Vector3 (tempX, 8, 0), Quaternion.identity);
						} else {
//				Debug.Log(superLevel);
								if (superLevel > 5) {
										Instantiate (lightSpeed, new Vector3 (tempX, 8, 0), Quaternion.identity);
								} else {
										Instantiate (stars [Random.Range (0, 6)], new Vector3 (tempX, 8, 0), Quaternion.identity);
								}
						}
				} else if (superLevel == 20 && score > 30) {
					
						float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
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
				float tempX = (Random.Range (mLeft * 100, mRight * 100)) / 100;
				float tempY = (Random.Range (mDown * 100, mUp * 100)) / 100;
				if (LEVEL > 1) {
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

		void getItem ()
		{
				switch (numHave) {
				case 0:
						colHave1 = colCreate;
						if (colCreate.Equals ("b"))
								resetStar (1);
						if (colCreate.Equals ("o"))
								resetStar (2);
						if (colCreate.Equals ("p"))
								resetStar (3);
						star1.sprite = tempStar;
						numHave++;
						StartCoroutine ("monster", colCreate);
						audio.PlayOneShot (itemSound);
//						StartCoroutine ("monster", colCreate);
					
			//						StartCoroutine ("undead", 6f);
//						StartCoroutine ("monster", colHave1);
//						StopCoroutine ("undead",4f);
						break;
			
				case 1:
						if (colHave1 == colCreate) {
								numHave++;
//								StartCoroutine ("getAnim", GameObject.Find ("star2"));
//						
//								balloon.SendMessage ("biggerBomb", true);		
								audio.PlayOneShot (itemSound);
								star2.sprite = tempStar;
						} else {
								
//								balloon.SendMessage ("biggerBomb", false);		
//								Destroy (GameObject.FindGameObjectWithTag ("monster"));
//								balloon.SendMessage ("resetMonster");

								resetStar (0);
								getItem ();
						}
						break;
		
				case 2:
						audio.PlayOneShot (itemSound);
						if (colHave1 == colCreate) {
								//monster
								isMonster = true;
								star3.sprite = tempStar;
								isUndead = true;
								balloon.SendMessage ("undead", true);
								undeadTime = 3.5f;
//								if (colCreate.Equals ("b"))
//										Instantiate (monsterB, new Vector2 (0, 0), Quaternion.identity);
//								if (colCreate.Equals ("o"))
//										Instantiate (monsterO, new Vector2 (0, 0), Quaternion.identity);
//								if (colCreate.Equals ("p"))
//										Instantiate (monsterP, new Vector2 (0, 0), Quaternion.identity);
								//moster
								StartCoroutine ("monster", colCreate);
								

						} else {
								isMonster = false;
								resetStar (0);
								getItem ();
						}
						break;
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

		void monsterOrange ()
		{
				isMonster = false;
				if (superLevel < levelLimit) {
						superLevel++;
						superMode (superLevel);
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

				oStars.animation.Play ();

				yield return new WaitForSeconds (0.5f);
				StartCoroutine ("getAnim", GameObject.Find ("star1"));
				StartCoroutine ("getAnim", GameObject.Find ("star2"));
				StartCoroutine ("getAnim", GameObject.Find ("star3"));
				 
			
		
		
				if (mColHave.Equals ("b"))
						Instantiate (monsterB, new Vector2 (0, 0), Quaternion.identity);
				if (mColHave.Equals ("o"))
						Instantiate (monsterO, new Vector2 (0, 0), Quaternion.identity);
				if (mColHave.Equals ("p"))
						Instantiate (monsterP, new Vector2 (0, 0), Quaternion.identity);
//				Instantiate (monsterEffect, new Vector2 (0, 0), Quaternion.identity);
				yield return new WaitForSeconds (1f);
				resetStar (0);
//				resetStar ();
		}

		void resetStar (int num)
		{
				 
				star1.sprite = sItems [num];
				star2.sprite = sItems [num];
				star3.sprite = sItems [num];
						 
				numHave = 0;
		}

		IEnumerator getAnim (GameObject star)
		{
				star.GetComponent<Animator> ().SetInteger ("item", 1);
				yield return new WaitForSeconds (0.5f);
				star.GetComponent<Animator> ().SetInteger ("item", 0);
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
//				timer--;
//				oAirs [timer].animation.Play ();
				superLevel = 1;
				if (item1 == 1)
						superLevel = 2;
//		superLevel = 5;
		
				//TEST
//				superLevel = 4;
//				superLevel = 3;
				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 1);
				balloon.SetActive (true);
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
				existBalloon = true;
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
//						balloon.transform.localScale = new Vector3 (0, 0, 0);
			
						
			
			
						//						if (num == 1)
//						balloon.SetActive (false);
//						if (timer == 0)
//								StartCoroutine ("timesUp");
			
						
					
						 
						//---------------
			
			
						
				}
		
				
		
				//				Debug.Log ("removeTimer");
		
		}

		void timeOut ()
		{
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
				if (existBalloon && superLevel < levelLimit) {
						
						previousBalloon = zonePosition;
						currentBalloon = balloon.transform.position;
						Vector2 tempVector = currentBalloon - previousBalloon;
						float zoneLimit = 1.4f;

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
				} else if (existBalloon && superLevel == levelLimit) {
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
				} else if (LEVEL != 6 && LEVEL != 7 && LEVEL != 8) {
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
		
				//********************score
				CancelInvoke ("scoreCount");
				switch (num) {
				case 0:
						InvokeRepeating ("scoreCount", 0.1f, levelRate [0]);
						InvokeRepeating ("backCreate", 0, levelRate [0] * 8);
						
						break;
				case 1:
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 8);
						break;
				case 2:
						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 8);
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.2";
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						audio.PlayOneShot (levelUp);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
			//LEVEL2
						if (LEVEL == 2 && onPlay) {
								PlayerPrefs.SetInt ("2", 1);
								StartCoroutine ("timesUp");
						}
						
						break;
				case 3:
						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 8);
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.3";
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
			//LEVEL3
						if (LEVEL == 3 && onPlay) {
								PlayerPrefs.SetInt ("3", 1);
								StartCoroutine ("timesUp");
						}
						break;
			
				 

				case 4:
						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 12);
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.4";
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = superBalloon4;
			//LEVEL4
						if (LEVEL == 4 && onPlay) {
								PlayerPrefs.SetInt ("4", 1);
								StartCoroutine ("timesUp");
						}
						break;

				case 5:
						enemyBomb ();
						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 20);
						
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.5";
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = superBalloon5;
			//LEVEL5
						if (LEVEL == 5 && onPlay) {
								PlayerPrefs.SetInt ("5", 1);
								StartCoroutine ("timesUp");
						}
						break;
				case 6:
						enemyBomb ();
						turnLightSpeed (120);
						InvokeRepeating ("backCreate", 0, levelRate [4] * 10);
						scoreRate = scoreRates [num - 6];
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv.6";
						turnLightSpeed (120);
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = ufos [0];
			//LEVEL5
						 
						break;
				case 7:
						enemyBomb ();
						turnLightSpeed (120);
						InvokeRepeating ("backCreate", 0, levelRate [4] * 10);
						scoreRate = scoreRates [num - 6];
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv." + num;
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//LEVEL5
			
						break;
				case 8:
						enemyBomb ();
						turnLightSpeed (120);
						InvokeRepeating ("backCreate", 0, levelRate [4] * 15);
						scoreRate = scoreRates [num - 6];
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv." + num;
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//LEVEL5
			
						break;
				case 9:
						enemyBomb ();
						turnLightSpeed (120);
						InvokeRepeating ("backCreate", 0, levelRate [4] * 20);
						scoreRate = scoreRates [num - 6];
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv." + num;
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//LEVEL5
			
						break;
				case 10:
						enemyBomb ();
						turnLightSpeed (120);
						InvokeRepeating ("backCreate", 0, levelRate [4] * 20);
						scoreRate = scoreRates [num - 6];
						isUndead = true;
						balloon.SendMessage ("undead", true);
						undeadTime = 3;
						lv.SendMessage ("levelUp");
						lvText.text = "Lv." + num;
						audio.PlayOneShot (levelUp);
						Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity);
						Instantiate (effectSuper2, new Vector2 (0, 0), Quaternion.identity);
			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
						Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity);
						if (!isMonster)
								balloonSprite.sprite = ufos [num - 6];
			//LEVEL5
			
						break;
		 
				case 20:
						InvokeRepeating ("scoreCount", 0.1f, 0.1f);
						InvokeRepeating ("backCreate", 0, levelRate [1] * 8);
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
				//LEVEL1
				if (LEVEL == 1 && onPlay && score > 20) {
						PlayerPrefs.SetInt ("1", 1);
						StartCoroutine ("timesUp");
				}
				if (LEVEL == 9 && onPlay && score > 1000) {
						PlayerPrefs.SetInt ("9", 1);
						StartCoroutine ("timesUp");
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
						
			
				}
				if (score > spacesHeight [spaceId] && spaceId == 9) {
						spaceId = 10;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
			
						enemyCreateRate = enemyRates [spaceId];
						
				}
				if (score > spacesHeight [spaceId] && spaceId == 10) {
						spaceId = 11;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
			
				}
				if (score > spacesHeight [spaceId] && spaceId == 11) {
						spaceId = 12;
						Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
						enemyCreateRate = enemyRates [spaceId];
						
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
						resetStar (0);
						break;
				case 5:
						isUndead = true;
						if (onPlay)
								balloon.SendMessage ("undead", true);
						undeadTime = 1.5f;
						isMonster = false;
						break;
				case 6:
			//LEVEL6,7
						killNum++;
						if (LEVEL == 6 && onPlay && killNum > 5) {
								onPlay = false;
								PlayerPrefs.SetInt ("6", 1);
								StartCoroutine ("timesUp");
						}
						if (LEVEL == 7 && onPlay && killNum > 5) {
								onPlay = false;
								PlayerPrefs.SetInt ("7", 1);
								StartCoroutine ("timesUp");
						}
						break;
				case 7:
			//LEVEL8
						if (oBoss != null) 
								Destroy (oBoss);
						Debug.Log (GameObject.FindGameObjectWithTag ("boss"));
						if (LEVEL == 8 && onPlay) {
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
				//				PlayerPrefs.SetInt ("LEVEL", 9);
				////				PlayerPrefs.SetInt ("LEVEL", 2);
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
				countGem2 = PlayerPrefs.GetInt ("NUMGEM2");
				countGem3 = PlayerPrefs.GetInt ("NUMGEM3");
				//				if (test)
				//						Instantiate (testBack, new Vector2 (0, 0), Quaternion.identity);
				//						
				////				timer = gameTime;
				star1 = GameObject.Find ("star1").GetComponent<SpriteRenderer> ();
				star2 = GameObject.Find ("star2").GetComponent<SpriteRenderer> ();
				star3 = GameObject.Find ("star3").GetComponent<SpriteRenderer> ();
				balloonSprite = balloon.GetComponentInChildren<SpriteRenderer> ();
				scoreText = GameObject.Find ("score").GetComponent<tk2dTextMesh> ();
				lvText = GameObject.Find ("lv").GetComponent<tk2dTextMesh> ();
				//				superTimer = superTime;
				mUp = 5.5f;
				mDown = mUp * -1;
				//				existBalloon = false;
				mLeft = GameObject.Find ("lv").transform.position.x;
				mRight = mLeft * -1;
				////				Debug.Log ("screenSize=" + mUp + " " + mDown + " " + mLeft + " " + mRight + " ");
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
				if (onToast) {
						if (e.Selection == monsterIcons [0]) {
								if (!selectedMonster1)
										selectedMonsterNum++;
								selectedMonster1 = true;
								e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
								 
						}
						if (e.Selection == monsterIcons [1]) {
								if (!selectedMonster2)
										selectedMonsterNum++;
								selectedMonster2 = true;
								e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
								 
						}
						if (e.Selection == monsterIcons [2]) {
								if (!selectedMonster3)
										selectedMonsterNum++;
								selectedMonster3 = true;
								e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
								 
						}
				}
				if (e.Selection == btn_menu) {
						btn_menu.GetComponent<SpriteRenderer> ().color = Color.yellow;
				}
				if (e.Selection == btn_replay) {
						btn_replay.GetComponent<SpriteRenderer> ().color = Color.yellow;
//												Application.LoadLevel (0);
				}
				if (e.Selection == btnNext) {
						btnNext.GetComponent<SpriteRenderer> ().color = Color.yellow;
						//												Application.LoadLevel (0);
				}
				if (!existBalloon && onPlay && e.Selection != btn_pause) {
						existBalloon = true;
						Create (GetWorldPos (e.Position));
				}
				if (!existBalloon && !onPlay && e.Selection == balloon && onCount) {
						onPlay = true;
//			balloon.SendMessage("stopCount");
						onCount = false;
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
				GameObject[] temp = GameObject.FindGameObjectsWithTag ("light");
				for (int i=0; i<temp.Length; i++) {
						Destroy (temp [i]);
				}
				if (onToast) {
						if (e.Selection == monsterIcons [0] || e.Selection == monsterIcons [1] || e.Selection == monsterIcons [2]) {
								 
								if (selectedMonsterNum == 2) {
										Destroy (GameObject.Find ("toast 1(Clone)"));
										onToast = false;
										gameStart ();
								}
						}
						 
				}
				if (e.Selection == btn_menu) {
						btn_menu.GetComponent<SpriteRenderer> ().color = Color.white;
						Time.timeScale = 1.0f;
//						gameReset ();
						Application.LoadLevel (1);
				}
				if (e.Selection == btnNext) {
						btnNext.GetComponent<SpriteRenderer> ().color = Color.white;
						admob.SendMessage ("ShowInterstitial");
//						PlayerPrefs.SetInt (LEVEL + "", 1);
//			LEVEL = 9;
						if (LEVEL < 10) {
								LEVEL++;
								PlayerPrefs.SetInt ("LEVEL", LEVEL);
								if (LEVEL == 10)
										Application.LoadLevel (1);
						}

						Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
						gameReset ();
						//						gameReset ();
//						Application.LoadLevel (1);
				}
				if (e.Selection == btn_resume) {
						btn_resume.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						onPlay = true;
						Time.timeScale = 1.0f;
				}
				if (e.Selection == btn_replay) {
						admob.SendMessage ("ShowInterstitial");
						btn_replay.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
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
