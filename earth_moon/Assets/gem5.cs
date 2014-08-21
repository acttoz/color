using UnityEngine;
using System.Collections;

public class gem5 : MonoBehaviour
{

		// Use this for initialization

		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void getGem ()
		{
				animation.Play ();
				PlayerPrefs.SetInt ("NUMGEM", PlayerPrefs.GetInt ("NUMGEM") + 5);
				audio.Play ();
		}

}
