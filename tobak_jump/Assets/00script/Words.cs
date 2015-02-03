using UnityEngine;
using System.Collections;

public class Words : MonoBehaviour
{
		public GameObject parentWords;
		public GameObject prf_word;
		public static Words mInstance;
		int randomWord;
		int randomAnswer;
		string[] tobak = {"토박1","토박2","토박3","토박4","토박5"};
		string[] korean = {"들온말1","들온말2","들온말3","들온말4","들온말5"};
		public int[] answer4 = {0,0};
		public int[] answer3 = {0,0};
		public int[] answer2 = {0,0};
		public int[] answer1 = {0,0};
		public string[] word4 = new string[2];
//		public string[] word3 = new string[2];
//		public string[] word2 = new string[2];
//		public string[] word1 = new string[2];
		public GameObject[] oWords = new GameObject[4];
		public Transform target;
		int wordsPosition = 0;
		// Use this for initialization
		public Words ()
		{
				mInstance = this;
		}
	
		public static Words instance {
				get {
						if (mInstance == null)
								new Words ();
						return mInstance;
				}
		}

		public void reset ()
		{
				
				StartCoroutine ("initGame");
		}

		IEnumerator initGame ()
		{
				answer4 = new int[]{0,0};
				answer3 = new int[]{0,0};
				answer2 = new int[]{0,0};
				answer1 = new int[]{0,0};
				wordsPosition = 0;
				parentWords.transform.position = new Vector3 (0, 0, 0);
				GameObject[] temp2;
				temp2 = GameObject.FindGameObjectsWithTag ("word");
				for (int i=0; i<temp2.Length; i++) {
						Destroy (temp2 [i]);
				}
				for (int i=0; i<3; i++) {
						createWord ();
						wordsPosition -= 4;
						moveAnswer ();
						parentWords.transform.position = new Vector3 (0, wordsPosition, 0);
			
						yield return new WaitForSeconds (0.01f);
				}
		}

		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		//ENEMY
		public void createWord ()
		{
//				for (int i = 1; i < 4; i++)
//						oWords [i - 1] = oWords [i];
				answer4 [0] = 0;
				answer4 [1] = 0;
				randomAnswer = Random.Range (0, 2);
				randomWord = Random.Range (0, 5);
				answer4 [randomAnswer] = 1;
				
				if (randomAnswer == 0) {
						word4 [0] = tobak [randomWord];
						word4 [1] = korean [randomWord];
				} else {
						word4 [0] = korean [randomWord];
						word4 [1] = tobak [randomWord];
				}
				oWords [3] = Instantiate (prf_word, new Vector2 (0, 9), Quaternion.identity) as GameObject;
				oWords [3].transform.parent = GameObject.Find ("Words").transform;
		}


		public void jump (int jumpDirection)
		{
				if (MANAGER.instance.score == 0) {
						if (jumpDirection == answer1 [1])//right answer
								right ();
						else
								wrong ();
				} else {
						createWord ();

						wordsPosition -= 4;
						Vector3 pointB = new Vector3 (0, wordsPosition, 0);


						moveAnswer ();

						StartCoroutine (MoveObject (parentWords.transform, parentWords.transform.position, pointB, 0.1f, jumpDirection));
				}
				//				answer3 [0] = answer4 [0];
				//				answer3 [1] = answer4 [1];
		
		}

		public void moveAnswer ()
		{
				

				answer1 [0] = answer2 [0];
				answer1 [1] = answer2 [1];
				answer2 [0] = answer3 [0];
				answer2 [1] = answer3 [1];
				answer3 [0] = answer4 [0];
				answer3 [1] = answer4 [1];

		}
	
		IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time, int jumpDirection)
		{
				yield return new WaitForSeconds (0.1f);
				float i = 0.0f;
				float rate = 1.0f / time;
				while (i < 1.0f) {
						i += Time.deltaTime * rate;
						thisTransform.position = Vector3.Lerp (startPos, endPos, i);
						yield return null; 
				}
				if (jumpDirection == answer1 [1])//right answer
						right ();
				else
						wrong ();


		}

		void right ()
		{
				Player.instance.wasSuccess = true;
		}

		void wrong ()
		{
				Player.instance.wasSuccess = false;
		}
	
		public void InitEnemy ()
		{
		}
	
}
