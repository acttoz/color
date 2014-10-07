using UnityEngine;
using System.Collections;

public class STATE : MonoBehaviour
{
 
		public static string _STATE = "READY";
		public static bool isTouched = false;
		public static int mats = 0;//colored mat
		public static int matsAll = 0;//number of all mats
		public static int stars = 3;
		private GameObject oMats;//parent of mats
		private GameObject oCamera;
		private color_mat[] matArray;
		private int[] bossIds;
		tk2dTextMesh stateText;
		MANAGER manager;
		// Use this for initialization
		void Start ()
		{
				oCamera = GameObject.FindWithTag ("MainCamera");
				STATE._STATE = "READY";
				oMats = GameObject.Find ("mats");
				matArray = oMats.GetComponentsInChildren<color_mat> ();
				bossIds = new int[matArray.Length];
				for (int i=0; i<matArray.Length; i++) {
						bossIds [i] = i;
				}
				for (int i=0; bossIds.Length>i; i++) {
						int r = Random.Range (0, i);
						int tmp = bossIds [i] + 0;
						bossIds [i] = bossIds [r];
						bossIds [r] = tmp;
				}
				foreach (int i in bossIds)
						matArray [i].bossID = bossIds [i];
//				Debug.Log ("" + matArray);
				stateText = GetComponentInChildren<tk2dTextMesh> ();
				manager = GetComponent<MANAGER> ();
				
		}
	 
	
		// Update is called once per frame
		void Update ()
		{
				stateText.text = _STATE;
				switch (_STATE) {
				case  "READY":
						manager.ready ();
						_STATE = "WAIT";
						break;
				case  "WAIT":
						oCamera.transform.position = new Vector3 (0, 0, -50);
			
						break;
				case  "START":
						manager.reset ();
						_STATE = "gIDLE";
						break;
				case  "NEXT":
						break;
				case  "gIDLE":
						oCamera.transform.position = new Vector3 (-1.5f, 1.5f, -50);
						break;
				case  "gTOUCH":
						break;
				case  "gSUCCESS":
						manager.success ();
						_STATE = "WAIT";
						break;
				case  "gFAIL":
						manager.fail ();
						_STATE = "WAIT";
						break;
				
				}
		}

 
}
