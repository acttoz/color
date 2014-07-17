using UnityEngine;
using System.Collections;

public class finger_gesture : MonoBehaviour
{
		public GameObject prf_touchObj;
		public bool testUp;
		private GameObject obj_touchObj;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		bool isCounting = false;
		int dragFingerIndex = -1;
	
		void OnDrag (DragGesture gesture)
		{
				// first finger
				FingerGestures.Finger finger = gesture.Fingers [0];
		
		
//		if (existBalloon) {
				if (gesture.Phase == ContinuousGesturePhase.Started) {
						// remember which finger is dragging balloon
						dragFingerIndex = finger.Index;
				
						// spawn some particles because it's cool.
						//				 SpawnParticles (balloon);
				} else if (finger.Index == dragFingerIndex) {  // gesture in progress, make sure that this event comes from the finger that is dragging our balloon
						if (gesture.Phase == ContinuousGesturePhase.Updated) {
								// update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
								Vector3 touchXY = GetWorldPos (gesture.Position);
								obj_touchObj.transform.position = touchXY;
						} else {
								// reset our drag finger index
								dragFingerIndex = -1;
						}
				}
//		}
		}
	
		void OnTap (TapGesture e)
		{
		
//		if (e.Selection == btn_menu) {
//			//						btn_menu.GetComponent<SpriteRenderer> ().color = Color.yellow;
//			Time.timeScale = 1.0f;
//			//						gameReset ();
//			bgm.SendMessage ("superMode", 1);
//			Application.LoadLevel (1);
//		}
//		if (e.Selection == btn_replay) {
//			//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.yellow;
//			int adNum;
//			adNum = PlayerPrefs.GetInt ("ADTIME", 0);
//			adNum++;
//			PlayerPrefs.SetInt ("ADTIME", adNum);
//			if (adNum > 10 && adNum % 3 == 0)
//				admob.SendMessage ("ShowInterstitial");
//			
//			btn_replay.GetComponent<SpriteRenderer> ().color = Color.white;
//			Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
//			Destroy (GameObject.Find ("prf_pause(Clone)"));
//			Time.timeScale = 1.0f;
//			existBalloon = false;
//			balloon.transform.localScale = new Vector3 (0, 0, 0);
//			balloon.SetActive (false);
//			gameReset ();
//			//												Application.LoadLevel (0);
//		}
//		if (e.Selection == btnNext) {
//			//						btnNext.GetComponent<SpriteRenderer> ().color = Color.yellow;
//			btnNext.GetComponent<SpriteRenderer> ().color = Color.white;
//			int adNum;
//			adNum = PlayerPrefs.GetInt ("ADTIME", 0);
//			adNum++;
//			PlayerPrefs.SetInt ("ADTIME", adNum);
//			if (adNum > 10 && adNum % 3 == 0) 
//				admob.SendMessage ("ShowInterstitial");
//			//						PlayerPrefs.SetInt (LEVEL + "", 1);
//			//			LEVEL = 9;
//			if (LEVEL < 10) {
//				LEVEL++;
//				PlayerPrefs.SetInt ("LEVEL", LEVEL);
//				if (LEVEL == 10) {
//					Application.LoadLevel (1);
//					bgm.SendMessage ("superMode", 1);
//				}
//			}
//			
//			Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
//			Destroy (GameObject.Find ("prf_pause(Clone)"));
//			Time.timeScale = 1.0f;
//			gameReset ();
//			//						gameReset ();
//			//						Application.LoadLevel (1);
//			//												Application.LoadLevel (0);
//		}
//		
//		
//		
//		if (e.Selection == btn_pause && onPlay) {
//			//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
//			btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
//			existBalloon = false;
//			StartCoroutine (pauseGame ());
//			//							 					Application.LoadLevel (0);
//		}
//		//				if (e.Selection == btn_pause && onCount) {
//		//						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
//		//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
//		//						StartCoroutine (pauseGame ());
//		//						//							 					Application.LoadLevel (0);
//		//				}
//		
//		if (e.Selection == btn_resume) {
//			//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.yellow;
//			btn_resume.GetComponent<SpriteRenderer> ().color = Color.white;
//			Destroy (GameObject.Find ("prf_pause(Clone)"));
//			onPlay = true;
//			existBalloon = false;
//			Time.timeScale = 1.0f;
//			
//			//												Application.LoadLevel (0);
//		}
//		
		
		
				//				Debug.Log ("click");
		}

		int stationaryFingerIndex = -1;

		void OnFingerStationary (FingerMotionEvent e)
		{

				if (e.Selection.tag == "mat") {
						if (e.Phase == FingerMotionPhase.Started) {
								if (stationaryFingerIndex != -1)
										return;
			
								Debug.Log ("stationaryS");
			
//								if (e.Selection.tag == "mat") {
								e.Selection.SendMessage ("onColor");
				
								stationaryFingerIndex = e.Finger.Index;
				
//								}
						} else if (e.Phase == FingerMotionPhase.Updated) {
								Debug.Log ("update" + e.Selection);
//								e.Selection.SendMessage ("onColor");
//			Debug.Log("stationaryU");
						
						} else if (e.Phase == FingerMotionPhase.Ended) {
								if (e.Finger.Index == stationaryFingerIndex) {
										Debug.Log ("stationaryE");
										e.Selection.SendMessage ("offColor");
										stationaryFingerIndex = -1;
								}
						}
				}
		}

		void OnFingerDown (FingerDownEvent e)
		{
				obj_touchObj = Instantiate (prf_touchObj, GetWorldPos (e.Position), Quaternion.identity) as GameObject;
//		if (onToast) {
//			if (e.Selection == monsterIcons [0]) {
//				if (!selectedMonster1)
//					selectedMonsterNum++;
//				selectedMonster1 = true;
//				e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
//				
//			}
//			if (e.Selection == monsterIcons [1]) {
//				if (!selectedMonster2)
//					selectedMonsterNum++;
//				selectedMonster2 = true;
//				e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
//				
//			}
//			if (e.Selection == monsterIcons [2]) {
//				if (!selectedMonster3)
//					selectedMonsterNum++;
//				selectedMonster3 = true;
//				e.Selection.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
//				
//			}
//		}
//		
//		if (!existBalloon && onPlay && e.Selection != btn_pause) {
//			existBalloon = true;
//			Create (GetWorldPos (e.Position));
//		}
//		if (!existBalloon && !onPlay && e.Selection == balloon && onCount) {
//			onPlay = true;
//			//			balloon.SendMessage("stopCount");
//			onCount = false;
//			existBalloon = true;
//			Create (GetWorldPos (e.Position));
//		}
		}
//	//FINGERUP
		void OnFingerUp (FingerUpEvent e)
		{
				if (!testUp)						
						Destroy (obj_touchObj);
//		//				tempLevel = superLevel;
//		//				GameObject[] temp = GameObject.FindGameObjectsWithTag ("light");
//		//				for (int i=0; i<temp.Length; i++) {
//		//						Destroy (temp [i]);
//		//				}
//		if (onToast) {
//			if (e.Selection == monsterIcons [0] || e.Selection == monsterIcons [1] || e.Selection == monsterIcons [2]) {
//				
//				if (selectedMonsterNum == 2) {
//					Destroy (GameObject.Find ("toast 1(Clone)"));
//					onToast = false;
//					gameStart ();
//				}
//			}
//			
//		}
//		if (e.Selection == btn_menu) {
//			//						btn_menu.GetComponent<SpriteRenderer> ().color = Color.white;
//			
//		}
//		if (e.Selection == btnNext) {
//			
//		}
//		if (e.Selection == btn_resume) {
//			
//		}
//		if (e.Selection == btn_replay) {
//			
//			//												Application.LoadLevel (0);
//		}
//		if (e.Selection == btn_pause && onPlay) {
//			
//			
//			//												Application.LoadLevel (0);
//		}
//		//				if (existBalloon && onPlay) {
//		//						CancelInvoke ("balloonStop");
//		//						StartCoroutine (Remove (1));			
//		//			
//		//				}
//		//		balloonRemove ();
//		//				Debug.Log ("release");
		}
	
		public static Vector3 GetWorldPos (Vector2 screenPos)
		{
				Ray ray = Camera.main.ScreenPointToRay (screenPos);
		
				// we solve for intersection with z = 0 plane
				float t = -ray.origin.z / ray.direction.z;
		
				return ray.GetPoint (t);
		}

}
