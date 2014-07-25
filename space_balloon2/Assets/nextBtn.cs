using UnityEngine;
using System.Collections;

public class nextBtn : MonoBehaviour
{
		public Sprite failed;
		public bool isQuest;
		// Use this for initialization
		void Start ()
		{
				if (!Value.isQuest) {
						int level = PlayerPrefs.GetInt ("LEVEL", 0);
						int success = PlayerPrefs.GetInt ("" + level, 0);
						//		Debug.Log(level+ " " +success);
						if (success != 1) {
								transform.parent.GetComponent<CapsuleCollider> ().enabled = false;
								GetComponent<SpriteRenderer> ().sprite = failed;
						} else {
								animation.Play ();
						}
				} else {
						transform.parent.GetComponent<CapsuleCollider> ().enabled = false;
			
						int level = PlayerPrefs.GetInt ("QUEST" + Value.questNum, 0);
						Debug.Log ("" + level);
						Debug.Log ("" + Value.questLevel);
						if (level == Value.questLevel) {
								GetComponent<SpriteRenderer> ().sprite = failed;
						} else {
								animation.Play ();
						}
				}
		}

		 	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
