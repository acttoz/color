using UnityEngine;
using System.Collections;

public class finger_gesture : MonoBehaviour
{
		public GameObject prf_brush;
		public GameObject prf_hammer;
		public GameObject oPlayer;
		Animator animator;
		public bool testUp;
		private bool onColor = false;
		private GameObject obj_touched;
		public static string state = "brush";//pump,hammer
		Enemy enemy;
		STATE Component_STATE;
		// Use this for initialization
		void Start ()
		{
				enemy = GetComponent<Enemy> ();
				Component_STATE = GetComponent<STATE> ();
				animator = oPlayer.GetComponent<Animator> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

//		bool isCounting = false;
		int dragFingerIndex = -1;
	
		void OnDrag (DragGesture gesture)
		{
				if (!STATE.isTouched) 
						return;
				if (!STATE._STATE.Equals ("gIDLE") || state.Equals ("hammer"))
						return;
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
								obj_touched.transform.position = touchXY;
						} else {
								// reset our drag finger index
								dragFingerIndex = -1;
						}
				}
//		}
		}
	
		void OnTap (TapGesture e)
		{
//				if (!STATE._STATE.Equals ("WAIT") || STATE.isTouched)
//						return;
				Debug.Log (e.Selection.name);
				switch (e.Selection.name) {
				case  "btn_menu":
//						STATE._STATE = "WAIT";
						Application.LoadLevel (0);
						break;
				case  "btn_next":
						break;
		
				case  "btn_replay":
						STATE._STATE = "READY";
						Destroy (e.Selection.gameObject.transform.parent.parent.gameObject);
						break;
				case  "btn_play":
						Application.LoadLevel (1);
						break;
			 

		
				}
		
		}

		void OnFingerHover (FingerHoverEvent e)
		{
				if (!STATE._STATE.Equals ("gIDLE"))
						return;

				if (e.Selection.tag == "mat" && state.Equals ("brush")) {
						// finger entered the object
						if (e.Phase == FingerHoverPhase.Enter) {
								e.Selection.SendMessage ("onColor");
						} else if (e.Phase == FingerHoverPhase.Exit) { // finger left the object
								e.Selection.SendMessage ("offColor");
						}
				}
				if (e.Selection.tag == "mat" && state.Equals ("pump")) {
						// finger entered the object
						if (e.Phase == FingerHoverPhase.Enter) {
								e.Selection.SendMessage ("onPump");
						} else if (e.Phase == FingerHoverPhase.Exit) { // finger left the object
								e.Selection.SendMessage ("offPump");
						}
				}
		}
 
		void OnFingerDown (FingerDownEvent e)
		{
				if (!STATE._STATE.Equals ("gIDLE"))
						return;
				switch (e.Selection.name) {
		 
				case  "btn_brush":
						e.Selection.gameObject.animation.Play ();
						Component_STATE.cameraHover (true);
						state = "brush";
						animator.SetBool ("artist", true);
						break;
			 
				case  "btn_hammer":
						e.Selection.gameObject.animation.Play ();
						Component_STATE.cameraHover (false);
						state = "hammer";
						animator.SetBool ("artist", false);
						break;

				default:
						if (!STATE.isTouched) {
								STATE.isTouched = true;
				
				
								if (state.Equals ("brush")) {
										obj_touched = Instantiate (prf_brush, GetWorldPos (e.Position), Quaternion.identity) as GameObject;
								} else {
										obj_touched = Instantiate (prf_hammer, GetWorldPos (e.Position), Quaternion.identity) as GameObject;
										if (e.Selection.tag.Equals ("boss"))
												e.Selection.SendMessage ("thiefOff");
								}
				
						}
						break;
			
			
				}

 
		}

		void OnFingerUp (FingerUpEvent e)
		{
		if (STATE.isTouched) {
		
						STATE.isTouched = false;
		
						if (!STATE._STATE.Equals ("gIDLE"))
								return;

			if (obj_touched != null&& state.Equals ("brush"))
								Destroy (obj_touched);
				}
 
		}
	
		public static Vector3 GetWorldPos (Vector2 screenPos)
		{
				Ray ray = Camera.main.ScreenPointToRay (screenPos);
		
				// we solve for intersection with z = 0 plane
				float t = -ray.origin.z / ray.direction.z;
		
				return ray.GetPoint (t);
		}

}
