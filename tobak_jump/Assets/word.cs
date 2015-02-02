using UnityEngine;
using System.Collections;

public class word : MonoBehaviour
{
		public bool isLeft;
		// Use this for initialization
		void Start ()
		{
				tk2dTextMesh cText = GetComponent<tk2dTextMesh> ();

				if (isLeft)
						cText.text = Words.instance.word4 [0];
				else
						cText.text = Words.instance.word4 [1];
		
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
