using UnityEngine;
using System.Collections;

public class MoonCameraWork : MonoBehaviour
{
		public string scene;
		public GameObject oCamera;
		public GameObject bgm;
		GameObject oSelectedOrbit;
		bool bMoveStart1 = false;
		bool bMoveStart2 = false;
		bool count1 = false;
		bool count2 = false;
		bool count3 = false;
		// Use this for initialization
		void Start ()
		{
		}
  
		// Update is called once per frame
		void Update ()
		{

				if (Input.GetKeyDown (KeyCode.Escape)) { // 아이폰에서도 작용하는지 모르겠음

						if (scene.Equals ("main")) {
								Application.Quit ();
						} else {
								Application.LoadLevel (1);
						}
				}
		 
				if (bMoveStart1) {
						Vector3 orbit = oSelectedOrbit.transform.localPosition;
						Vector3 cam = oCamera.transform.localPosition;
//						oCamera.transform.position -= new  Vector3 (0f, scaleRate, 1);
						if (cam.x > 0.5f) {
								oCamera.transform.localPosition -= new Vector3 (1, 0, 0);
						} else if (cam.x < -0.5f) {
								oCamera.transform.localPosition += new Vector3 (1, 0, 0);
				
						} else {
								count1 = true;
								Debug.Log ("count1");
//								oCamera.transform.LookAt (oSelectedOrbit.transform);
				
						}

						if (cam.y > 15f) {
								oCamera.transform.localPosition -= new Vector3 (0, 1, 0);
						} else if (cam.y < -15f) {
								oCamera.transform.localPosition += new Vector3 (0, 1, 0);

						} else {
				
								count2 = true;
//								oCamera.transform.LookAt (oSelectedOrbit.transform);
				
								
						}
						if (cam.z > 0.5f) {
								oCamera.transform.localPosition -= new Vector3 (0, 0, 1);
						} else if (cam.z < -0.5f) {
								oCamera.transform.localPosition += new Vector3 (0, 0, 1);
				
						} else {
				
								count3 = true;
								oCamera.transform.LookAt (oSelectedOrbit.transform);
				

						}

						if (count1 && count2 && count3) {
								bMoveStart1 = false;
				
						}
				
				}
				
		}

		void call_camera (string orbitName)
		{
				Debug.Log ("받음" + orbitName);
				oSelectedOrbit = GameObject.Find (orbitName);
				oCamera.transform.parent = oSelectedOrbit.transform;
				oCamera.transform.LookAt (oSelectedOrbit.transform);
		
				oCamera.transform.position = new Vector3 (0, 100, 0);
				count1 = false;
				count2 = false;
				count3 = false;
				bMoveStart1 = true;

				this.SendMessage ("ShowContent", orbitName); // Modified by Jeong Dae Yeong


		}

		void btn1 ()
		{
				Application.LoadLevel (2);
		}

		void btn2 ()
		{
				Application.LoadLevel (3);
		}

}
