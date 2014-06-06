using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour
{
		public GameObject[] oLevels = new GameObject[6];
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
				}
				if (gesture.Selection == oLevels [2]) {
						gesture.Selection.SendMessage ("equip", 1);
				}
				if (gesture.Selection == oLevels [3]) {
						gesture.Selection.SendMessage ("equip", 1);
				}
				if (gesture.Selection == oLevels [4]) {
						gesture.Selection.SendMessage ("equip", 1);
				}
				if (gesture.Selection == oLevels [5]) {
						gesture.Selection.SendMessage ("equip", 1);
				}
		}
}
