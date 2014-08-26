using UnityEngine;
using System.Collections;

public class mat : MonoBehaviour
{
		public int xID, yID;
       
		// Use this for initialization
		void Start ()
		{
				xID = int.Parse (gameObject.transform.parent.name);
				yID = int.Parse (gameObject.transform.name);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void setMelody ()
		{
				VALUE.melody [xID] = yID;

		}

		
}
