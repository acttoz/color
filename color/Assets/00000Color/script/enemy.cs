using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
		public GameObject prf_enemy, prf_thief;
		private GameObject[] oMats;//parent of mats
		private int tempNum_id = 0;
		private int[] bossIds;

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
				InitBossBot ();
		}

		public void randomBoss ()
		{
				oMats = GameObject.FindGameObjectsWithTag ("mat");
				bossIds = new int[oMats.Length];
				for (int i=0; i<oMats.Length; i++) {
						bossIds [i] = i;
				}
				for (int i=0; bossIds.Length>i; i++) {
						int r = Random.Range (0, i);
						int tmp = bossIds [i] + 0;
						bossIds [i] = bossIds [r];
						bossIds [r] = tmp;
				}
				
		}

		public void InitBossBot ()
		{
				tempNum_id = 0;
				randomBoss ();
				InvokeRepeating ("initBoss", RATE.bossInitRate, RATE.bossInitRate);
		}

		private void initBoss ()
		{
				if (tempNum_id < bossIds.Length) {
						Instantiate (prf_thief, oMats [bossIds [tempNum_id]].transform.position, Quaternion.identity);
						tempNum_id++;
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
