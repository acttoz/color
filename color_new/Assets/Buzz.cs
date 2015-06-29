using UnityEngine;
using System.Collections;

public class Buzz : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void reset ()
		{
				STATE.buzz = 0;
		}

		public void get ()
		{
				GameObject[] oBuzzes = GameObject.FindGameObjectsWithTag ("buzz");
				STATE.buzz = oBuzzes.Length;
				for (int i=0; i<oBuzzes.Length; i++) {
						oBuzzes [i].SendMessage ("score");
				}
		}

		public void lose ()
		{
				GameObject[] oBuzzes = GameObject.FindGameObjectsWithTag ("buzz");
				for (int i=0; i<oBuzzes.Length; i++)
						Destroy (oBuzzes [i]);
		}
}
