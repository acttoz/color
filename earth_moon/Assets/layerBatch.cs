using UnityEngine;
using System.Collections;

public class layerBatch : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
				SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer> ();
				for (int i=0; i<sprites.Length; i++) {
			sprites [i].sortingLayerName="ready";
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
