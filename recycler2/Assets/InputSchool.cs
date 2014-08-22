using UnityEngine;
using System.Collections;

public class InputSchool : MonoBehaviour
{
	public GameObject mMainManger;

	// Use this for initialization
	void Start ()
	{
	//테스트
//		mMainManger.SendMessage ("searchSchool", "수");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void awake()
	{
	}

	void OnSubmit ()
	{
	
		mMainManger.SendMessage ("searchSchool", GetComponent<UIInput> ().text);
			
			
	}
}
