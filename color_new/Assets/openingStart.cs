using UnityEngine;
using System.Collections;

public class openingStart : MonoBehaviour {
	public GameObject opening;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void opening_call(){
		Instantiate(opening,new Vector2(0,0),Quaternion.identity);
		Destroy (this.gameObject);
	}

}
