using UnityEngine;
using System.Collections;

public class scr_monster : MonoBehaviour
{
		public string monsterColor;
		public Sprite temp;
		GameObject oBalloon;
		public int isOrange;

		// Use this for initialization
		void Start ()
		{
				oBalloon = GameObject.FindGameObjectWithTag ("balloon");
		}
	
		// Update is called once per frame
		void Update ()
		{
			
		}

		void gogo ()
		{
		if (oBalloon == null)
						Destroy (this.gameObject);
				transform.parent.gameObject.GetComponent<scr_monParent> ().enabled = true;
				transform.parent.parent = oBalloon.transform;
				audio.Play ();
		}

		void itemUse ()
		{
//				GameObject.Find ("GAMEMANAGER").SendMessage ("itemUse", monsterColor);
//				transform.parent.gameObject.animation.Play ();
				
				if (isOrange != 1) {
						oBalloon.GetComponent<SpriteRenderer> ().sprite = temp;
						oBalloon.SendMessage ("onMonster", monsterColor);
				} else {
						GameObject.Find ("GAMEMANAGER").SendMessage ("getBalloonMSG", 5);
				}
		}

		void countDown ()
		{
//				oBalloon.GetComponent<SpriteRenderer> ().sprite = temp;
//				oBalloon.SendMessage ("onMonster", monster);
//				StartCoroutine ("startCount");
		}
	
		
}
