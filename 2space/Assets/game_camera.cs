using UnityEngine;
using System.Collections;

public class game_camera : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void clearCamera ()
		{
				camera.clearFlags = CameraClearFlags.Nothing;
		}
}
