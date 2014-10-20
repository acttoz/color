using UnityEngine;
using System.Collections;

public class main : MonoBehaviour
{
		public GameObject[] texts;
		public GameObject[] touches;
		public AudioClip whic, bounce;
		public GameObject camera, returnPoint;
		int level = 0;
		bool returnPosition = false;
		// Use this for initialization
		void Start ()
		{
				foreach (GameObject element in touches) {
						element.SetActive (false);
				}
		}

		void returnCamera ()
		{
				audio.PlayOneShot (whic);
				foreach (GameObject element in touches) {
						element.SetActive (false);
				}
				returnPosition = true;
				foreach (GameObject element in texts) {
						element.SendMessage ("angle3d");
				}
		}

		void playBounce ()
		{
				audio.PlayOneShot (bounce);
		
		}
		// Update is called once per frame
		void Update ()
		{
				if (returnPosition) {
						float step = 5f * Time.deltaTime;
						camera.transform.position = Vector3.MoveTowards (camera.transform.position, returnPoint.transform.position, step);
						if (!camera.animation.isPlaying)			
								camera.animation.Play ("zoomOut");
						if (camera.transform.position == returnPoint.transform.position) {

								returnPosition = false;
//								StartCoroutine (cameraWait ());
			
						}
				}
		}

		IEnumerator cameraWait ()
		{
				yield return new WaitForSeconds (2.5f);
				cameraWork ();
		}

		void cameraWork ()
		{
				audio.PlayOneShot (whic);
				 
				level = PlayerPrefs.GetInt ("LEVEL", 1);
//				level =2;

				switch (level) {
				case 1:
						touches [level - 1].SetActive (true);
						PlayerPrefs.SetInt ("LEVEL", 2);

						camera1 ();
						break;
				case 2:
						touches [level - 1].SetActive (true);
						PlayerPrefs.SetInt ("LEVEL", 3);
						camera2 ();
						break;
				case 3:
						touches [level - 1].SetActive (true);
						PlayerPrefs.SetInt ("LEVEL", 4);
						camera3 ();
						break;
				case 4:

						PlayerPrefs.SetInt ("LEVEL", 1);
						Application.LoadLevel (1);
						break;
				}
		}

		IEnumerator text3d ()
		{
				yield return new WaitForSeconds (1f);
				foreach (GameObject element in texts) {
						element.SendMessage ("angle2d");
				}
//				texts [level].SendMessage ("Play");
//				GameObject.FindGameObjectWithTag ("MainCamera").transform.localPosition = texts [level].transform.position;
		}

		void camera1 ()
		{
				camera.animation.Play ("camera1");
				StartCoroutine (text3d ());
		}

		void camera2 ()
		{
				camera.animation.Play ("camera2");
				StartCoroutine (text3d ());
		}

		void camera3 ()
		{
				camera.animation.Play ("camera3");
				StartCoroutine (text3d ());
		}
}
