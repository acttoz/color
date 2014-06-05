using UnityEngine;
using System.Collections;

public class scr_numGem : MonoBehaviour
{
		public string gemId;
		// Use this for initialization
		int gemNum;

		void Start ()
		{

				countGem ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				
	
		}

		void countGem ()
		{
				gemNum = PlayerPrefs.GetInt ("NUMGEM");
		Debug.Log(gemNum);
				switch (int.Parse(gemId)) {
				case 1:
						gemNum = gemNum / 100;
						break;
				case 2:
						gemNum = gemNum / 10 - (gemNum / 100 * 10);
			
						break;
				case 3:
						gemNum = gemNum - (gemNum / 10 * 10);
			
						break;
				}
				GetComponent<tk2dTextMesh> ().text = ": " + gemNum;
	
		}
}
