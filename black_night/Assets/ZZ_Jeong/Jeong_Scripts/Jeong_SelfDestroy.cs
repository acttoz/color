using UnityEngine;
using System.Collections;

public class Jeong_SelfDestroy : MonoBehaviour {

	private Jeong_ShowContent _a;   // 다른 컴포턴트 변수 변경함 

	public GameObject A_Object;

	// Use this for initialization
	void Start () {
	
		_a =A_Object.GetComponent<Jeong_ShowContent>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void selfDestroy()
	{
		_a.btnBool = true;
	}
}
