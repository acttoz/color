using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{

		public GameObject prf_ready, prf_success, prf_fail, oStarTimer;
		private Enemy iEnemy;
		private Mats iMats;
//		public static MANAGER mInstance;
		// Use this for initialization
//		public MANAGER ()
//		{
//				mInstance = this;
//		}
//	
//		public static MANAGER instance {
//				get {
//						if (mInstance == null)
//								new MANAGER ();
//						return mInstance;
//				}
//		}

		// Use this for initialization
		void Start ()
		{
				iEnemy = GetComponent<Enemy> ();
				iMats = GetComponent<Mats> ();
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
				iMats.Stop ();
		
		}

		public void fail ()
		{
				oStarTimer.animation.Stop ();
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
			
				GameObject.FindGameObjectWithTag ("MainCamera").animation.Play ();
//				reset ();
				Instantiate (prf_fail, new Vector2 (0, 0), Quaternion.identity);
				iMats.Stop ();
		}

		public void reset ()
		{
				//START
				oStarTimer.animation.Rewind ();
				oStarTimer.animation.Play ();
				iEnemy.reset ();
				iMats.reset ();
				

	
		
				//				Debug.Log ("reset");
		}

		public void ready ()
		{
				Instantiate (prf_ready, new Vector3 (0, 0, 0), Quaternion.identity);
		}
}
