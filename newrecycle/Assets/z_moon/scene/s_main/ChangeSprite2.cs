using UnityEngine;
using System.Collections;

public class ChangeSprite2 : MonoBehaviour {
	tk2dSprite sprite;
	public Camera mainCam;
	public GameObject loadAnim;
	// Use this for initialization
	void Start () {
	 sprite = GetComponent<tk2dSprite>();
    
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTap(){
		sprite.spriteId=sprite.GetSpriteIdByName("btn2");
//		GetComponent<TBTap>().enabled = false;
//		
		//		StartCoroutine( "rolldownPlay" );
		
		loadScene("gamePotrait");
	
	}

 


		
	
	
	void loadScene(string temp)
	{
		Application.LoadLevelAsync (temp);
		mainCam.transform.Rotate(0,-90,0);
		loadAnim.animation.Play ();
		//DontDestroyOnLoad(GameObject.FindGameObjectWithTag("umbrella"));
	}
	
}