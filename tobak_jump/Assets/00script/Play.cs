using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour
{
		
		public static Play mInstance;

		public Play ()
		{
				mInstance = this;
		}
	
		public static Play instance {
				get {
						if (mInstance == null)
								new Play ();
						return mInstance;
				}
		}
		
		void Start ()
		{
			
 
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				if (Application.platform == RuntimePlatform.Android) {
						if (Input.GetKey (KeyCode.Escape)) {
				
								return;
				
						}
				}
		
		}
 
		/************************ Control **********************/
		bool isCounting = false;
		int dragFingerIndex = -1;
	
//		void OnDrag (DragGesture gesture)
//		{
//				// first finger
//				FingerGestures.Finger finger = gesture.Fingers [0];
//		
//		
//				if (MANAGER.instance.state.Equals ("PLAY")) {
//						if (gesture.Phase == ContinuousGesturePhase.Started) {
//								// remember which finger is dragging balloon
//								dragFingerIndex = finger.Index;
//				
//								// spawn some particles because it's cool.
//								//				 SpawnParticles (balloon);
//						} else if (finger.Index == dragFingerIndex) {  // gesture in progress, make sure that this event comes from the finger that is dragging our balloon
//								if (gesture.Phase == ContinuousGesturePhase.Updated) {
//										// update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
//									 
//										touchPoint.transform.position = GetWorldPos (gesture.Position);
//								} else {
//										// reset our drag finger index
//										dragFingerIndex = -1;
//								}
//						}
//				}
//		}
	
		void OnTap (TapGesture e)
		{
				if (e.Selection.name == "touch_left") {
						Player.instance.touchLeft ();
				}
				if (e.Selection.name == "touch_Right") {
						Player.instance.touchRight ();
				}
				if (e.Selection.name == "btn_menu") {
						Application.LoadLevel (1);
				}
				if (e.Selection.name == "btn_replay") {
						//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.yellow;
						int adNum;
						adNum = PlayerPrefs.GetInt ("ADTIME", 0);
						adNum++;
						PlayerPrefs.SetInt ("ADTIME", adNum);
			
						//						btn_replay.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_timesup 1(Clone)"));
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
						//												Application.LoadLevel (0);
				}
				if (e.Selection.name == "btn_next") {
			
				}
		
		
		
				if (e.Selection.name == "btn_pause" && MANAGER.instance.state.Equals ("IDLE")) {
						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
						//							 					Application.LoadLevel (0);
						MANAGER.instance.state = "PAUSE";
				}
				//				if (e.Selection == btn_pause && onCount) {
				//						//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.yellow;
				//						btn_pause.GetComponent<SpriteRenderer> ().color = Color.white;
				//						StartCoroutine (pauseGame ());
				//						//							 					Application.LoadLevel (0);
				//				}
		
				if (e.Selection.name == "btn_resume") {
						//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.yellow;
						//						btn_resume.GetComponent<SpriteRenderer> ().color = Color.white;
						Destroy (GameObject.Find ("prf_pause(Clone)"));
						Time.timeScale = 1.0f;
			
						//												Application.LoadLevel (0);
				}
		
		
		
				//				Debug.Log ("click");
		}
	
		void OnFingerDown (FingerDownEvent e)
		{
			 
		
				if (MANAGER.instance.state.Equals ("IDLE") && e.Selection.name != "btn_pause") {
//						MANAGER.instance.state = "PLAY";
//						Create (GetWorldPos (e.Position));
//						touchPoint.transform.position = GetWorldPos (e.Position);
				}
		
		}
		//FINGERUP
		void OnFingerUp (FingerUpEvent e)
		{
				//			
				//						//												Application.LoadLevel (0);
				//				}
				//				if (existBalloon && onPlay) {
				//						CancelInvoke ("balloonStop");
				//						StartCoroutine (Remove (1));			
				//			
				//				}
				//		balloonRemove ();
				//				Debug.Log ("release");
		}
	
		public static Vector3 GetWorldPos (Vector2 screenPos)
		{
				Ray ray = Camera.main.ScreenPointToRay (screenPos);
		
				// we solve for intersection with z = 0 plane
				float t = -ray.origin.z / ray.direction.z;
		
				return ray.GetPoint (t);
		}

	
}
