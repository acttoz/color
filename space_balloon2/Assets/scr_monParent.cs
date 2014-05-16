using UnityEngine;
using System.Collections;

public class scr_monParent : MonoBehaviour
{
 
		public float speed;
		public Sprite temp;
		GameObject oBalloon;
		public string monster;
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
		}

		void countDown ()
		{
				oBalloon.GetComponent<SpriteRenderer> ().sprite = temp;
				StartCoroutine ("startCount");
		}

		IEnumerator startCount ()
		{
				oBalloon.SendMessage ("onMonster", monster);
				yield return new WaitForSeconds (3f);
//				oBalloon.SendMessage ("offMonster");
		}
}

