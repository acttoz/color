using UnityEngine;
using System.Collections;

public class TrashBoxRight : MonoBehaviour
{
	public GameObject moon;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnFingerDown ()
	{
	
		animation.Play ();

		moon.animation.Play ("cleanRight");	
		
	}
	
}
