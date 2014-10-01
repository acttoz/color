using UnityEngine;
using System.Collections;

public class UiFollow : MonoBehaviour {
	public GameObject balloon;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = balloon.transform.position;
	}
}
