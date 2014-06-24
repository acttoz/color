using UnityEngine;
using System.Collections;

public class MANAGER_START : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void GAMEMANAGERSTART ()
		{
				GameObject.Find ("GAMEMANAGER").SendMessage ("gameStart");
		}
}
