using UnityEngine;
using System.Collections;

public class Mats : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void Stop ()
		{
				GameObject[] oColorMats = GameObject.FindGameObjectsWithTag ("color");
				for (int i=0; i<oColorMats.Length; i++)
						oColorMats [i].transform.tag = "mat";
				GameObject[] oMats = GameObject.FindGameObjectsWithTag ("mat");
				for (int i=0; i<oMats.Length; i++)
						oMats [i].SendMessage ("endPlus");
		}

		public void reset ()
		{
				GameObject[] oMats = GameObject.FindGameObjectsWithTag ("mat");
				STATE.mats = 0;
				STATE.matsAll = oMats.Length;
				for (int i=0; i<oMats.Length; i++)
						oMats [i].SendMessage ("reset");
		}
}
	