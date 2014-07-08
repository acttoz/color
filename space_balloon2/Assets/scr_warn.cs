using UnityEngine;
using System.Collections;

public class scr_warn : MonoBehaviour
{
		public GameObject prf_enemy, prf_boss;
		public int isBoss;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void create ()
		{
				if (isBoss == 1) {
						GameObject obj = Instantiate (prf_boss, transform.position, Quaternion.identity) as GameObject;
						obj.SendMessage ("laugh");
				} else {
						Instantiate (prf_enemy, transform.position, Quaternion.identity);
				}
				Destroy (this.gameObject);
		}
}
