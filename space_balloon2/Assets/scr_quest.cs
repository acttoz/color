using UnityEngine;
using System.Collections;

public class scr_quest : MonoBehaviour
{
		public enum questID
		{
				SHOT,
				SPACE,
				KILL,
				ALIVE1,
				ALIVE2,
				COLLECT
		}
		public questID quest;
		private int level;
		public int[] conLevel;
		public int[] conPoint;
		// Use this for initialization
		void Start ()
		{
				switch (quest) {
				case questID.SHOT: 

						break;
				case questID.SPACE:
						break;
				case questID.KILL:
						break;
				case questID.ALIVE1:
						break;
				case questID.ALIVE2:
						break;
				case questID.COLLECT:
						break;
				default:
						break;
				}
				GetComponentInChildren<tk2dTextMesh> ().text = ";"; 
					

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
