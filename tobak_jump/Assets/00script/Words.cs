using UnityEngine;
using System.Collections;

public class Words : MonoBehaviour
{
		public GameObject prf_enemy;
		public GameObject warn_boss;
		public float enemyCreateRate;
		public int enemyNum;
		public static Words mInstance;
		// Use this for initialization
		public Words ()
		{
				mInstance = this;
		}
	
		public static Words instance {
				get {
						if (mInstance == null)
								new Words ();
						return mInstance;
				}
		}

		public void reset ()
		{				//boss reset
				GameObject[] tempBoss;
				tempBoss = GameObject.FindGameObjectsWithTag ("boss");
				for (int i=0; i<tempBoss.Length; i++) {
						Destroy (tempBoss [i]);
				}
				if (GameObject.Find ("prf_warn(Clone)") != null)
						Destroy (GameObject.Find ("prf_warn(Clone)"));
				GameObject[] prevEnemy = GameObject.FindGameObjectsWithTag ("enemy");
				for (int i=0; i<prevEnemy.Length; i++) {
						Destroy (prevEnemy [i]);
				}
		
		}
	
		void Start ()
		{
				StartCoroutine ("enemyCreate");
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		//ENEMY
		IEnumerator enemyCreate ()
		{
				while (true) {
						if (MANAGER.instance.state.Equals ("IDLE"))
								InitEnemy ();
			 
						yield return new WaitForSeconds (enemyCreateRate / 1.2f);
				}
		}
	
		public void InitEnemy ()
		{
		}
	
		public void InitBoss ()
		{
				float tempX = (Random.Range (-4 * 100, 4 * 100)) / 100;
				float tempY = (Random.Range (-4 * 100, 4 * 100)) / 100;
		
		
		
		
				Instantiate (warn_boss, new Vector2 (tempX, tempY), Quaternion.identity);
		}
}
