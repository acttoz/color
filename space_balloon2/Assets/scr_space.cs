using UnityEngine;
using System.Collections;

public class scr_space : MonoBehaviour
{
		GameObject MGR;
		public int spaceId;
		float[] levels = new float[] {
				0,
				0.001f,
				0.002f,
				0.004f,
				0.008f,
				0.016f,
				0.032f,
				0.06f,
				0.1f,
				0.15f,
				0.2f,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				-0.03f
		};
		// Use this for initialization
		void Start ()
		{
				MGR = GameObject.Find ("GAMEMANAGER");
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position -= new Vector3 (0, levels [scr_manager.superLevel] / 2, 0);
				if (transform.position.y < -13 || transform.position.y > 13)
						Destroy (this.gameObject);

	
				if (transform.position.y < 5 && transform.position.y > -5) {
						if (Value.isQuest && Value.questNum == 2 && Value.questLevel == spaceId) {
								Value.questLevel++;
								MGR.SendMessage ("timeOut");
				Debug.Log("++");
								
						}
				}
		}

//		void changeSpeed ()
//		{
//				level = levels [scr_manager.superLevel];
//		}


}
