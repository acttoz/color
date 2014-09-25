using UnityEngine;
using System.Collections;

public class StarPop : MonoBehaviour
{
		public int starNum;
		// Use this for initialization
		void Start ()
		{
				if (STATE.stars > starNum)
						animation.Play ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}


}
