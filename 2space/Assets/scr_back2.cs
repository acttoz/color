using UnityEngine;
using System.Collections;

public class scr_back2 : MonoBehaviour
{
	float[] levels = new float[]{0,0.01f,0.03f,0.1f,0.2f,0.4f,0.6f,1,1,1,1,11,12,13,14,15,16,17,18,19,0f};
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Level.instance.superLevel < 6 || Level.instance.superLevel == 20) {
			transform.position -= new Vector3 (0, levels [Level.instance.superLevel], 0);
		} else {
			transform.position -= new Vector3 (0, levels [Level.instance.superLevel - 5], 0);
		}
		if (transform.position.y < -20)
			Destroy (this.gameObject);
	}
	
	//		void changeSpeed ()
	//		{
	//				level = levels [Level.instance.superLevel];
	//		}
	
	
}
