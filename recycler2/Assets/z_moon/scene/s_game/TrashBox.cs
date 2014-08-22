using UnityEngine;
using System.Collections;

public class TrashBox : MonoBehaviour
{
	public GameObject moon,parentTrash;
	public string boxId;
	public static int trashId = 1;
	public static GameObject[] trashes;
	GameObject mGameManager;

	// Use this for initialization
	void Start ()
	{
		
		mGameManager = GameObject.Find ("GAMEMANAGER");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		trashes = GameObject.FindGameObjectsWithTag ("enemy");
	}
	
	void OnFingerDown ()
	{
	
	
		
//		Destroy (GameObject.Find (trashId.ToString () + "(Clone)"));
		trashes = GameObject.FindGameObjectsWithTag ("enemy");
		
		if (trashes.Length != 0) {
			
			checkTrash ();
		} else {
//			torch.active = true;
			
				
		}
		
	}
		
	void checkTrash ()
	{
		if (GameObject.Find (trashId.ToString () + "(Clone)").GetComponent<Trashes> ().trashColor == boxId) {
			
			audio.Play ();
			
			
			animation.Play ();
			if (boxId == "red" || boxId == "blue") {
				moon.animation.Play ("clean");
			} else {
				moon.animation.Play ("cleanRight");
			}
			GameObject.Find (trashId.ToString () + "(Clone)").SendMessage ("explosion");
				
			trashId++;
			
			
			
		} else {
			//틀렸을때!!
			mGameManager.SendMessage("mWrong");
			parentTrash.animation.Play();
			
			
//			if (Game_Manager.mTorch.active)
//				Game_Manager.mTorch.SetActive (false);
//			StartCoroutine ("wrong");
		}
	}
	
	
	
}
