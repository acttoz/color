using UnityEngine;
using System.Collections;

public class scoreList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void showScores(string text){
		GetComponent<tk2dTextMesh> ().text = text;

	}
}
