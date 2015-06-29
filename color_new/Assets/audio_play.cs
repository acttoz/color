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


}
