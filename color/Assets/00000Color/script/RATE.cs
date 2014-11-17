using UnityEngine;
using System.Collections;

public class RATE : MonoBehaviour
{
		public static int colorPlusRate;
		public static int colorMinusRate;
		public static float bossInitRate;
		public static float bossAttackRate;
		public int _colorPlusRate;
		public int _colorMinusRate;
		public float _bossInitRate;
		public float _bossAttackRate;
		// Use this for initialization
		void Start ()
		{
				colorPlusRate = _colorPlusRate;
				colorMinusRate = _colorMinusRate;
				bossInitRate = _bossInitRate;
				bossAttackRate = _bossAttackRate;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
