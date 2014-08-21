using UnityEngine;
using System.Collections;

public class Final_audio_play : MonoBehaviour
{
		public GameObject oPlay;
		public string objName;
		// Use this for initialization
		void Start ()
		{
				if (objName != null)
						oPlay = GameObject.Find (objName);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void audioPlay ()
		{
				if (oPlay != null) {
						oPlay.audio.Play ();
				} else {
						audio.Play ();
				}
						
		}
}
