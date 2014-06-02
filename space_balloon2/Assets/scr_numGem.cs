using UnityEngine;
using System.Collections;

public class scr_numGem : MonoBehaviour
{
	public string gemId;
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
				GetComponent<tk2dTextMesh> ().text = ": " + PlayerPrefs.GetInt ("NUMGEM"+gemId);
	
		}
}
