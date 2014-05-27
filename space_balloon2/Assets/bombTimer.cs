using UnityEngine;
using System.Collections;

public class bombTimer : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
			
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		void onTimer (float num)
		{
				StartCoroutine ("timer", num);
		}
	
		IEnumerator timer (float num)
		{
				yield return new WaitForSeconds (num);
//				GameObject.Find("GAMEMANAGER").SendMessage ("getBalloonMSG", 5);
				Destroy (this.gameObject);
		
		}
}
