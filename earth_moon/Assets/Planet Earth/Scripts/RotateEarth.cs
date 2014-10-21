using UnityEngine;
using System.Collections;

public class RotateEarth : MonoBehaviour
{

		private float mouseXAmount, mouseYAmount;
		public bool isEarth;
		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				int fingerCount = 0;
				foreach (Touch touch in Input.touches) {
						if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
								fingerCount++;
			
				}
		
		
				if (fingerCount < 2) {
						if (Input.GetMouseButton (0)) {
								mouseXAmount = -Input.GetAxis ("Mouse X") * 3f;
								if (!isEarth)
										transform.Rotate (0, mouseXAmount, 0);
								else
					transform.Rotate (0, 0, mouseXAmount);
					
						}
				}
		 
		}
}
