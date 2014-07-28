using UnityEngine;
using System.Collections;

public class success : MonoBehaviour
{
		public Sprite failed;
		public GameObject gemEffect;

		// Use this for initialization
		void Start ()
		{
		
				if (!Value.isQuest) {
						int level = PlayerPrefs.GetInt ("LEVEL", 0);
						int success = PlayerPrefs.GetInt ("" + level, 0);
//		Debug.Log(level+ " " +success);
						if (level == 10) {
								Destroy (this.gameObject);
						} else	if (success != 1) {
								GetComponent<SpriteRenderer> ().sprite = failed;
						}
				} else {
						int level = PlayerPrefs.GetInt ("QUEST" + Value.questNum, 0);
						Debug.Log (level + " " + Value.questLevel);
						if (level == Value.questLevel) {
								GetComponent<SpriteRenderer> ().sprite = failed;
						}
				}
		}

		
		 
		// Update is called once per frame
		void Update ()
		{
	
		}
}
