using UnityEngine;
using System.Collections;

public class scr_rank_manager : MonoBehaviour
{
		public GameObject mainCam, prf_inputName, oInputName, prf_warning, oWarning, prf_error, oError;
		public GameObject btn1, btn2;
		public GameObject score;
		public GameObject btn10, btn20, btn30, btn40, btn50;
		SpriteRenderer btn10s, btn20s, btn30s, btn40s, btn50s;
		private int posCursor = 1;
		public GameObject[] oCursors;
		public string[] myName = {"","","","","",""};
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
				btn10s.color = hexColor (231, 100, 100, 255);
				if (PlayerPrefs.GetString ("MYNAME").Equals ("")) {
						oInputName = Instantiate (prf_inputName, new Vector2 (0, 0), Quaternion.identity)as GameObject;
						oCursors [0] = GameObject.Find ("cursor1");
						oCursors [1] = GameObject.Find ("cursor1");
						oCursors [2] = GameObject.Find ("cursor2");
						oCursors [3] = GameObject.Find ("cursor3");
						oCursors [4] = GameObject.Find ("cursor4");
						oCursors [5] = GameObject.Find ("cursor5");
				} else {
						score.SendMessage ("postRank");
				}
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
				if (gesture.Selection == btn2) {
						oWarning = Instantiate (prf_warning, new Vector2 (0, 0), Quaternion.identity)as GameObject;
						
				}
				if (gesture.Selection.name.Equals ("YES")) {
						PlayerPrefs.SetInt ("NUMGEM", 0);
						Destroy (oWarning);
						oInputName = Instantiate (prf_inputName, new Vector2 (0, 0), Quaternion.identity)as GameObject;
						oCursors [0] = GameObject.Find ("cursor1");
						oCursors [1] = GameObject.Find ("cursor1");
						oCursors [2] = GameObject.Find ("cursor2");
						oCursors [3] = GameObject.Find ("cursor3");
						oCursors [4] = GameObject.Find ("cursor4");
						oCursors [5] = GameObject.Find ("cursor5");
			
				}
				if (gesture.Selection.name.Equals ("NO")) {
						Destroy (oWarning);
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

				if (gesture.Selection.tag.Equals ("key")) {
						if (posCursor <= 5)
								keyInput (gesture.Selection.transform.name);
						
				}

				if (gesture.Selection.name.Equals ("DEL")) {
						if (posCursor > 1)
								delKeyInput ();
			
				}
				if (gesture.Selection.name.Equals ("Cancel")) {
						Application.LoadLevel (1);
			
				}

				if (gesture.Selection.name.Equals ("OK")) {
						if (posCursor > 1) {
								string NAME = "";
								foreach (string human in myName) {
										NAME += human;
								}
								Debug.Log (NAME);
								PlayerPrefs.SetString ("MYNAME", NAME);
								score.SendMessage ("postName");
//								Destroy (oInputName);
						}
				}
				if (gesture.Selection.name.Equals ("Ok")) {
						Destroy (oError);
				}



				 
		}

		void returnName (string msg)
		{
				if (msg.Equals ("OK")) {
						Destroy (oInputName);
				} else {
						oError = Instantiate (prf_error, new Vector2 (0, 0), Quaternion.identity) as GameObject;
						Debug.Log (msg);
				}

		}

		void keyInput (string inputString)
		{
				oCursors [posCursor].SendMessage ("inputKey", inputString);
				myName [posCursor] = inputString;
				posCursor++;
				//		foreach( string human in rankList )
//		{
//			Debug.Log( human );
//		}
		}

		void delKeyInput ()
		{
				posCursor--;
				myName [posCursor] = "";
				oCursors [posCursor].SendMessage ("inputKey", "");

		}

		public static Vector4 hexColor (float r, float g, float b, float a)
		{
				Vector4 color = new Vector4 (r / 255, g / 255, b / 255, a / 255);
				return color;
		}
		 
}
