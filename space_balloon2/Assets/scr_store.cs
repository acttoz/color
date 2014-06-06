using UnityEngine;
using System.Collections;

public class scr_store : MonoBehaviour
{
		public Sprite unlockedIcon, unlockedPanel;
		public int pay;
		public bool equipped = false;
		string level, preLevel;
		public GameObject oEquip;
		SpriteRenderer[] componentArray;
		// Use this for initialization
		void Start ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				preLevel = "L" + (int.Parse (gameObject.transform.name) - 1);
				level = "L" + gameObject.transform.name;
				//				GetComponentInChildren<tk2dTextMesh> ().text = level;
		
				PlayerPrefs.SetInt ("L5", 1);
				if (PlayerPrefs.GetInt (level, 0) == 1)
						equipped = true;
				//				PlayerPrefs.SetInt ("9", 1);
				componentArray = GetComponentsInChildren<SpriteRenderer> ();
				if (PlayerPrefs.GetInt ("NUMGEM") > pay && !equipped && PlayerPrefs.GetInt (preLevel, 0) == 1) {
						//payable
						GetComponent<BoxCollider> ().enabled = true;
						GetComponentInChildren<tk2dTextMesh> ().color = Color.white;
						componentArray [0].sprite = unlockedIcon;
						componentArray [1].sprite = unlockedPanel;
						componentArray [2].color = new Color (1, 1, 1, 1);
				}
				if (equipped) {
						//Cleared/////////////////////////////////////////
						//						GetComponent<BoxCollider> ().enabled = true;
						//						SpriteRenderer[] componentArray = GetComponentsInChildren<SpriteRenderer> ();
						equip (0);
			
				}
	
		}

		void equip (int num)
		{
				componentArray [0].sprite = unlockedIcon;
				componentArray [2].color = new Color (1, 1, 1, 1);
				oEquip.SetActive (true);
				PlayerPrefs.SetInt (level, 1);
				if (num == 1) {
						PlayerPrefs.SetInt ("NUMGEM", PlayerPrefs.GetInt ("NUMGEM") - pay);
						GetComponent<BoxCollider> ().enabled = false;
				}
		}
}
