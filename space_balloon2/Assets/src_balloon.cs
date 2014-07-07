using UnityEngine;
using System.Collections;

public class src_balloon : MonoBehaviour
{
//		float bombSize = 0.4f;
//		float[] levels = new float[]{0,0.5f,2f,4f,6f,10f,15f,20f,25f};
		Animator anim;
		GameObject bomb;
//		int monsterLevel = 1;
		public Sprite[] ufos;
		bool onCount = false;
		public AudioClip sCountDown;
		bool exist = false;
		int countDownTime = 3;
		public GameObject GAMEMANAGER, item, shine, timer, oCountDown;
		public GameObject[] effects = new GameObject[3];
		public Sprite   balloon, rainbow, hot;
		bool isUndead = false;
		bool isMonster = false;
		string monster;
		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();

		}

		// Update is called once per frame
		void Update ()
		{
	
		}

		void create (int num)
		{
				Debug.Log ("create");
				onCount = false;
//				fire.SetActive (false);
				exist = true;
				isUndead = false;
//				if (num < 4)
				shine.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
		if(num<3)
				GetComponent<SpriteRenderer> ().sprite = balloon;
//				Debug.Log ("OnEnable()");
				oCountDown.SetActive (false);
				anim = GetComponent<Animator> ();
				anim.SetInteger ("cancel", 0);
				anim.SetBool ("balloonExist", true);
				anim.SetInteger ("super", num);
				GAMEMANAGER.SendMessage ("getBalloonMSG", 9);
		}

		void shield ()
		{
				exist = true;
		}

		void cancel (int num)
		{
				Destroy (GameObject.FindGameObjectWithTag ("monster"));
				exist = false;
				anim.SetBool ("balloonExist", false);
				anim.SetInteger ("cancel", num);
				
		}

		void undead (bool mbool)
		{
				isUndead = mbool;
		}

		void startCoundDown ()
		{
				onCount = true;
				Destroy (GameObject.FindGameObjectWithTag ("monster"));
		GetComponent<SpriteRenderer> ().sprite = balloon;
				exist = false;
				anim.SetBool ("balloonExist", false);
				oCountDown.SetActive (true);
				countDownTime = 3;
				countDown ();
				anim.SetInteger ("cancel", 1);
				isUndead = true;
//				oCountDown.animation.Play ();
		}
	
		void countDown ()
		{
		Destroy (GameObject.FindGameObjectWithTag ("monster"));
				if (!onCount)
						return;
				oCountDown.GetComponent<tk2dTextMesh> ().text = "" + countDownTime;
				countDownTime--;
				audio.PlayOneShot (sCountDown);
				if (countDownTime < 0) {
						onCount = false;
						oCountDown.SetActive (false);
						transform.position = new Vector2 (100, 100);
						isUndead = false;
						GAMEMANAGER.SendMessage ("getBalloonMSG", 10);
				}


		}

		void superMode (int num)
		{
				 
				anim.SetInteger ("super", num);
				if (num > 4) {
						shine.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
//						fire.SetActive (true);
				}
			
		}

//		void stopBalloon (bool stop)
//		{
//				anim.SetBool ("stop", stop);
//	
//		}

		void onMonster (string temp)
		{
				
				monster = temp;
				isMonster = true;
				shine.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		}

		void offMonster ()
		{
				
				isMonster = false;
				if (scr_manager.superLevel < 5)
						shine.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				
		}

//		void biggerBomb (bool temp)
//		{
//				if (temp) {
//						monsterLevel++;
//						bombSize += 0.2f;
//						transform.parent.localScale += new Vector3 (0.2f, 0.2f, 0f);
//				} else {
//						monsterLevel = 1;
//						bombSize = 0.4f;
//						transform.parent.localScale = new Vector3 (0.7f, 0.7f, 0.4f);
//				}
//		}

		void OnTriggerEnter (Collider myTrigger)
		{
				if ((myTrigger.transform.tag == "boss" || myTrigger.transform.tag == "enemy") && exist && !isUndead && !isMonster) {
						exist = false;
						anim.SetInteger ("cancel", 0);
						GAMEMANAGER.SendMessage ("getBalloonMSG", 1);
						
				}
//				if ((myTrigger.transform.tag == "boss" || myTrigger.transform.tag == "enemy") && exist && !isUndead && isMonster) {
//						
//			
//			
//				}
				if (myTrigger.transform.tag == "item") {
						GAMEMANAGER.SendMessage ("getItem");
//						audio.PlayOneShot (itemSound);
						Instantiate (item, myTrigger.gameObject.transform.position, Quaternion.identity);
						Destroy (myTrigger.gameObject);
			
			
				}
		 
		}

		void OnTriggerStay (Collider myTrigger)
		{
				if ((myTrigger.transform.tag == "boss" || myTrigger.transform.tag == "enemy") && exist && !isUndead && !isMonster) {
						exist = false;
						anim.SetInteger ("cancel", 0);
						GAMEMANAGER.SendMessage ("getBalloonMSG", 1);
			
				}
//				if ((myTrigger.transform.tag == "boss" || myTrigger.transform.tag == "enemy") && exist && !isUndead && isMonster) {
//						undead (true);
//						GAMEMANAGER.SendMessage ("getBalloonMSG", 5);
//						offMonster ();
//						if (monster.Equals ("b")) {
//								bomb = Instantiate (effects [0], transform.position, Quaternion.identity) as GameObject;
//						}
//						if (monster.Equals ("o")) {
//								bomb = Instantiate (effects [1], transform.position, Quaternion.identity) as GameObject;
//						}
//						if (monster.Equals ("p")) {
//								bomb = Instantiate (effects [2], transform.position, Quaternion.identity) as GameObject;
//						}
//						Debug.Log (levels [monsterLevel]);
////						bomb.transform.localScale = new Vector2 (bombSize, bombSize);
////						bomb.transform.parent = transform;
//						resetMonster ();
//						GAMEMANAGER.SendMessage ("getBalloonMSG", 4);
////						biggerBomb (false);
//			
//			
//				}
				if (myTrigger.transform.tag == "item") {
						GAMEMANAGER.SendMessage ("getItem");
						//						audio.PlayOneShot (itemSound);
						Instantiate (item, myTrigger.gameObject.transform.position, Quaternion.identity);
						Destroy (myTrigger.gameObject);
			
			
				}
		
		}

		void monsterShot ()
		{
				undead (true);
				GAMEMANAGER.SendMessage ("getBalloonMSG", 5);
				offMonster ();
//				if (monster.Equals ("b")) {
//						bomb = Instantiate (effects [0], transform.position, Quaternion.identity) as GameObject;
//						bomb.SendMessage ("onTimer", 2f);
//				}
//				if (monster.Equals ("o")) {
//						bomb = Instantiate (effects [1], transform.position, Quaternion.identity) as GameObject;
//				}
//				if (monster.Equals ("p")) {
//						bomb = Instantiate (effects [2], transform.position, Quaternion.identity) as GameObject;
//				}
				//						bomb.transform.localScale = new Vector2 (bombSize, bombSize);
				//						bomb.transform.parent = transform;
				resetMonster ();
				GAMEMANAGER.SendMessage ("getBalloonMSG", 4);
				//						biggerBomb (false);

		}

		void resetMonster ()
		{
				switch (scr_manager.superLevel) {
				case 4:
						GetComponent<SpriteRenderer> ().sprite = rainbow;
						break;
				case 5:
						GetComponent<SpriteRenderer> ().sprite = hot;
						break;
				case 6:
						GetComponent<SpriteRenderer> ().sprite = ufos [scr_manager.superLevel - 6];
						break;
				case 7:
						GetComponent<SpriteRenderer> ().sprite = ufos [scr_manager.superLevel - 6];
						break;
				case 8:
						GetComponent<SpriteRenderer> ().sprite = ufos [scr_manager.superLevel - 6];
						break;
				case 9:
						GetComponent<SpriteRenderer> ().sprite = ufos [scr_manager.superLevel - 6];
						break;
				case 10:
						GetComponent<SpriteRenderer> ().sprite = ufos [scr_manager.superLevel - 6];
						break;
				default:
						GetComponent<SpriteRenderer> ().sprite = balloon;
						break;
			
				}
				
				Destroy (GameObject.FindGameObjectWithTag ("monster"));
		}

		 

		 
	 
}
