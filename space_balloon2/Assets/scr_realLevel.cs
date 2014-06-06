using UnityEngine;
using System.Collections;

public class scr_realLevel : MonoBehaviour
{
	public GameObject btn_back, btn_back2, oCart, btn_play, oToast, loading, UI ;
		public GameObject oItem_time, oItem_shield, oItem_smaller, oItem_star, oSelectedPan1, oSelectedPan2;
		public Sprite[] items = new Sprite[5] ;
		int selectedLevel = 5;
		int numGem;
		int iSelectedItem = 0;
		// Use this for initialization
		void Start ()
		{
				PlayerPrefs.SetInt ("ITEM1", 0);
				PlayerPrefs.SetInt ("ITEM2", 0);
				PlayerPrefs.SetInt ("ITEM3", 0);
				PlayerPrefs.SetInt ("ITEM4", 0);
				numGem = PlayerPrefs.GetInt ("NUMGEM");
				 
		}

		void OnTap (TapGesture gesture)
		{
				if (gesture.Selection == btn_back) {
						Application.LoadLevel (1);
						//						Application.LoadLevel (1);
				}
				 
				if (gesture.Selection == btn_play) {
						StartCoroutine ("play", selectedLevel);
						//						Application.LoadLevel (1);
				}
				if (gesture.Selection == oItem_time) {
						itemSelect (gesture.Selection, 0);
						//						Application.LoadLevel (1);
				}
				if (gesture.Selection == oItem_shield) {
						itemSelect (gesture.Selection, 1);
						//						Application.LoadLevel (1);
				}
				if (gesture.Selection == oItem_star) {
						Debug.Log (" ");
						itemSelect (gesture.Selection, 2);
						//						Application.LoadLevel (1);
				}
				if (gesture.Selection == oItem_smaller) {
						itemSelect (gesture.Selection, 3);
						//						Application.LoadLevel (1);
				}
				 
			 

		
		}

		void itemSelect (GameObject obj, int num)
		{

				if (numGem < 5) {
						onToast ();
						return;
				}
				
				switch (iSelectedItem) {
				case 0:
						numGem -= 5;
						PlayerPrefs.SetInt ("NUMGEM", numGem);
						iSelectedItem++;
						obj.SendMessage ("selected");
						PlayerPrefs.SetInt (obj.transform.name, 1);
						oSelectedPan1.GetComponent<SpriteRenderer> ().sprite = items [num];
						switch (num) {
						case 0:
								PlayerPrefs.SetInt ("ITEM1", 1);
								break;
						case 1:
								PlayerPrefs.SetInt ("ITEM2", 1);
								break;
						case 2:
								PlayerPrefs.SetInt ("ITEM3", 1);
								break;
						case 3:
								PlayerPrefs.SetInt ("ITEM4", 1);
								break;

				
						}

						break;
				case 1:
						numGem -= 5;
						PlayerPrefs.SetInt ("NUMGEM", numGem);
						iSelectedItem++;
						obj.SendMessage ("selected");
						PlayerPrefs.SetInt (obj.transform.name, 1);
						oSelectedPan2.GetComponent<SpriteRenderer> ().sprite = items [num];
						switch (num) {
						case 0:
								PlayerPrefs.SetInt ("ITEM1", 1);
								break;
						case 1:
								PlayerPrefs.SetInt ("ITEM2", 1);
								break;
						case 2:
								PlayerPrefs.SetInt ("ITEM3", 1);
								break;
						case 3:
								PlayerPrefs.SetInt ("ITEM4", 1);
								break;
						}
						break;
				case 2:
						 
						break;
				default:
						break;
				}
		}

		IEnumerator loadLV (int num)
		{
//				animation.Play ("anim_menu2");
				Application.LoadLevel (num);
				yield return new WaitForSeconds (0f);
		}

		IEnumerator play (int num)
		{
				PlayerPrefs.SetInt ("LEVEL", 10);
				UI.SetActive (false);
				loading.SetActive (true);
				Application.LoadLevel (5);
				yield return new WaitForSeconds (1.5f);
		}

		 
		 

		void onToast ()
		{
		
				GameObject obj = Instantiate (oToast, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				Destroy (obj, 1.5f);
		}
	
		 
		// Update is called once per frame
		void Update ()
		{
				if (Application.platform == RuntimePlatform.Android) {
						if (Input.GetKey (KeyCode.Escape)) {
								Application.LoadLevel (1);
								return;
						}
				}
				if (numGem < 5) {
						oItem_star.SendMessage ("selected");
						oItem_time.SendMessage ("selected");
						oItem_smaller.SendMessage ("selected");
						oItem_shield.SendMessage ("selected");
				}
	
		}
}
