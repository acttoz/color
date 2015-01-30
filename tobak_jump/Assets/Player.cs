using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public int state = 0;//0:start 1:left 2:right;
		public static Player mInstance;
		Sprite mSprite;
		public Sprite sThink;
		public Sprite sJumpLeft;
		public Sprite sReadyLeft;
		public Sprite sReadyRight;
		public Sprite sJumpRight;
		public Sprite sJumpStraight;
		public Sprite sReadyStraight;
	
		public Player ()
		{
				mInstance = this;
		}
	
		public static Player instance {
				get {
						if (mInstance == null)
								new Player ();
						return mInstance;
				}
		}

		public void touchLeft ()
		{
				if (state < 2) {
						//left
						StartCoroutine ("jumpStraight");
				} else {
						//right
						StartCoroutine ("jumpLeft");
				}
				state = 1;

		}

		public void touchRight ()
		{
				if (state < 2) {
						//left
						StartCoroutine ("jumpRight");
				} else {
						//right
						StartCoroutine ("jumpStraight");
				}
				state = 2;
		}

		IEnumerator jumpLeft ()
		{
				mSprite = sReadyLeft;
				yield return new WaitForSeconds (0.2f);
				mSprite = sJumpLeft;
				yield return new WaitForSeconds (1f);
				mSprite = sReadyLeft;
				yield return new WaitForSeconds (0.2f);
				mSprite = sThink;

		}

		IEnumerator jumpRight ()
		{
				mSprite = sReadyLeft;
				yield return new WaitForSeconds (0.2f);
				mSprite = sJumpLeft;
				yield return new WaitForSeconds (1f);
				mSprite = sReadyLeft;
				yield return new WaitForSeconds (0.2f);
		}

		IEnumerator jumpStraight ()
		{
				mSprite = sReadyLeft;
				yield return new WaitForSeconds (0.2f);
				mSprite = sJumpLeft;
				yield return new WaitForSeconds (1f);
				mSprite = sReadyLeft;
				yield return new WaitForSeconds (0.2f);
		}




		// Use this for initialization
		void Start ()
		{
				mSprite = GetComponent<SpriteRenderer> ().sprite;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}


}
