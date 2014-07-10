using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;

public class Highscore : MonoBehaviour
{
		public bool isScore = false;
		public string secretKey = "12345";
		public string PostScoreUrl = "http://YouWebsite.com/.../postScore.php?";
		public string PostNameUrl = "http://YouWebsite.com/.../postScore.php?";
		public string GetHighscoreUrl = "http://YouWebsite.com/.../getHighscore.php";
		private string score = "Score";
//		private string WindowTitel = "";
		private string Scores = "";
		private string Names = "";
		private tk2dTextMesh scoreText;
		private string[] rankList = new string[50];
		public GUISkin Skin;
		public float windowWidth = 380;
		private float windowHeight = 300;
		public Rect windowRect;
		public int maxNameLength = 10;
		public int getLimitScore = 15;
		public GameObject loading, scoreList;
		readonly int NAME = 0;
		readonly int SCORE = 1;
	
		void Start ()
		{
				scoreText = GetComponent<tk2dTextMesh> ();
				StartCoroutine ("GetScore");		
		}

		void Update ()
		{
//				windowRect = new Rect (Screen.width / 2 - (windowWidth / 2), 40, windowWidth, Screen.height - 50);
//				windowHeight = Screen.height - 50;

		}
	
		IEnumerator GetScore ()
		{
				Scores = "";
			
//				WindowTitel = "Loading";
	
		
				WWWForm form = new WWWForm ();
				form.AddField ("limit", 50);
		
				WWW www = new WWW (GetHighscoreUrl, form);
				yield return www;
		
				if (www.text == "") {
						loading.GetComponent<tk2dTextMesh> ().text = "Network Error..";
						print ("There was an error getting the high score: " + www.error);
//						WindowTitel = "There was an error getting the high score";
						yield return new WaitForSeconds (1f);
						Application.LoadLevel (1);
				} else {
						loading.SetActive (false);
//						WindowTitel = "Done";
						textSplit (www.text);
						showRank (10);
//						Score = textSplit (www.text)[0];
				}
		}

		void showRank (int lastId)
		{
				Names = "";
				Scores = "";
				for (int i=lastId-10; i<lastId; i++) {

						string[] temp = rankList [i].Split (',');

						Names += (i + 1) + ". " + temp [NAME];
						if (i < lastId)
								Names += "\n";

						Scores += temp [SCORE];
						if (i < lastId)
								Scores += "\n";
				}


				scoreText.text = Names;
				scoreList.SendMessage ("showScores", Scores);
		}

		void postRank ()
		{
				StartCoroutine (PostScore (PlayerPrefs.GetString ("MYNAME", "noname"), PlayerPrefs.GetInt ("NUMGEM", 0)));
		}

		void postName ()
		{
				StartCoroutine (PostName (PlayerPrefs.GetString ("MYNAME", "noname"), PlayerPrefs.GetInt ("NUMGEM", 0)));
		}

		void textSplit (string www)
		{
				rankList = www.Split ('\n');
//		foreach( string human in rankList )
//		{
//			Debug.Log( human );
//		}
		}
	
		IEnumerator PostScore (string name, int score)
		{
				string _name = name;
				int _score = score;
		
				string hash = Md5Sum (_name + _score + secretKey).ToLower ();
		
				WWWForm form = new WWWForm ();
				form.AddField ("name", _name);
				form.AddField ("score", _score);
				form.AddField ("hash", hash);
		
				WWW www = new WWW (PostScoreUrl, form);
//				WindowTitel = "Wait";
				yield return www;
		
				if (www.text == "done") {
						//						StartCoroutine ("GetScore");
						GameObject.Find ("UI_MENU").SendMessage ("returnName", "OK");
				} else {
						//						print ("There was an error posting the high score: " + www.error);
						GameObject.Find ("UI_MENU").SendMessage ("returnName", www.error + "");
				}
		}

		IEnumerator PostName (string name, int score)
		{
				string _name = name;
				int _score = score;
		
				string hash = Md5Sum (_name + _score + secretKey).ToLower ();
		
				WWWForm form = new WWWForm ();
				form.AddField ("name", _name);
				form.AddField ("score", _score);
				form.AddField ("hash", hash);
		
				WWW www = new WWW (PostNameUrl, form);
//				WindowTitel = "Wait";
				yield return www;
		
				if (www.text == "done") {
						//						StartCoroutine ("GetScore");
						GameObject.Find ("UI_MENU").SendMessage ("returnName", "OK");
				} else {
						//						print ("There was an error posting the high score: " + www.error);
						GameObject.Find ("UI_MENU").SendMessage ("returnName", www.error + "");
				}
		}
	
//	void OnGUI()
//	{
//	GUI.skin = Skin;
//		
//		windowRect = GUI.Window(0, windowRect, DoMyWindow, WindowTitel);
//	
//		name = GUI.TextField (new Rect (Screen.width / 2 - 160, 10, 100, 20), name, maxNameLength);
//    	score = GUI.TextField (new Rect (Screen.width / 2 - 50, 10, 100, 20), score, 25);
//		
//    	if (GUI.Button(new Rect(Screen.width / 2 + 60, 10, 90, 20),"Post Score"))
//    	{
//			StartCoroutine(PostScore(name, int.Parse(score)));
//       		name = "";
//       		score = "";
//    	}    
//	}
	
		void DoMyWindow (int windowID)
		{
				GUI.skin = Skin;
		
				GUI.Label (new Rect (windowWidth / 2 - windowWidth / 2, 30, windowWidth, windowHeight), Scores);
    	
				if (GUI.Button (new Rect (15, Screen.height - 90, 70, 30), "Refresh")) {
						StartCoroutine ("GetScore");
				}         
		}
	
		public string Md5Sum (string input)
		{
				System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create ();
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes (input);
				byte[] hash = md5.ComputeHash (inputBytes);
 
				StringBuilder sb = new StringBuilder ();
				for (int i = 0; i < hash.Length; i++) {
						sb.Append (hash [i].ToString ("X2"));
				}
				return sb.ToString ();
		}
}
