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
				Instantiate (prf_success, new Vector2 (0, 0), Quaternion.identity);
		
		}

		public void fail ()
		{
				Instantiate (prf_fail, new Vector2 (0, 0), Quaternion.identity);
		}

		public void reset ()
		{
				Debug.Log ("reset");
		}

		public void ready ()
		{
				Instantiate (prf_ready, new Vector2 (0, 0), Quaternion.identity);
		}
}
