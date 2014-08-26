using UnityEngine;
using System.Collections;

public class matParent : MonoBehaviour {
	SpriteRenderer[] children;
	public Color colorBlue;
	// Use this for initialization
	void Start () {
		children=GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void resetColor(){
		foreach(SpriteRenderer element in children){
			element.color=colorBlue;
		}

	}
}
