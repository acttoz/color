using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
		public GameObject prf_enemy;
		public GameObject warn_boss;
		public float enemyCreateRate;
		public int enemyNum;
		public static Enemy instance;
		// Use this for initialization
		void Start ()
		{
				instance = this;
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
						if (MANAGER.STATE.Equals ("IDLE"))
								InitEnemy ();
			 
						yield return new WaitForSeconds (enemyCreateRate / 1.2f);
				}
		}
	
		public void InitEnemy ()
		{
				float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100;
				float tempY = (Random.Range (MANAGER.mDown * 100, MANAGER.mUp * 100)) / 100;
		
				enemyNum++;
				if (Level.instance.getLevel () == 5 && enemyNum > 7)
						StopCoroutine ("enemyCreate");
				if (Level.instance.getLevel () == 4 && enemyNum > 5)
						StopCoroutine ("enemyCreate");
				if (Level.instance.getLevel () == 3 && enemyNum > 5)
						StopCoroutine ("enemyCreate");
				if (Level.instance.getLevel () == 2 && enemyNum > 0)
						StopCoroutine ("enemyCreate");
				if (Level.instance.getLevel () == 1 && enemyNum > 0)
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
