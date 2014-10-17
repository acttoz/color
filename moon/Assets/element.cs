using UnityEngine;
using System.Collections;

public class element : MonoBehaviour
{
		public int id;
		// Use this for initialization
		public Sprite[] sprites;

		void Start ()
		{
				angle3d ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void angle3d ()
		{
				transform.rotation = Quaternion.Euler (345.5666f, 336.3143f, 6.929811f);
		}

		void angle2d ()
		{
				transform.rotation = Quaternion.Euler (0, 0, 0);
		}

		void Tab1 ()
		{

				GetComponent<SpriteRenderer> ().sprite = sprites [0];

		
		}

		void Tab2 ()
		{
		
				GetComponent<SpriteRenderer> ().sprite = sprites [1];
		
		
		}

		void Tab3 ()
		{
		
				
		
		
		}

		void Horizontal1 ()
		{
		}

		void Horizontal2 ()
		{
		}

		void Vertical1 ()
		{
		}

		void Vertical2 ()
		{
		}

		void Highlight1 ()
		{
		}

		void Highlight2 ()
		{
		}
}
