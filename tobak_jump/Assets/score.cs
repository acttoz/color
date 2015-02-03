using UnityEngine;
using System.Collections;

public class score : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
				GetComponent<tk2dTextMesh> ().text = MANAGER.instance.score+"";
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
