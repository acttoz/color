using UnityEngine;
using System.Collections;

public class STATE : MonoBehaviour
{
		public GameObject prfGameset;
		public static string _STATE="DEFAULT";
//		{
//				DEFAULT,
//				WAIT,
//				PLAYING,
//				START
//	}
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				switch (_STATE) {
				case  "DEFAULT":
						break;
				case  "WAIT":
						break;
				case  "PLAYING":
						break;
				case  "READY":
						Instantiate (prfGameset, new Vector2 (0, 0), Quaternion.identity);
			_STATE="WAIT";
						break;
				case  "START":
						Debug.Log ("START");
						break;
				}
		}

//		void CHANGESTATE (_STATE state)
//		{
//				switch (state) {
//				case _STATE.DEFAULT:
//						break;
//				case _STATE.WAIT:
//						break;
//				case _STATE.PLAYING:
//						break;
//				case _STATE.START:
//						Debug.Log ("START");
//						break;
//				}
//		
//		}
}
