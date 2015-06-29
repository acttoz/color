using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
		tk2dTextMesh text;
		public GameObject oEffectScore;
		// Use this for initialization
		void Start ()
		{
				text = GetComponent<tk2dTextMesh> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		void Collect ()
		{
				animation.Play ();
				text.text = STATE.SCORE+"";
				Instantiate (oEffectScore, this.transform.position, Quaternion.identity);
		
		}
}
