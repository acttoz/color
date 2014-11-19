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

				StopCoroutine ("initBoss");
				GameObject[] oBosses = GameObject.FindGameObjectsWithTag ("boss");
				for (int i=0; i<oBosses.Length; i++)
						Destroy (oBosses [i]);

				for (int i=0; i<5; i++)
						Instantiate (prf_enemy, new Vector2 (Random.Range (-3f, 3f), Random.Range (-2f, 2f)), Quaternion.identity);
				InvokeRepeating ("InitBossBot", RATE.bossInitRate, RATE.bossInitRate);
		}
	
		public void randomBoss ()
		{
				oMats = GameObject.FindGameObjectsWithTag ("color");
				bossIds = Random.Range (0, (oMats.Length - 1));
		}
	
		public void InitBossBot ()
		{
				if (GameObject.FindGameObjectsWithTag ("color") != null) {
						//						tempNum_id = 0;
						StopCoroutine ("initBoss");
						randomBoss ();
						StartCoroutine ("initBoss");
				}
		}

		IEnumerator initBoss ()
		{
				if (0 < oMats.Length) {
						Instantiate (prf_thief, oMats [bossIds].transform.position, Quaternion.identity);
						oMats [bossIds].SendMessage ("thiefOn");
//						Debug.Log ("<<_____" + oMats [bossIds]);
//						Debug.Log ("id" + bossIds);
						yield return new WaitForSeconds (RATE.bossAttackRate);
//						Debug.Log ("id" + bossIds);
						oMats [bossIds].SendMessage ("thiefAttack");
//						Debug.Log (oMats [bossIds] + "_____>>");
//						Debug.Log ("bossId" + bossIds);
				}
		}
	
}
