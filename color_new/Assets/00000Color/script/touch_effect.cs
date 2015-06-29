using UnityEngine;
using System.Collections;

public class touch_effect : MonoBehaviour
{
		float cooldown = 0;
		GameObject effect;
		public GameObject prfEffect;
		// Use this for initialization
		void Start ()
		{
		cooldown = 0.15f;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (cooldown > 0) {
						cooldown -= Time.deltaTime;
				}


				if (cooldown <= 0) {
				
//						Instantiate (prfEffect, this.gameObject.transform.position, Quaternion.identity);
		
						//For showcasing reasons: some effects don't look good if they appear half underground. I move these effects a bit upward.

		
						cooldown = 0.15f;
				}
		}
}
