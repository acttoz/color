using UnityEngine;
using System.Collections;

public class scr_selectLevel : MonoBehaviour
{
		GameObject[] oLevels = new GameObject[9];
		public GameObject btn_back, btn_back2, oCart, btn_play, oToast, loading, UI ;
		public GameObject oItem_time, oItem_shield, oItem_smaller, oItem_star, oSelectedPan1, oSelectedPan2;
		public Sprite[] items = new Sprite[5] ;
		public bool isQuest;
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
				for (int i=0; i<9; i++) {
						oLevels [i] = GameObject.Find ("" + (i + 1));
				}
		}

		void OnTap (TapGesture gesture)
		{
				if (gesture.Selection == btn_back) {
						Application.LoadLevel (1);
						//						Application.LoadLevel (1);
				}
				if (gesture.Selection == btn_back2) {
						offCart ();
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
				if (!isQuest) {
						if (gesture.Selection == oLevels [0]) {
								selectedLevel = 5 + 0;
								cart ();
						}
						if (gesture.Selection == oLevels [1]) {
								selectedLevel = 5 + 1;
								cart ();
						}
						if (gesture.Selection == oLevels [2]) {
								selectedLevel = 5 + 2;
								cart ();
						}
						if (gesture.Selection == oLevels [3]) {
								selectedLevel = 5 + 3;
								cart ();
						}
						if (gesture.Selection == oLevels [4]) {
								selectedLevel = 5 + 4;
								cart ();
						}
						if (gesture.Selection == oLevels [5]) {
								selectedLevel = 5 + 5;
								cart ();
						}
						if (gesture.Selection == oLevels [6]) {
								selectedLevel = 5 + 6;
								cart ();
						}
						if (gesture.Selection == oLevels [7]) {
								selectedLevel = 5 + 7;
								cart ();
						}
						if (gesture.Selection == oLevels [8]) {
								selectedLevel = 5 + 8;
								cart ();
						}
				} else {
						if (gesture.Selection == oLevels [0]) {
								selectedLevel = 5 + 10;
								cart ();
						}
						if (gesture.Selection == oLevels [1]) {
								selectedLevel = 5 + 11;
								cart ();
						}
						if (gesture.Selection == oLevels [2]) {
								selectedLevel = 5 + 12;
								cart ();
						}
						if (gesture.Selection == oLevels [3]) {
								selectedLevel = 5 + 13;
								cart ();
						}
						if (gesture.Selection == oLevels [4]) {
								selectedLevel = 5 + 14;
								cart ();
						}
						if (gesture.Selection == oLevels [5]) {
								selectedLevel = 5 + 15;
								cart ();
						}
						Debug.Log ("" + selectedLevel);
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
				PlayerPrefs.SetInt ("LEVEL", num - 4);
				UI.SetActive (false);
				loading.SetActive (true);
				Application.LoadLevel (5);
				yield return new WaitForSeconds (1.5f);
		}

		void cart ()
		{

				oCart.animation ["anim_menu_cart_on2"].speed = 1.0f;
				animation ["anim_menu_cart_out"].speed = 1.0f;

				oCart.animation ["anim_menu_cart_on2"].time = 0;
				animation ["anim_menu_cart_out"].time = 0;
				
				oCart.animation.Play ();
				animation.Play ("anim_menu_cart_out");
		}

		void offCart ()
		{

				oCart.animation ["anim_menu_cart_on2"].speed = -1.0f;
				animation ["anim_menu_cart_out"].speed = -1.0f;

				oCart.animation ["anim_menu_cart_on2"].time = oCart.animation ["anim_menu_cart_on2"].length;
				animation ["anim_menu_cart_out"].time = animation ["anim_menu_cart_out"].length;


				oCart.animation.Play ();
				animation.Play ("anim_menu_cart_out");
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
