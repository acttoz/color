﻿using UnityEngine;
using System.Collections;

public class color_mat : MonoBehaviour
{
		private bool isPlus = false;
		private bool isMinus = false;
		private bool isBlack = false;
		public string state = "normal";//thief
//		private Transform whiteMAT_position;
//		private GameObject whiteMat;
//		private SpriteRenderer whiteMatSprite;
//		private Animation[] whiteMAT_anim;
//		private SpriteRenderer matSprite;
		private EasySprite_HSV saturation;
//		private EasySprite_Pattern pattern;
		public GameObject effectSuccess;
		public GameObject effectSpark;
		public GameObject effectSuccessBack;
		public GameObject oClone;
		public AudioClip sSuccess;
		GameObject manager;
//	public EasySprite_HSV cHSV;
//		public Texture2D rainbow;
		public int bossID;

		void Start ()
		{
				manager = GameObject.Find ("0000MANAGER");
//				whiteMat = gameObject.transform.parent.gameObject;
//				whiteMatSprite = whiteMat.GetComponent<SpriteRenderer> ();
//				matSprite = GetComponent<SpriteRenderer> ();
//				pattern = GetComponent<EasySprite_Pattern> ();
				saturation = GetComponent ("EasySprite_HSV") as EasySprite_HSV;
				saturation._HueShift = 0;
			 
		}
		// Use this for initialization
		void reset ()
		{
//				saturation.enabled = false;
				thiefOff ();
//				if (oClone != null)
//						Destroy (oClone);
				transform.tag = "mat";
		
				state = "normal";
				CancelInvoke ("thiefMinusColor");
				CancelInvoke ("minusColor");
				CancelInvoke ("plusColor");
//				whiteMAT_position = GetComponentInChildren<Transform> ();
//		whiteMAT_position.localPosition = new Vector3 (0, 0, 0);
//				whiteMAT_anim = GetComponentsInChildren<Animation> ();
//				whiteMatSprite.sortingOrder = matSprite.sortingOrder - 1;
//				whiteMatSprite.sortingLayerName = matSprite.sortingLayerName;
//				GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
				InvokeRepeating ("plusColor", 0, 0.01f);
				InvokeRepeating ("minusColor", 0, 0.01f);
				offColor ();
		}

		 
	
		// Update is called once per frame
		void Update ()
		{
		}

		void thiefOn ()
		{
				state = "thief";
				transform.tag = "mat";
				StartCoroutine ("thiefBlink", 0.1f);
		}

		void thiefOff ()
		{
				state = "normal";
				StopCoroutine ("thiefBlink");
				saturation._ValueBrightness = 1;
				transform.tag = "color";
		}

		void thiefAttack ()
		{
				if (state.Equals ("thief")) {
						thiefOff ();
						Debug.Log ("thiefattack");
						saturation._Saturation = 5;
						animation.Stop ();
						STATE.mats--;
						InvokeRepeating ("thiefMinusColor", 0, 0.01f);
				}
		}

		IEnumerator thiefBlink (float term)
		{
				while (true) {
						saturation._ValueBrightness = 0;
						yield return new WaitForSeconds (term);
						saturation._ValueBrightness = 1;
						yield return new WaitForSeconds (term);
				}

		}

		void plusColor ()
		{
//				Debug.Log (GetComponent<SpriteRenderer> ().color.a + "");
				if (!isPlus || !STATE._STATE.Equals ("gIDLE") || !state.Equals ("normal")) {
						endPlus ();
						return;
				}
				if (saturation._Saturation < 3) {
						startPlus ();

//						cpnt_whiteMAT_sprite [1].color = new Color (1, 0, 0);
//						pattern._Alpha = 1 - saturation._Saturation;
						saturation._Saturation += RATE.colorPlusRate / 3000f;
//						pattern._Alpha -= RATE.colorPlusRate / 10000f;
						saturation._ValueBrightness += RATE.colorPlusRate / 6000f;
						
				} else {

						//////SUCCESS
						///
						manager.SendMessage ("AddPoints", this.transform);

						state = "color";
						transform.tag = "color";
						saturation._Saturation = 1;
						//						pattern._Alpha -= RATE.colorPlusRate / 10000f;
						saturation._ValueBrightness = 1;
						Instantiate (effectSuccess, transform.position, Quaternion.identity);
						Instantiate (effectSpark, transform.position, Quaternion.identity);
						Instantiate (effectSuccessBack, new Vector2 (0, 0), Quaternion.identity);
						endPlus ();
						audio.PlayOneShot (sSuccess);
						CancelInvoke ("minusColor");
						CancelInvoke ("plusColor");
						animation.Play ("bloom");
						//success
						STATE.mats++;
//						Debug.Log ("mats" + STATE.mats + " " + STATE.matsAll);
						if (STATE.mats == STATE.matsAll)
								STATE._STATE = "gSUCCESS";
			
				}
		}

		void successAnim ()
		{
				animation.Play ("success");
		
		}

		void endPlus ()
		{
				if (audio.isPlaying)
						audio.Stop ();
//				if (whiteMat.animation.isPlaying)
//						whiteMat.animation.Stop ();
				if (animation.isPlaying) {
						animation.Stop ();
				}
//				pattern.enabled = false;
//				oClone.SetActive (false);
//				saturation.enabled = true;
		}

		void startPlus ()
		{
				if (!audio.isPlaying)
						audio.Play ();
				//						if (!whiteMat.animation.isPlaying)
				//								whiteMat.animation.Play ();
				if (!animation.isPlaying)
						animation.Play ();
//				oClone.SetActive (true);

//				pattern.enabled = true;
//				saturation.enabled = false;
				
		}

		void minusColor ()
		{
				if (isMinus && saturation._Saturation > 0) {
						endPlus ();
						saturation._Saturation -= RATE.colorMinusRate / 3000f;
						saturation._ValueBrightness -= RATE.colorMinusRate / 6000f;
			
						//						whiteMatSprite.color = new Color (1, 1, 1);
				}
		}

		void thiefMinusColor ()
		{
				if (saturation._Saturation > 0) {
						saturation._Saturation -= RATE.colorMinusRate / 1000f;
				} else {
						
						reset ();
				}				
		}

		void onColor ()
		{
				isPlus = true;
				isMinus = false;
				Debug.Log ("oncolor");
		}

		void offColor ()
		{
				isMinus = true;
				isPlus = false;
		}
 
}