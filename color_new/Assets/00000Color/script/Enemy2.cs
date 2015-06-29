using UnityEngine;
using System.Collections;

public class Enemy2 : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void thiefAttack ()
		{
				transform.parent.gameObject.SendMessage ("thiefAttack");
				Destroy (this.gameObject);
		}

		void thiefOff ()
		{
				transform.parent.gameObject.SendMessage ("thiefOff");
				Destroy (this.gameObject);
		}
}
