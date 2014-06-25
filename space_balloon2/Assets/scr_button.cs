﻿using UnityEngine;
using System.Collections;

public class scr_button : MonoBehaviour
{
		public GameObject btn1, btn2, btn3, btn4, loading, UI, realBtn;
		// Use this for initialization
		void Start ()
		{
//		PlayerPrefs.SetInt ("NUMGEM", 1000000);
				if (PlayerPrefs.GetInt ("9", 0) == 1) {
						btn1.GetComponent<CapsuleCollider> ().enabled = false;
						realBtn.GetComponent<CapsuleCollider> ().enabled = true;
						animation.Play ();
				}
//				DontDestroyOnLoad (GameObject.Find ("back"));
				

//				btn1.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
//				btn2.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Application.platform == RuntimePlatform.Android) {
						if (Input.GetKey (KeyCode.Escape)) {
				
								Application.Quit ();
				
								return;
				
						}
				}
		}
	
		void OnTap (TapGesture gesture)
		{
				 
				if (btn1 != null && gesture.Selection == btn1) {
						btn1.GetComponent<SpriteRenderer> ().color = Color.yellow;
						StartCoroutine ("loadLV", 2);
					
				}
				if (btn2 != null && gesture.Selection == btn2) {
						btn2.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
						StartCoroutine ("loadLV", 3);
//						Application.LoadLevel (1);
				}
				if (btn3 != null && gesture.Selection == btn3) {
						btn2.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
						StartCoroutine ("loadLV", 4);
						//						Application.LoadLevel (1);
				}
				if (btn4 != null && gesture.Selection == btn4) {
						btn4.gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
						StartCoroutine ("loadLV", 7);
						//						Application.LoadLevel (1);
				}
				if (realBtn != null && gesture.Selection == realBtn) {
						btn1.GetComponent<SpriteRenderer> ().color = Color.yellow;
						StartCoroutine ("loadLV", 6);
			
				}

				 

		}

		void playAudio ()
		{
		}

		IEnumerator loadLV (int num)
		{
				yield return new WaitForSeconds (0.01f);
				UI.SetActive (false);
				loading.SetActive (true);
//				animation.Play ("anim_menu2");
				Application.LoadLevel (num);
		}
		 
}
