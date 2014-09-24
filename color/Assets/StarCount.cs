using UnityEngine;
using System.Collections;

public class StarCount : MonoBehaviour
{
		public GameObject oStarBloomer;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void starCount (int num)
		{
				STATE.stars = num;
				Debug.Log ("" + STATE.stars);
				if (num == 3) {
						oStarBloomer.animation ["star_bloom"].speed = 1;
				}		
				if (num == 1) {
						oStarBloomer.animation ["star_bloom"].speed = 2;
						audio.pitch = 2;
						
				}
				if (num == 0) {
						STATE._STATE = "gFAIL";
				}
		}
}
