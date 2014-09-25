using UnityEngine;
using System.Collections;

public class color_mat : MonoBehaviour
{
		private bool isPlus = false;
		private bool isMinus = false;
//		private Transform whiteMAT_position;
//		private GameObject whiteMat;
//		private SpriteRenderer whiteMatSprite;
//		private Animation[] whiteMAT_anim;
		private SpriteRenderer matSprite;
		private EasySprite_HSV saturation;
		public GameObject effectSuccess;
		public GameObject effectColoring;
		public AudioClip sSuccess;

		void Start ()
		{
//				whiteMat = gameObject.transform.parent.gameObject;
//				whiteMatSprite = whiteMat.GetComponent<SpriteRenderer> ();
				matSprite = GetComponent<SpriteRenderer> ();
				saturation = GetComponent<EasySprite_HSV> ();
		}
		// Use this for initialization
		void reset ()
		{
		Instantiate (this.gameObject, this.transform.position, Quaternion.identity);
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

		void plusColor ()
		{
//				Debug.Log (GetComponent<SpriteRenderer> ().color.a + "");
				if (!isPlus || !STATE._STATE.Equals ("gIDLE")) {
						endPlus ();
						return;
				}
				if (saturation._Saturation < 1) {
						if (!audio.isPlaying)
								audio.Play ();
//						if (!whiteMat.animation.isPlaying)
//								whiteMat.animation.Play ();
						if (!animation.isPlaying)
								animation.Play ();
//						cpnt_whiteMAT_sprite [1].color = new Color (1, 0, 0);
						saturation._Saturation += RATE.colorPlusRate / 10000f;
						saturation._ValueBrightness += RATE.colorPlusRate / 20000f;
						
				} else {

						//////SUCCESS
						///
						Instantiate (effectSuccess, transform.position, Quaternion.identity);
						endPlus ();
						audio.PlayOneShot (sSuccess);
						CancelInvoke ("minusColor");
						CancelInvoke ("plusColor");
						//success
						STATE.mats++;
						Debug.Log ("mats" + STATE.mats + " " + STATE.matsAll);
						if (STATE.mats == STATE.matsAll)
								STATE._STATE = "gSUCCESS";
			
				}
		}

		void endPlus ()
		{
				if (audio.isPlaying)
						audio.Stop ();
//				if (whiteMat.animation.isPlaying)
//						whiteMat.animation.Stop ();
				if (animation.isPlaying)
						animation.Stop ();
		}

		void plusing ()
		{
				Instantiate (effectColoring, transform.position, Quaternion.identity);
		
		}

		void minusColor ()
		{
				if (isMinus && saturation._Saturation > 0) {
						endPlus ();
						saturation._Saturation -= RATE.colorPlusRate / 10000f;
						saturation._ValueBrightness -= RATE.colorPlusRate / 20000f;
			
//						whiteMatSprite.color = new Color (1, 1, 1);
				}
		}

		void onColor ()
		{
				isPlus = true;
				isMinus = false;
		}

		void offColor ()
		{
				isMinus = true;
				isPlus = false;
		}
//
//		void OnTriggerStay (Collider myTrigger)
//		{
//		
//				if (myTrigger.transform.tag == "touch") {
//						isPlus = true;
//						isMinus = false;
//				
//				} else if (myTrigger.transform.tag == "Respawn") {
//						isMinus = true;
//						isPlus = false;
//				}
//			
//				//						if (myTrigger.transform.tag == "bomb_o" && color.Equals ("o")) {
//				//				
//				//								Instantiate (pop, transform.position, Quaternion.identity);
//				//								Destroy (this.gameObject);
//				//				
//				//				
//				//						}
//				//						if (myTrigger.transform.tag == "bomb_p" && color.Equals ("p")) {
//				//				
//				//								Instantiate (pop, transform.position, Quaternion.identity);
//				//								Destroy (this.gameObject);
//				//				
//				//				
//				//					
//		}

//		void onTriggerExit (Collider myTrigger)
//		{
//				Debug.Log ("Outminus1");
//				if (myTrigger.transform.tag == "touch") {
//						Debug.Log ("Outminus2");
//						isMinus = true;
//						isPlus = false;
//			
//				}
//		}

}
