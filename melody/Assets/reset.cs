using UnityEngine;
using System.Collections;

public class reset : MonoBehaviour
{
		public GameObject[] objParents;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void resetAll ()
		{
				foreach (GameObject element in objParents) {
						element.SendMessage ("resetColor");
				}
				for (int i=0; i<24; i++) {
						VALUE.melody [i] = 9;
			
				}
				VALUE.melody [0] = 2;
		}

}
