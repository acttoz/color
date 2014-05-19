using UnityEngine;
using System.Collections;

public class src_balloon : MonoBehaviour
{
		float bombSize = 0.4f;
		Animator anim;
		GameObject bomb;
//		public AudioClip itemSound;
		bool exist = false;
		public GameObject GAMEMANAGER, item;
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
				exist = true;
//				if (num < 4)
				GetComponent<SpriteRenderer> ().sprite = balloon;
//				Debug.Log ("OnEnable()");
				anim = GetComponent<Animator> ();
				anim.SetBool ("balloonExist", true);
				anim.SetInteger ("super", num);
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

		void superMode (int num)
		{
				anim.SetInteger ("super", num);
		
		}

		void stopBalloon (bool stop)
		{
				anim.SetBool ("stop", stop);
	
		}

		void onMonster (string temp)
		{
				monster = temp;
				isMonster = true;
		}

		void offMonster ()
		{
				isMonster = false;
		}

		void biggerBomb (bool temp)
		{
				if (temp) {
						bombSize += 0.2f;
						transform.parent.localScale += new Vector3 (0.2f, 0.2f, 0f);
				} else {

						bombSize = 0.4f;
						transform.parent.localScale = new Vector3 (0.5f, 0.5f, 0.4f);
				}
		}

		void OnTriggerEnter (Collider myTrigger)
		{
				if (myTrigger.transform.tag == "enemy" && exist && !isUndead && !isMonster) {
						exist = false;
						GAMEMANAGER.SendMessage ("getBalloonMSG", 1);
						
				}
				if (myTrigger.transform.tag == "enemy" && exist && !isUndead && isMonster) {
						undead (true);
						GAMEMANAGER.SendMessage ("getBalloonMSG", 5);
						offMonster ();
						if (monster.Equals ("b")) {
								bomb = Instantiate (effects [0], transform.position, Quaternion.identity) as GameObject;
						}
						if (monster.Equals ("o")) {
								bomb = Instantiate (effects [1], transform.position, Quaternion.identity) as GameObject;
						}
						if (monster.Equals ("p")) {
								bomb = Instantiate (effects [2], transform.position, Quaternion.identity) as GameObject;
						}
						bomb.transform.localScale = new Vector2 (bombSize, bombSize);
//						bomb.transform.parent = transform;
						switch (scr_manager.superLevel) {
						case 4:
								GetComponent<SpriteRenderer> ().sprite = rainbow;
								break;
						case 5:
								GetComponent<SpriteRenderer> ().sprite = hot;
								break;
						default:
								GetComponent<SpriteRenderer> ().sprite = balloon;
								break;
					
						}
						
						Destroy (GameObject.FindGameObjectWithTag ("monster"));
						GAMEMANAGER.SendMessage ("getBalloonMSG", 4);
						biggerBomb (false);
			
			
				}
				if (myTrigger.transform.tag == "item") {
						GAMEMANAGER.SendMessage ("getItem");
//						audio.PlayOneShot (itemSound);
						Instantiate (item, myTrigger.gameObject.transform.position, Quaternion.identity);
						Destroy (myTrigger.gameObject);
			
			
				}
		 
		}

		 
	 
}
