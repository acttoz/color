using UnityEngine;
using System.Collections;

public class buzzes : MonoBehaviour
{
		GameObject oScore;
		public GameObject oEffectScore;
		bool getScore = false;
		private float speed = 20;
		// Use this for initialization
		void Start ()
		{
				oScore = GameObject.Find ("score");	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (getScore) {
						float step = speed * Time.deltaTime;

						transform.position = Vector3.MoveTowards (transform.position, oScore.transform.position, step);
						if (transform.position == oScore.transform.position) {
								STATE.buzz--;
								STATE.SCORE += (STATE.buzzScore += 1);
								oScore.SendMessage ("Collect");
								if (STATE.buzzScore == 0)
										STATE.buzzScore = 0;
								Destroy (this.gameObject);
						}
				}
	
	
		}

		void score ()
		{
				getScore = true;
		}
}

