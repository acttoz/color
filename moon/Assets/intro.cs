using UnityEngine;
using System.Collections;

public class intro : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void whic ()
		{
				audio.Play ();
		}

		void gotoLevel ()
		{
				Application.LoadLevel (1);
		}
}
