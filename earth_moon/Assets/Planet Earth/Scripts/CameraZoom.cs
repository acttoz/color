using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			transform.Translate(0,0,100f);
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			transform.Translate(0,0,-100f);
		}
	}
}
