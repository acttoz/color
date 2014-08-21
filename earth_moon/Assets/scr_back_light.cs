using UnityEngine;
using System.Collections;

public class scr_back_light : MonoBehaviour
{
		float[] levels = new float[] {
				0,
				0.01f,
				0.03f,
				0.1f,
				0.2f,
				0.4f,
				0.6f,
				0.6f,
				1.2f,
				1.8f,
				2f,
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
		public Sprite temp;
		bool gogo = false;
		// Use this for initialization
		void Start ()
		{
//				float temp = Random.Range (8, 14) / 10;
//				transform.localScale = new Vector2 (temp, temp);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (gogo) {
						transform.position -= new Vector3 (0, levels [scr_manager.superLevel], 0);
						if (transform.position.y < -8 || transform.position.y > 8)
								Destroy (this.gameObject);
						if (scr_manager.superLevel == 20)
								GetComponent<SpriteRenderer> ().sprite = temp;
				}
		}
	void gogogo(){
		gogo = true;
	}
//		void changeSpeed ()
//		{
//				level = levels [scr_manager.superLevel];
//		}


}
