using UnityEngine;
using System.Collections;

public class scr_monParent : MonoBehaviour
{
 
		public float speed;
		GameObject oBalloon;
		public GameObject bombO;
		public int isOrange;
		bool onTimer = false;
		int time = 6;
		// Use this for initialization
		void Start ()
		{
				oBalloon = GameObject.FindGameObjectWithTag ("balloon");
		}
	
		// Update is called once per frame
		void Update ()
		{
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, oBalloon.transform.position, step);
				if (transform.position == oBalloon.transform.position && !onTimer) {
						onTimer = true;
						InvokeRepeating ("Timer", 0f, 0.5f);
//						GameObject.Find ("GAMEMANAGER").SendMessage ("monsterOrange");
//						Instantiate (bombO, transform.position, Quaternion.identity);
//						Destroy (this.gameObject);


				}

		}

		void Timer ()
		{
		time--;
		GetComponentInChildren<tk2dTextMesh>().text = time+"";
		if (time == 0) {
			CancelInvoke("Timer");
			oBalloon.SendMessage("monsterShot");
			GameObject bomb;
			bomb = Instantiate (bombO, transform.position, Quaternion.identity) as GameObject;
			bomb.SendMessage ("onTimer", 5f);
				}
		}
		
}

