using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
		private int level;
		public static Level instance;
		// Use this for initialization
		void Start ()
		{
				instance = this;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void setLevel (int num)
		{
				level = num;
		}

		public int  getLevel ()
		{
				return level;
		}
}
