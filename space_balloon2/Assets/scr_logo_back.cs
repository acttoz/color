using UnityEngine;
using System.Collections;

public class scr_logo_back : MonoBehaviour
{
		public GameObject menu, mainCam, pop,oUfo;
		public AudioClip ufo, crush, down, scream;
		// Use this for initialization
		void Start ()
		{
	
		}

		void destroy ()
		{
				Application.LoadLevel (1);

		}

		// Update is called once per frame
		void Update ()
		{
	
		}

		void audioUfo ()
		{
				audio.loop = true;
				audio.clip = ufo;
				audio.Play ();
		}

		void audioCrush ()
		{
				audio.PlayOneShot (crush);
				Instantiate (pop, oUfo.transform.position, Quaternion.identity);
		}

		void audioDown ()
		{
				audio.PlayOneShot (down);
		}
}
