using UnityEngine;
using System.Collections;

public class SavePrefs : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnSubmit ()
	{
		Debug.Log (GetComponent<UIInput> ().text);	
		GameObject.Find ("Panel").SendMessage ("PopUp", GetComponent<UIInput> ().text);
		
		
	}
}
