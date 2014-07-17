using UnityEngine;
using System.Collections;

public class color_mat : MonoBehaviour
{
		bool isPlus = false;
		bool isMinus = false;
		// Use this for initialization
		void Start ()
		{

				GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
				InvokeRepeating ("plusColor", 0, 0.01f);
				InvokeRepeating ("minusColor", 0, 0.01f);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (STATE._STATE.Equals ("WAIT"))
						GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
		}

		void plusColor ()
		{
//				Debug.Log (GetComponent<SpriteRenderer> ().color.a + "");
				if (isPlus && GetComponent<SpriteRenderer> ().color.a < 1) {
			
						GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, RATE.colorPlusRate / 1000f);
				}

				if (GetComponent<SpriteRenderer> ().color.a >= 1) {
						CancelInvoke ("minusColor");
				}
		}

		void minusColor ()
		{
				if (isMinus && GetComponent<SpriteRenderer> ().color.a > 0 && GetComponent<SpriteRenderer> ().color.a != 1)
						GetComponent<SpriteRenderer> ().color -= new Color (0, 0, 0, RATE.colorMinusRate / 1000f);
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
