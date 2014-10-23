using UnityEngine;
using System.Collections;

public class BackKey : MonoBehaviour
{
	public string toScene = "main_potrait";
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {

			if (Input.GetKey (KeyCode.Escape)) {

				// Insert Code Here (I.E. Load Scene, Etc)

				// OR Application.Quit();
				loadScene (toScene);
 

				return;
			}
		}
	}

	void loadScene (string temp)
	{
		if (temp == "exit") {
			Application.Quit ();
		} else {
		
			Application.LoadLevel (temp);
		}
		//DontDestroyOnLoad(GameObject.FindGameObjectWithTag("umbrella"));
	}
}
