using UnityEngine;
using System.Collections;

public class scr_destroyTimer : MonoBehaviour
{
		public float destroyTime;
		// Use this for initialization
		void Start ()
		{
				StartCoroutine ("timer");
//				GameObject.Find ("img-ballon").GetComponent<SpriteRenderer> ().sortingOrder = 1;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		IEnumerator timer ()
		{
				yield return new WaitForSeconds (destroyTime);
//				GameObject temp = GameObject.Find ("img-ballon");
//				if (temp != null) {
//						temp.GetComponent<SpriteRenderer> ().sortingOrder = -1;
//						if (scr_manager.superLevel == 5)
//								temp.audio.Play ();
//
//				}

				Destroy (this.gameObject);
		
		}

}
