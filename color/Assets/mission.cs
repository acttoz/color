using UnityEngine;
using System.Collections;

public class mission : MonoBehaviour
{
		public Sprite[] missions;
		// Use this for initialization
		void Start ()
		{
				GetComponent<SpriteRenderer> ().sprite = missions [PlayerPrefs.GetInt ("LEVEL", 1) - 1];
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
