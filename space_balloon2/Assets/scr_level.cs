using UnityEngine;
using System.Collections;

public class scr_level : MonoBehaviour
{
		public Sprite unlockedIcon, unlockedPanel;
		string level, preLevel;
		// Use this for initialization
		void Start ()
		{
				level = gameObject.transform.name;
				preLevel = int.Parse (level) - 1+"";
				GetComponentInChildren<tk2dTextMesh> ().text = level;

				PlayerPrefs.SetInt ("0", 1);
				PlayerPrefs.SetInt ("9", 1);

				if (PlayerPrefs.GetInt (preLevel, 0) == 1) {
						//Cleared/////////////////////////////////////////
						GetComponent<BoxCollider> ().enabled = true;
						SpriteRenderer[] componentArray = GetComponentsInChildren<SpriteRenderer> ();
						GetComponentInChildren<tk2dTextMesh> ().color = Color.white;
						componentArray [0].sprite = unlockedIcon;
						componentArray [1].sprite = unlockedPanel;

				}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
