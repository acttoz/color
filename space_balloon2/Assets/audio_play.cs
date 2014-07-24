using UnityEngine;
using System.Collections;

public class audio_play : MonoBehaviour
{
		public AudioClip[] audios;
		public int audioNum;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void audioPlay ()
		{
				audio.PlayOneShot (audios [audioNum]);
		}

		void getGem ()
		{
				if (Value.isQuest) {
						int level = PlayerPrefs.GetInt ("QUEST" + Value.questNum, 0);
						if (level != Value.questLevel) {
								//success
								GameObject.Find ("gem").animation.Play ();
								PlayerPrefs.SetInt ("QUEST" + Value.questNum, (Value.questLevel));
								PlayerPrefs.SetInt ("NUMGEM", PlayerPrefs.GetInt ("NUMGEM", 0) + Value.questGem [Value.questLevel]);
						}
				}
		}


}
