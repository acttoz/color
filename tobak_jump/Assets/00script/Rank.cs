using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;

public class Rank : MonoBehaviour
{
		public bool isScore = false;
		string secretKey = "0453";
		string PostScoreUrl = "http://actoze.dothome.co.kr/tobak/postScore.php?";
		string PostNameUrl = "http://actoze.dothome.co.kr/tobak/postName.php?";
		string GetHighscoreUrl = "http://actoze.dothome.co.kr/tobak/getHighscore.php";
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
		string name;
		int score;
	
		void Start ()
		{
					
		}

		void Update ()
		{

		}

		public Rank (int score, string name)
		{
				this.score = score;
				this.name = name;
		}

		IEnumerator GetScore ()
		{
				Scores = "";
			
//				WindowTitel = "Loading";
	
		
				WWWForm form = new WWWForm ();
				form.AddField ("limit", 100);
		
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

						Names += (i + 1) + ". " + temp [0];
						if (i < lastId)
								Names += "\n";

						Scores += temp [1];
						if (i < lastId)
								Scores += "\n";
				}


				scoreText.text = Names;
				scoreList.SendMessage ("showScores", Scores);
		}

		public void postRank ()
		{
				StartCoroutine ("PostScore");
		}

		void textSplit (string www)
		{
				rankList = www.Split ('\n');
		}
	
		public IEnumerator PostScore ()
		{
		
				string hash = Md5Sum (name + score + secretKey).ToLower ();
		
				WWWForm form = new WWWForm ();
				form.AddField ("name", name);
				form.AddField ("score", score);
				form.AddField ("hash", hash);
		
				WWW www = new WWW (PostScoreUrl, form);
//				WindowTitel = "Wait";
				yield return www;
		
				if (www.text == "done") {
						Debug.Log ("done");
				} else {
						Debug.Log ("networkError");
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
