using UnityEngine;
using System.Collections;

public class Sinus : MonoBehaviour {
	public float StartAngle;
	public float Sinusdistance=0.5f;
	public float SinusSpeed=100f;
	public float SinusSpeedoffset=0f;
	public float SinX;

	float a = 0;
	float MSinX=0;

	void Start () 
	{
		a = StartAngle;
		SinX = 0;
	}
	
	void FixedUpdate () 
	{
		a+=Time.deltaTime*(SinusSpeed+SinusSpeedoffset);
		a = a % 360.0f;
		MSinX = SinX;
		SinX=Sinusdistance*Mathf.Sin (a*Mathf.PI/180.0f);
		gameObject.transform.Translate (new Vector3 (0, SinX-MSinX, 0f));

		
	}
}
