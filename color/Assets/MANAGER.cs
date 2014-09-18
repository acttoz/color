using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
		public GameObject prf_ready, prf_success, prf_fail;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void success ()
		{
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
				Instantiate (prf_success, new Vector2 (0, 0), Quaternion.identity);
		
		}

		public void fail ()
		{
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
				reset ();
				Instantiate (prf_fail, new Vector2 (0, 0), Quaternion.identity);
		}

		public void reset ()
		{
				GameObject[] oMats = GameObject.FindGameObjectsWithTag ("mat");
				STATE.mats = 0;
				STATE.matsAll = oMats.Length;
				for (int i=0; i<oMats.Length; i++)
						oMats [i].SendMessage ("reset");

//				Debug.Log ("reset");
		}

		public void ready ()
		{
				Instantiate (prf_ready, new Vector2 (0, 0), Quaternion.identity);
		}
}
