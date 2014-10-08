using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
		int selectedMonsterNum = 0;
		int numHave = 0;
		public float itemCreateRate;
		bool selectedMonster1 = true;
		bool selectedMonster2 = true;
		string colHave1 = "n", colCreate = "n";//b;o,p
		bool selectedMonster3 = false;
		GameObject existItem, createItem;
		public Sprite tempStar, oStar, pStar, bStar;
		public Sprite[] sItems;
		GameObject oStars;
		public GameObject monsterB, monsterO, monsterP;
		public GameObject itemBlue, itemOrange, itemPurple;
		public GameObject itemEffectO, itemEffectB, itemEffectP, itemEffectBack;
		public SpriteRenderer star1, star2, star3;
		public static Item instance;
		// Use this for initialization
		void Start ()
		{
				instance = this;
				star1 = GameObject.Find ("star1").GetComponent<SpriteRenderer> ();
				star2 = GameObject.Find ("star2").GetComponent<SpriteRenderer> ();
				star3 = GameObject.Find ("star3").GetComponent<SpriteRenderer> ();
				oStars = GameObject.Find ("stars");
				itemCreateStart ();
				resetStar (0);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void itemCreateStart ()
		{
				InvokeRepeating ("itemCreate", 1f, itemCreateRate);
		
		}

		void itemCreate ()
		{
				if (!MANAGER.STATE.Equals ("IDLE"))
						return;
				float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100;
//				if (existItem != null)
//						Destroy (existItem);
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
				
								createItem = itemOrange;
								colCreate = "o";
								tempStar = oStar;
						} else {
								createItem = itemBlue;
								colCreate = "b";
								tempStar = bStar;
						}
						break;
			
				case 3:
						if (selectedMonster3) {
				
//								if (oBoss != null) {
//										createItem = itemPurple;
//										colCreate = "p";
//										tempStar = pStar;
//								} else {
								if (selectedMonster1) {
										createItem = itemBlue;
										colCreate = "b";
										tempStar = bStar;
								} else {
										createItem = itemOrange;
										colCreate = "o";
										tempStar = oStar;
								}
//								}
						} else {
								createItem = itemBlue;
								colCreate = "b";
								tempStar = bStar;
						}
						break;
				case 4:
						if (selectedMonster3) {
//								if (oBoss != null) {
//										createItem = itemPurple;
//										colCreate = "p";
//										tempStar = pStar;
//								} else {
								if (selectedMonster1) {
										createItem = itemBlue;
										colCreate = "b";
										tempStar = bStar;
								} else {
										createItem = itemOrange;
										colCreate = "o";
										tempStar = oStar;
								}
//								}
						} else {
								createItem = itemOrange;
								colCreate = "o";
								tempStar = oStar;
						}
						break;
			
			
				}
		
				switch (Level.instance.getLevel()) {
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
				//				float tempY = (Random.Range (MANAGER.mDown * 100, MANAGER.mUp * 100)) / 100;
				if (Level.instance.getLevel() > 5) {
						if (MANAGER.STATE.Equals ("IDLE")) {
								existItem = Instantiate (createItem, new Vector3 (tempX, MANAGER.mUp, 0), Quaternion.identity) as GameObject;
				
						}
				}
		}

		public void getItem ()
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
//						if (testItem)
//								StartCoroutine ("monster", colCreate);
						//audio.PlayOneShot (itemSound);
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
								//audio.PlayOneShot (itemSound);
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
						//audio.PlayOneShot (itemSound);
						if (colHave1 == colCreate) {
								//monster
//								isMonster = true;
//								questEnemy = 0;
//								if (Value.isQuest)
//										oEnemyNum.GetComponent<tk2dTextMesh> ().text = 0 + "";
				
								star3.sprite = tempStar;
								MANAGER.STATE = "UNDEAD";
//								balloon.SendMessage ("undead", true);
//								undeadTime = 3.5f;
								//								if (colCreate.Equals ("b"))
								//										Instantiate (monsterB, new Vector2 (0, 0), Quaternion.identity);
								//								if (colCreate.Equals ("o"))
								//										Instantiate (monsterO, new Vector2 (0, 0), Quaternion.identity);
								//								if (colCreate.Equals ("p"))
								//										Instantiate (monsterP, new Vector2 (0, 0), Quaternion.identity);
								//moster
								StartCoroutine ("monster", colCreate);
				
				
						} else {
//								isMonster = false;
								resetStar (0);
								getItem ();
						}
						break;
				}
		
		
		}

//		public void itemUse (string col)
//		{
//				if (col.Equals ("b")) {
//						//						timer += 10;
//						//audio.PlayOneShot (levelUp);
//						Instantiate (itemEffectBack, itemEffectB.transform.position, Quaternion.identity);
//				}
//				if (col.Equals ("o")) {
//						//audio.PlayOneShot (levelUp);
//						//						StopCoroutine ("scoreUp");
//						//						StartCoroutine ("scoreUp");
//						Instantiate (itemEffectBack, itemEffectO.transform.position, Quaternion.identity);
//				}
//				if (col.Equals ("p")) {
//						//audio.PlayOneShot (levelUp);
//						Instantiate (itemEffectBack, itemEffectP.transform.position, Quaternion.identity);
////						if (gauge.transform.localScale.y > 1.3) {
////								float temp = 1.74f - gauge.transform.localScale.y;
////								gauge.transform.localScale += new Vector3 (0, temp, 0);
////						} else {
////								gauge.transform.localScale += new Vector3 (0, 0.5f, 0);
////						}
//			
//				}
//		}

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
	
		public void resetStar (int num)
		{
				Debug.Log (star1);
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

//		public void monsterChoice ()
//		{
//				if (MANAGER.Level.level > 8) {
//						Instantiate (monsterIcons [3], new Vector2 (0, 0), Quaternion.identity);
//						monsterIcons [0] = GameObject.Find ("btn1");
//						monsterIcons [1] = GameObject.Find ("btn2");
//						monsterIcons [2] = GameObject.Find ("btn3");
//						selectedMonsterNum = 0;
//						selectedMonster1 = false;
//						selectedMonster2 = false;
//						selectedMonster3 = false;
//						for (int i=0; i<3; i++) {
//								monsterIcons [i].GetComponent<SpriteRenderer> ().color = new Color (0.5f, 0.5f, 0.5f, 1);
//						}
//				} else {
//						if (PlayerPrefs.GetInt ("TUTO1", 0) == 0) {
//								PlayerPrefs.SetInt ("TUTO1", 1);
//								Instantiate (oTutorial1, new Vector2 (0, 0), Quaternion.identity);
//						} else {
//								gameStart ();
//						}
//			
//				}
//		}
}
