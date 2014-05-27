using UnityEngine;
using System.Collections;

public class success : MonoBehaviour
{
		public Sprite failed;
		// Use this for initialization
		void Start ()
		{
				int level = PlayerPrefs.GetInt ("LEVEL", 0);
				int success = PlayerPrefs.GetInt ("" + level, 0);
		Debug.Log(level+ " " +success);
				if (success != 1)
						GetComponent<SpriteRenderer> ().sprite = failed;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
