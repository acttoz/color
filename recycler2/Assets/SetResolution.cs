using UnityEngine;
using System.Collections;

public class SetResolution : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		UIRoot root = GameObject.Find ("Camera").GetComponent<UIRoot> (); 
// 요건 씬이 어떻게 제작되었는지 모르기 때문에 일단 
// 안전하게 박아뒀습니다.  게임 가로 480기준... 
		root.manualHeight = 960; 
		root.minimumHeight = 480; 
		root.maximumHeight = 1280; 

		GameObject obj = GameObject.Find ("Camera"); 

		Camera camera = obj.GetComponent<Camera> (); 

// 원래 게임이 320*480으로 제작되어 있다 가정했을경우 
		float perx = 640.0f / Screen.width; 
		float pery = 960.0f / Screen.height; 
		float v = (perx > pery) ? perx : pery; 
		camera.orthographicSize = v; 
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
