using UnityEngine;
using System.Collections;

public class scr_button1 : MonoBehaviour
{
		public GameObject mainCam;
		public GameObject btn1;
		public GameObject score;
		public GameObject btn10, btn20, btn30, btn40, btn50;
		SpriteRenderer btn10s, btn20s, btn30s, btn40s, btn50s;
		// Use this for initialization
		void Start ()
		{
//				mainCam.SendMessage ("playAfterLogo");
				btn10s = btn10.GetComponent<SpriteRenderer> ();
				btn20s = btn20.GetComponent<SpriteRenderer> ();
				btn30s = btn30.GetComponent<SpriteRenderer> ();
				btn40s = btn40.GetComponent<SpriteRenderer> ();
				btn50s = btn50.GetComponent<SpriteRenderer> ();
				btnReset ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void btnReset ()
		{
				btn10s.color = hexColor (231, 157, 100, 255);
				btn20s.color = hexColor (231, 157, 100, 255);
				btn30s.color = hexColor (231, 157, 100, 255);
				btn40s.color = hexColor (231, 157, 100, 255);
				btn50s.color = hexColor (231, 157, 100, 255);
		
		}

		void OnTap (TapGesture gesture)
		{
				 
				if (gesture.Selection == btn1) {
						btn1.GetComponent<SpriteRenderer> ().color = Color.yellow;
						Application.LoadLevel (1);
					
				}
				if (gesture.Selection == btn10) {
						btnReset ();
						btn10s.color = hexColor (231, 100, 100, 255);
						score.SendMessage ("showRank", 10);
			
				}
				if (gesture.Selection == btn20) {
						btnReset ();
						btn20s.color = hexColor (231, 100, 100, 255);
			
						score.SendMessage ("showRank", 20);
				}

				if (gesture.Selection == btn30) {
						btnReset ();
						btn30s.color = hexColor (231, 100, 100, 255);
						score.SendMessage ("showRank", 30);
			
				}

				if (gesture.Selection == btn40) {
						btnReset ();
						btn40s.color = hexColor (231, 100, 100, 255);
						score.SendMessage ("showRank", 40);
			
				}

				if (gesture.Selection == btn50) {
						btnReset ();
						btn50s.color = hexColor (231, 100, 100, 255);
						score.SendMessage ("showRank", 50);
			
				}



				 
		}

		public static Vector4 hexColor (float r, float g, float b, float a)
		{
				Vector4 color = new Vector4 (r / 255, g / 255, b / 255, a / 255);
				return color;
		}
		 
}
