using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
		public GameObject prf_enemy;
		public GameObject warn_boss;
		public float enemyCreateRate;
		public int enemyNum;
		public static Enemy mInstance;
		// Use this for initialization
		public Enemy ()
		{
				mInstance = this;
		}
	
		public static Enemy instance {
				get {
						if (mInstance == null)
								new Enemy ();
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
				float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100;
				float tempY = (Random.Range (MANAGER.mDown * 100, MANAGER.mUp * 100)) / 100;
		
				enemyNum++;
				if (Level.instance.level == 5 && enemyNum > 7)
						StopCoroutine ("enemyCreate");
				if (Level.instance.level == 4 && enemyNum > 5)
						StopCoroutine ("enemyCreate");
				if (Level.instance.level == 3 && enemyNum > 5)
						StopCoroutine ("enemyCreate");
				if (Level.instance.level == 2 && enemyNum > 0)
						StopCoroutine ("enemyCreate");
				if (Level.instance.level == 1 && enemyNum > 0)
						StopCoroutine ("enemyCreate");
		
		
				Instantiate (prf_enemy, new Vector2 (tempX, tempY), Quaternion.identity);
		}
	
		public void InitBoss ()
		{
				float tempX = (Random.Range (-4 * 100, 4 * 100)) / 100;
				float tempY = (Random.Range (-4 * 100, 4 * 100)) / 100;
		
		
		
		
				Instantiate (warn_boss, new Vector2 (tempX, tempY), Quaternion.identity);
		}
}
