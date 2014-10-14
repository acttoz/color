/// <summary>
/// 2D Space Shooter Example
/// By Bug Games www.Bug-Games.net
/// Programmer: Danar Kayfi - Twitter: @DanarKayfi
/// Special Thanks to Kenney for the CC0 Graphic Assets: www.kenney.nl
/// 
/// This is the GameController Script:
/// - Control The Waves of the asteroid and the enemies
/// 
/// </summary>
using UnityEngine;
using System.Collections;

public class GameController_Script : MonoBehaviour
{	
		public GameObject oPlayer;
		// Use this for initialization
		void Start ()
		{
//		StartCoroutine (asteroidSpawnWaves());  	//Start IEnumerator function
//		StartCoroutine (enemyBlueSpawnWaves());		//Start IEnumerator function
//		StartCoroutine (enemyGreenSpawnWaves());	//Start IEnumerator function
//		StartCoroutine (enemyRedSpawnWaves());		//Start IEnumerator function
		}

		// Update is called once per frame
		void Update ()
		{
				 
		}

		void toLevel (int num)
		{
				StartCoroutine (level (num));
		}

		IEnumerator  level (int id)
		{
				Debug.Log ("level");
				yield return new WaitForSeconds (1f);
				Application.LoadLevel (id);
		}

		bool isCounting = false;
		int dragFingerIndex = -1;
	
		void OnDrag (DragGesture gesture)
		{
//				if (!STATE._STATE.Equals ("gIDLE"))
//						return;
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
								oPlayer.transform.position = touchXY;
						} else {
								// reset our drag finger index
								dragFingerIndex = -1;
						}
				}
				//		}
		}

		public static Vector3 GetWorldPos (Vector2 screenPos)
		{
				Ray ray = Camera.main.ScreenPointToRay (screenPos);
		
				// we solve for intersection with z = 0 plane
				float t = -ray.origin.z / ray.direction.z;
		
				return ray.GetPoint (t);
		}
}
