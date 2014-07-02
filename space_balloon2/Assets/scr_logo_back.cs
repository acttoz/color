using UnityEngine;
using System.Collections;

public class scr_logo_back : MonoBehaviour
{
		public GameObject menu, mainCam, pop, oUfo, explosion, explosion2, ufoSound;
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

		void OnTap ()
		{
				Application.LoadLevel (1);
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

		void explosionCreate ()
		{
				explosion.SetActive (true);
		}
	
		void explosionCreate2 ()
		{
				explosion2.SetActive (true);
		}

		void audioDown ()
		{
				audio.PlayOneShot (down);
		}
}
