using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour
{
	public int rotateRate;
		public GameObject cameraPoint;
		bool moveCamera = false;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (moveCamera) {
						float step = 5000f * Time.deltaTime;
			float step2 = rotateRate * Time.deltaTime;
						transform.position = Vector3.MoveTowards (transform.position, cameraPoint.transform.position, step);
						transform.rotation = Quaternion.RotateTowards (transform.rotation, cameraPoint.transform.rotation, step2);
						if (transform.position == cameraPoint.transform.position && transform.rotation == cameraPoint.transform.rotation) {
				
								moveCamera = false;
//								StartCoroutine (cameraWait ());
				
						}
				}
		}

		void cameraWork ()
		{
				moveCamera = true;
		}
}
