using UnityEngine;
using System.Collections;

public class SeGametResolution : MonoBehaviour
{
	public int nPerWidth = 640;
	public int nPerHeight = 960;
	
	
	
	// Use this for initialization
	void Start ()
	{
		ResoultionView ();
	}
	
	void ResoultionView ()
	{
		Vector2 res = new Vector2 (Screen.width / nPerWidth, Screen.height / nPerHeight);
		
		if (res.x < res.y) {
			
			float fH = (float)nPerHeight / (float)nPerWidth * Screen.width / Screen.height;
			float fY = (1 - fH) * 0.5f;
			
			Camera.main.rect = new Rect (0, fY, 1, fH);
		} else if (res.x > res.y) {
			float fY = (float)nPerWidth / (float)nPerHeight * Screen.height / Screen.width;
			float fH = (1 - fY) * 0.5f;
			
			Camera.main.rect = new Rect (fY, 0, fH, 1);
		} else {
			
		}
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}
