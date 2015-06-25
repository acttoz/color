using UnityEngine;
using System.Collections;

public class MANAGER : MonoBehaviour
{

		public GameObject prf_ready, prf_success, prf_fail, oStarTimer;
		public GameObject[] canvas;
		private GameObject oCanvas;
		private Enemy iEnemy;
		private Mats iMats;
		private Buzz iBuzz;
		private ElectricityLine3D EL;
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
				iBuzz = GetComponent<Buzz> ();
				EL = GetComponent<ElectricityLine3D> ();

		}
	
		// Update is called once per frame
		void Update ()
		{
	 
		}
	
		public void success ()
		{
				oStarTimer.SendMessage ("stopCount");
		
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
				GameObject temp = Instantiate (prf_success, new Vector2 (0, 0), Quaternion.identity) as GameObject;
				temp.transform.position -= new Vector3 (0, 0, 40);
		
				iMats.Stop ();
 
				iBuzz.get ();
				EL.Clear ();
				audio.Stop ();
		
		}

		public void fail ()
		{
				oStarTimer.SendMessage ("stopCount");
				
				Destroy (GameObject.FindGameObjectWithTag ("touch"));
			
				GameObject.FindGameObjectWithTag ("MainCamera").animation.Play ();
//				reset ();
				Instantiate (prf_fail, new Vector2 (0, 0), Quaternion.identity);
				iMats.Stop ();
				EL.Clear ();
				iBuzz.lose ();
				audio.Stop ();
		
		}

		public void reset ()
		{
				iEnemy.reset ();
				iMats.reset ();
				iBuzz.reset ();

				//START
				oStarTimer.animation.Rewind ();
				oStarTimer.animation.Play ();

			
				

	
		
				//				Debug.Log ("reset");
		}

		public void ready ()
		{
				Instantiate (prf_ready, new Vector3 (0, 0, 0), Quaternion.identity);
				//level setting
				if (oCanvas != null)
						Destroy (oCanvas);
				oCanvas = Instantiate (canvas [STATE.LEVEL - 1], new Vector2 (0, 0), Quaternion.identity) as GameObject;
		}
}
