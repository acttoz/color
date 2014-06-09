using UnityEngine;
using System.Collections;

public class scr_space : MonoBehaviour
{
	float[] levels = new float[]{0,0.0001f,0.0003f,0.001f,0.002f,0.004f,0.06f,0.001f,0.003f,0.01f,0.02f,0.04f,11,12,13,14,15,16,17,18,19,-0.03f};
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
