using UnityEngine;
using System.Collections;

public class scr_space : MonoBehaviour
{
	float[] levels = new float[]{0,0.001f,0.002f,0.004f,0.008f,0.016f,0.032f,0.06f,0.1f,0.2f,0.4f,11,12,13,14,15,16,17,18,19,-0.03f};
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
			transform.position -= new Vector3 (0, levels [scr_manager.superLevel]/2, 0);
				if (transform.position.y < -13 || transform.position.y > 13)
						Destroy (this.gameObject);
		}

//		void changeSpeed ()
//		{
//				level = levels [scr_manager.superLevel];
//		}


}
