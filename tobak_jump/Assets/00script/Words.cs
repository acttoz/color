using UnityEngine;
using System.Collections;

public class Words : MonoBehaviour
{
		public GameObject parentWords;
		public GameObject back;
		public GameObject prf_word;
		public static Words mInstance;
		int randomWord;
		int randomAnswer;
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
				randomWord = Random.Range (0, 100);
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
	
		string[] tobak = {
		"가늠",
		"수",
		"나라임자",
		"마하늬새노",
		"굳히기",
		"솜씨",
		"말글",
		"도르리",
		"땅덩어리",
		"발표",
		"뽑다",
		"삯",
		"채",
		"스승",
		"겨루기",
		"알맹이",
		"집무리",
		"집채",
		"-새",
		"도움",
		"결",
		"품삯",
		"구미",
		"이때",
		"나중때",
		"그릴 ~",
		"알속",
		"흐벅몸매",
		"적바림",
		"솜씨",
		"얼개",
		"써 넣다",
		"써 넣다",
		"내는 때",
		"할 일",
		"사위",
		"~다움",
		"나눠내기",
		"따짐",
		"부",
		"녈음지이",
		"녈음지기",
		"되녈음지기",
		"되녈음지이",
		"보람",
		"지기",
		"그루갈이",
		"멋있다",
		"겹셈",
		"마땅히",
		"여러말 할것 없이",
		"터",
		"맛뵈기",
		"여러말 할것 없이",
		"돐",
		"센바람",
		"값 불리기",
		"두집살림",
		"된판",
		"띠",
		"방죽길",
		"강턱",
		"뒤집기",
		"투정",
		"때",
		"또이름",
		"변말",
		"짜임",
		"느낌",
		"적발하다",
		"목숨",
		"주먹질",
		"설밑모임",
		"들머리",
		"알림매",
		"멤돌이별",
		"모양새",
		"무리켜",
		"말무리",
		"마땅히",
		"분무기",
		"~처럼 되다",
		"흥정",
		"밑얼개",
		"맞흥정",
		"잣대",
		"곶",
		"곁먹거리",
		"썩다",
		"헤살",
		"제자",
		"흰밥",
		"벌",
		"말옮김",
		"뒷간",
		"한집살림",
		"갖춤",
		"보람",
		"보람"
	};
		string[] korean = {
		"테스트",
		"방법",
		"국민",
		"동서남북",
		"확인",
		"기술",
		"언어",
		"포트락",
		"대륙",
		"프리젠테이션",
		"프린트",
		"비용",
		"세대",
		"멘토",
		"대결",
		"컨텐츠",
		"건축물",
		"건물",
		"현상",
		"서포트",
		"품질",
		"임금",
		"만",
		"현재",
		"미래",
		"그래픽",
		"핵",
		"글래머",
		"메모",
		"기능",
		"시스템",
		"기재하다",
		"기입하다",
		"납기",
		"이벤트",
		"터부",
		"정체성",
		"더치페이",
		"논리",
		"팀",
		"농사",
		"농부",
		"귀농인",
		"귀농",
		"효과",
		"관리자",
		"이모작",
		"쿨하다",
		"복수",
		"물론",
		"도대체",
		"부지",
		"트라이얼",
		"대관절",
		"주기",
		"태풍",
		"재테크",
		"별거",
		"사태",
		"대",
		"윤중로",
		"고수부지",
		"패러독스",
		"땡깡",
		"시각",
		"아이디",
		"은어",
		"코드",
		"랑만",
		"노트",
		"운명",
		"폭력",
		"송년회",
		"로비",
		"매체",
		"행성",
		"스타일",
		"계층",
		"조류",
		"당연히",
		"스프레이",
		"-화",
		"네고",
		"플랫폼",
		"직거래",
		"기준",
		"반도",
		"사이드메뉴",
		"부패",
		"태클",
		"멘티",
		"백반",
		"버전",
		"통역",
		"화장실",
		"동거",
		"설비",
		"노고",
		"태그"
	};
	
}
