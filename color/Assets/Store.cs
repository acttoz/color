using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour
{
		public GameObject[] oLevels = new GameObject[6];
		public GameObject backBtn;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTap (TapGesture gesture)
		{
				if (gesture.Selection == oLevels [0]) {
						gesture.Selection.SendMessage ("equip", 1);
				}
				if (gesture.Selection == oLevels [1]) {
						gesture.Selection.SendMessage ("equip", 1);
						PlayerPrefs.SetInt ("LIMIT", 6);
				}
				if (gesture.Selection == oLevels [2]) {
						gesture.Selection.SendMessage ("equip", 1);
						PlayerPrefs.SetInt ("LIMIT", 7);
				}
				if (gesture.Selection == oLevels [3]) {
						gesture.Selection.SendMessage ("equip", 1);
						PlayerPrefs.SetInt ("LIMIT", 8);
				}
				if (gesture.Selection == oLevels [4]) {
						gesture.Selection.SendMessage ("equip", 1);
						PlayerPrefs.SetInt ("LIMIT", 9);
				}
				if (gesture.Selection == oLevels [5]) {
						gesture.Selection.SendMessage ("equip", 1);
						PlayerPrefs.SetInt ("LIMIT", 10);
				}
				if (gesture.Selection == backBtn) {
						Application.LoadLevel (1);
				}
		}
}
