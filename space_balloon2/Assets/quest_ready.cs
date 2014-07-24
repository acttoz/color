using UnityEngine;
using System.Collections;

public class quest_ready : MonoBehaviour
{
		public GameObject[] questList;
		// Use this for initialization
		void Start ()
		{
				int level = PlayerPrefs.GetInt ("LEVEL", 1) - 11;
				GameObject obj = Instantiate (questList [level], this.gameObject.transform.position, Quaternion.identity) as GameObject;
				obj.transform.parent = this.gameObject.transform;
				obj.transform.localScale = new Vector3 (0.9f, 0.9f,1);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
