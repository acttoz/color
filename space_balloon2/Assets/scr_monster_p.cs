using UnityEngine;
using System.Collections;

public class scr_monster_p : MonoBehaviour
{
		public string monsterColor;
		public Sprite temp;
		GameObject oBalloon;

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
				transform.parent.gameObject.GetComponent<scr_monParent_p> ().enabled = true;
//				transform.parent.parent = oBalloon.transform;
				audio.Play ();
		}

		void itemUse ()
		{
//				GameObject.Find ("GAMEMANAGER").SendMessage ("itemUse", monsterColor);
//				transform.parent.gameObject.animation.Play ();
				
//				oBalloon.GetComponent<SpriteRenderer> ().sprite = temp;
//				oBalloon.SendMessage ("onMonster", monsterColor);
		}

		void countDown ()
		{
//				oBalloon.GetComponent<SpriteRenderer> ().sprite = temp;
//				oBalloon.SendMessage ("onMonster", monster);
//				StartCoroutine ("startCount");
		}
	
		
}
