using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public int state = 0;//0:start 1:left 2:right;
		public static Player mInstance;
		SpriteRenderer mSprite;
		public Sprite sThink;
		public Sprite sFall;
		public Sprite[] sLeft;
		public Sprite[] sRight;
		public Sprite[] sStraight;
		protected Animator animator;
		public bool wasSuccess = true;

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
						jumpStraight ();
				} else {
						//right
						jumpLeft ();
				}
				state = 1;

		}

		public void touchRight ()
		{
				if (state < 2) {
						//left
						jumpRight ();
				} else {
						//right
						jumpStraight ();
				}
				state = 2;
		}

		void jumpLeft ()
		{
				animator.SetBool ("jumpLeft", true);
		
		}

		void jumpRight ()
		{
				if (state == 0)
						animator.SetBool ("Right", true);
				else
						animator.SetBool ("jumpRight", true);
		}

		void jumpStraight ()
		{
				if (state == 0)
						animator.SetBool ("Left", true);
				else if (state == 1)
						animator.SetBool ("jumpStraight", true);
				else 
						animator.SetBool ("jumpStraight2", true);
			
		}

		void rightPose (int num)
		{
				mSprite.sprite = sRight [num];
		}

		void leftPose (int num)
		{
				mSprite.sprite = sLeft [num];
		}

		void straightPose (int num)
		{
				mSprite.sprite = sStraight [num];
		}

		void thinkPose ()
		{
				if (wasSuccess) {
						mSprite.sprite = sThink;
						MANAGER.instance.state = "SUCCESS";
				} else {
						mSprite.sprite = sFall;
						MANAGER.instance.state = "FAIL";
						transform.parent.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1;
				}
		
		}

		void setPosition (int num)
		{
				if (num == 1)
						transform.position = new Vector2 (2f, -1.096691f);
				else
						transform.position = new Vector2 (-2f, -1.096691f);
		}



		// Use this for initialization
		void Start ()
		{
				mSprite = GetComponent<SpriteRenderer> ();
				animator = GetComponent<Animator> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		}


}
