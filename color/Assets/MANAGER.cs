using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{
		public GameObject prf_ready, prf_success, prf_fail, prf_enemy, oStarTimer;
		public static GameObject _MANAGER;


		// Use this for initialization
		void Start ()
		{
				_MANAGER = this.gameObject;
		}
	
		// Update is called once per frame
		void Update ()
		{
	 
		}

		public void success ()
		{
				oStarTimer.animation.Stop ();
		
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
				Instantiate (prf_success, new Vector2 (0, 0), Quaternion.identity);
		
		}

		public void fail ()
		{
				oStarTimer.animation.Stop ();
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
				GameObject[] oMats = GameObject.FindGameObjectsWithTag ("mat");
				for (int i=0; i<oMats.Length; i++)
						oMats [i].SendMessage ("endPlus");
				GameObject.FindGameObjectWithTag ("MainCamera").animation.Play ();
//				reset ();
				Instantiate (prf_fail, new Vector2 (0, 0), Quaternion.identity);
		}

		public void reset ()
		{
				//START
				oStarTimer.animation.Rewind ();
				oStarTimer.animation.Play ();
				GameObject[] oMats = GameObject.FindGameObjectsWithTag ("mat");
				GameObject[] oEnemies = GameObject.FindGameObjectsWithTag ("enemy");
				for (int i=0; i<oEnemies.Length; i++)
						Destroy (oEnemies [i]);
				for (int i=0; i<5; i++)
						Instantiate (prf_enemy, new Vector2 (Random.Range (-3f, 3f), Random.Range (-2f, 2f)), Quaternion.identity);
				STATE.mats = 0;
				STATE.matsAll = oMats.Length;
				for (int i=0; i<oMats.Length; i++)
						oMats [i].SendMessage ("reset");

//				Debug.Log ("reset");
		}

		public void ready ()
		{
				Instantiate (prf_ready, new Vector3 (0,0, 0), Quaternion.identity);
		}
}
