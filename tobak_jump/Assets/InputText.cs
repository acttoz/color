using UnityEngine;
using System.Collections;

public class InputText : MonoBehaviour
{
		string txt;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				txt = this.GetComponent<tk2dTextMesh> ().text;
				char[] charsToTrim = { ' ', '\t' };
				string result = txt.Trim (charsToTrim);
				MANAGER.instance.inputText = result;
		}
}
