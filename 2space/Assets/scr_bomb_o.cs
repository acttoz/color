using UnityEngine;
using System.Collections;

public class scr_bomb_o : MonoBehaviour
{
		Vector3 mDirection;
		public float speed;
		bool bossCollide = false;
		//     2
		//  1     3
		//     4 

		// Use this for initialization
		void Start ()
		{
		mDirection = new Vector3 (0, 0, 0);			 
	
		}
	
		// Update is called once per frame
		void Update ()
		{

				if (bossCollide)
						transform.position -= mDirection;
//				if (bossCollide) {
//				}

		}

		void boss (Vector3 bossPos)
		{
				bossCollide = true;
				GetComponent<SphereCollider> ().enabled = false;
		transform.parent.animation.Stop ();
//		transform.parent.gameObject.SendMessage ("onTimer", 2f);
				mDirection = bossPos - transform.position;
				mDirection = mDirection.normalized / 5f;
				Debug.Log (mDirection);
		}
}
