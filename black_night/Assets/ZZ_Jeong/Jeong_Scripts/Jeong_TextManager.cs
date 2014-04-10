using UnityEngine;
using System.Collections;

public class Jeong_TextManager : MonoBehaviour {
	
	
	public GameObject Text1,Text2,Text3,Text4;
	
	
	// Use this for initialization
	void Start () {
	
		Text1.SetActive(true);
		Text2.SetActive (true);
		Text3.SetActive (false);
		Text4.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void ShowNextLine()
	{
		Text1.SetActive(false);
		Text2.SetActive (false);
		Text3.SetActive (true);
		Text4.SetActive (true);
		GameObject.Find ("Btn_Check").SetActive(false);
	}
	
}
