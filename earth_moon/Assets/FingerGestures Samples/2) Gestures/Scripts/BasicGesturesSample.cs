using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates the use of the following gestures via messages sent by their 
/// respective GestureRecognizers (on the same object)
/// - Swipe (SwipeRecognizer)
/// - Tap & DoubleTap (TapRecognizer)
/// - Drag (DragRecognizer)
/// - LongPress (LongPressRecognizer)
/// </summary>
public class BasicGesturesSample : MonoBehaviour
{
		public bool isMoon = false;
		public GameObject MANAGER;
		// spin the yellow cube when swipping it
		
		void OnTap (TapGesture gesture)
		{
				if (gesture.Selection == tapObject) {
//						SpawnParticles (tapObject);
//						UI.StatusText = "Tapped with finger " + gesture.Fingers [0];
						if (isMoon) {
								MANAGER.SendMessage ("change");
						}
				}
		}
    
		

		int dragFingerIndex = -1;

		public GameObject tapObject;

}
