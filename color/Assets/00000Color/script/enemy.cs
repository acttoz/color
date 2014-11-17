using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
		public GameObject prf_enemy, prf_thief;
		private GameObject[] oMats;//parent of Colored mats
		private int bossIds;
	
		void Start ()
		{
		}
	
		void Update ()
		{
		}
	
		public void reset ()
		{
		
				GameObject[] oEnemies = GameObject.FindGameObjectsWithTag ("enemy");
				for (int i=0; i<oEnemies.Length; i++)
						Destroy (oEnemies [i]);
				for (int i=0; i<5; i++)
						Instantiate (prf_enemy, new Vector2 (Random.Range (-3f, 3f), Random.Range (-2f, 2f)), Quaternion.identity);
				InvokeRepeating ("InitBossBot", RATE.bossInitRate, RATE.bossInitRate);
		}
	
		public void randomBoss ()
		{
				oMats = GameObject.FindGameObjectsWithTag ("color");
				bossIds = Random.Range (0, oMats.Length - 1);
//				for (int i=0; i<oMats.Length; i++) {
//						bossIds [i] = i;
//				}
//				for (int i=0; bossIds.Length>i; i++) {
//						int r = 
//						int tmp = bossIds [i] + 0;
//						bossIds [i] = bossIds [r];
//						bossIds [r] = tmp;
//				}
		}
	
		public void InitBossBot ()
		{
				if (GameObject.FindGameObjectsWithTag ("color") != null) {
//						tempNum_id = 0;
						randomBoss ();
						StartCoroutine (initBoss ());
				}
		}
	
		IEnumerator initBoss ()
		{
				if (0 < oMats.Length) {
						Instantiate (prf_thief, oMats [bossIds].transform.position, Quaternion.identity);
						oMats [bossIds].SendMessage ("thiefOn");
						yield return new WaitForSeconds (RATE.bossAttackRate);
						oMats [bossIds].SendMessage ("thiefAttack");
				}
		}
		//		public static Enemy mInstance;
		//		// Use this for initialization
		//		public Enemy ()
		//		{
		//				mInstance = this;
		//		}
		//	
		//		public static Enemy instance {
		//				get {
		//						if (mInstance == null)
		//								new Enemy ();
		//						return mInstance;
		//				}
		//		}
	
	
}
