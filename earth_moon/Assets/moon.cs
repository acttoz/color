using UnityEngine;
using System.Collections;

public class moon : MonoBehaviour
{
		public GameObject LookAtTo;
		public GameObject oSol, oEarth, oMoon;
		public GameObject btnText;
		tk2dTextMesh comBtnText;
		int flag;
		 
		// Use this for initialization
		void Start ()
		{
				comBtnText = btnText.GetComponent<tk2dTextMesh> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.LookAt (LookAtTo.transform.position);
		}

		void change ()
		{
				switch (flag) {
				case 0:
						flag = 1;
						changePlanet (oMoon, oEarth);
						comBtnText.text = "달의 시점";
						break;
			 
				case 1:
						flag = 0;
						changePlanet (oEarth, oMoon);
						comBtnText.text = "지구의 시점";
						break;
				}
		}

		void changePlanet (GameObject basePlanet, GameObject lookPlanet)
		{
				transform.parent = basePlanet.transform;
				transform.localPosition = new Vector3 (0, 0, 0);
				LookAtTo = lookPlanet;
		}
}
