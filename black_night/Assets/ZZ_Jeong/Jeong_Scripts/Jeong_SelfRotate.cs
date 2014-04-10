using UnityEngine;
using System.Collections;

public class Jeong_SelfRotate : MonoBehaviour {

	public float Speed;   // 0~1
	
	
	// Update is called once per frame
	void Update () {
		 transform.Rotate(Vector3.up * Time.deltaTime * Speed);
	}
}
