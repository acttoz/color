using UnityEngine;
using System.Collections;

public class btn : MonoBehaviour
{
		public enum func
		{
				tomenu,
				replay,
				next,
				play,
				test
		}
		public func _func;
//		public func _func = func.next;
		// Use this for initialization
		void Start ()
		{
				STATE._STATE = "WAIT";
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTap ()
		{
//				func _func = ;
				if (!STATE._STATE.Equals ("WAIT"))
						return;
				switch (_func) {
				case  func.tomenu:
						STATE._STATE = "WAIT";
						Application.LoadLevel (0);
						break;
				case  func.next:
						break;
				case  func.replay:
						STATE._STATE = "READY";
						Destroy (this.gameObject.transform.parent.parent.gameObject);
						break;
				case  func.play:
						Application.LoadLevel (1);
						break;
				case  func.test:
						Debug.Log ("TAP");
						break;
				}
		}
}
