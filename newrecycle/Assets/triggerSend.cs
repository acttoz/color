using UnityEngine;
using System.Collections;

public class triggerSend : MonoBehaviour
{

	// Use this for initialization
//	public GameObject energyBar;
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name == "SuccessTrigger") {
			
			Destroy(this.gameObject);
			Debug.Log ("Yay!");
		}
	}
}
