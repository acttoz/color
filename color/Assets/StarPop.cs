using UnityEngine;
using System.Collections;

public class StarPop : MonoBehaviour
{
		GameObject oScore;
		public GameObject oEffectScore;
		bool getScore = false;
		private float speed = 50;
		public int starNum;
		bool collect = false;
		// Use this for initialization
		void Start ()
		{
				oScore = GameObject.Find ("score");	
				if (STATE.stars > starNum)
						animation.Play ();

		}

		void score ()
		{
				collect = true;
		}
		// Update is called once per frame
		void Update ()
		{
				if (collect) {
						float step = speed * Time.deltaTime;
				
						transform.position = Vector3.MoveTowards (transform.position, oScore.transform.position, step);
						if (transform.position == oScore.transform.position) {
								STATE.SCORE += 1;
								oScore.SendMessage ("Collect");
								Destroy (this.gameObject);
						}
				}
		}


}
