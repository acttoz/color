using UnityEngine;
using System.Collections;

public class Value : MonoBehaviour
{
//		public static int[] quest1 = new int[]{6,8,10,12,14,16,18,20};
//		public static int[] quest2 = new int[]{};
//		public static int[] quest3 = new int[]{2,3,4,5,6,7,8,9};
//		public static int[] quest4 = new int[]{12,16,20,22,24,26,28,30};
//		public static int[] quest5 = new int[]{3,4,5,6,7,8,9,10};
//		public static int[] quest6 = new int[]{7,12,17,22,25,30,32,35};
		public static int[][] quests = new int[][] {
		new int[]{3,5,7,9,11,13,15,17},
		new int[]{},
		new int[]{1,2,3,4,5,6,7,8},
		new int[]{12,16,20,22,24,26,28,30},
		new int[]{3,4,5,6,7,8,9,10},
		new int[]{7,12,17,22,25,30,32,35}
		};
		public static int[] questGem = new int[]{10,20,30,40,50,70,100,200};
	public static int questNum=0;
	public static int questLevel=0;
	public static bool isQuest=false;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
