using UnityEngine;
using System.Collections;

public class key : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponentInChildren<tk2dTextMesh> ().text = transform.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
