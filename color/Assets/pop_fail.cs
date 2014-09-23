using UnityEngine;
using System.Collections;

public class pop_fail : MonoBehaviour
{
		public GameObject effect_smoke;
		// Use this for initialization
		void Start ()
		{
				StartCoroutine (waitSeconds (0.1f));
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		IEnumerator waitSeconds (float seconds)
		{

				yield return new WaitForSeconds (seconds);
				Instantiate (effect_smoke, new Vector2 (0, 0), Quaternion.identity);
				//Instantiate here
		}

}
