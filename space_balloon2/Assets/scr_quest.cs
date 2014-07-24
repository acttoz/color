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
		public Sprite[] spaces;
		public GameObject oCondition, oPoint, oSuccess;
		private int level, questNum;
		int[] conLevel;
		int[] conPoint;
		private tk2dTextMesh[] texts;

		void Start ()
		{
				switch (quest) {
				case questID.SHOT: 
		
						level = PlayerPrefs.GetInt ("QUEST1", 0);
						break;
				case questID.SPACE:
						level = PlayerPrefs.GetInt ("QUEST2", 0);
			Debug.Log(level+"");
						oCondition.GetComponent<SpriteRenderer> ().sprite = spaces [level];
						break;
				case questID.KILL:
						level = PlayerPrefs.GetInt ("QUEST3", 0);
						break;
				case questID.ALIVE1:
						level = PlayerPrefs.GetInt ("QUEST4", 0);
						break;
				case questID.ALIVE2:
						level = PlayerPrefs.GetInt ("QUEST5", 0);
						break;
				case questID.COLLECT:
						level = PlayerPrefs.GetInt ("QUEST6", 0);
						break;
				default:
						break;
				}
				conLevel = Value.quests [(int)quest];	
				conPoint = Value.questGem;

				if (quest != questID.SPACE && quest != questID.ALIVE1 && quest != questID.ALIVE2)
						oCondition.GetComponent<tk2dTextMesh> ().text = conLevel [level] + "";
				
				oPoint.GetComponent<tk2dTextMesh> ().text = conPoint [level] + "";

				if (level == 5) {
						//master
						oSuccess.SetActive (true);
						GetComponent<BoxCollider> ().enabled = false;
				}

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
 
}
