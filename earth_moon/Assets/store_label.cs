using UnityEngine;
using System.Collections;

public class store_label : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<tk2dTextMesh>().text="Lv."+transform.parent.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
