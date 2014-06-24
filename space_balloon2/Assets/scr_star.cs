using UnityEngine;
using System.Collections;

public class scr_star : MonoBehaviour
{
		public Sprite original, temp;
//		SpriteRenderer[] sr;
		float[] levels = new float[] {
				0,
				0.01f,
				0.03f,
				0.1f,
				0.2f,
				0.4f,
				0.6f,
				1,
				1,
				1,
				1,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				-0.03f
		};
//		public Sprite[] stars;
//		public GameObject[] stars;
		// Use this for initialization
		void Start ()
		{
				original = GetComponentInChildren<SpriteRenderer> ().sprite;
//				sr = GetComponentsInChildren<SpriteRenderer> ();
//				int temp = Random.Range (0, stars.Length);
//				GetComponent<SpriteRenderer> ().sprite = stars [temp];
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position -= new Vector3 (0, levels [scr_manager.superLevel], 0);
				if (transform.position.y < -10 || transform.position.y > 10)
						Destroy (this.gameObject);
//				if (scr_manager.superLevel > 5) {
//						for (int i=0; i<sr.Length; i++) {
//								sr [i].sprite = temp;
//						}
//				} else {
//						for (int i=0; i<sr.Length; i++) {
//								sr [i].sprite = original;
//						}
//				}

		}

//		void changeSpeed ()
//		{
//				level = levels [scr_manager.superLevel];
//		}


}
