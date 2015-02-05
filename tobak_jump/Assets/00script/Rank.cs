using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;

public class Rank : MonoBehaviour
{
		public bool isScore = false;
		public string secretKey = "0453";
		public string PostScoreUrl;
		public string GetHighscoreUrl;
//		private string WindowTitel = "";
		private string rankText = "";
		private tk2dTextMesh scoreText;
		private string[] rankList;
	
		void Start ()
		{
				scoreText = GetComponent<tk2dTextMesh> ();
		}

		void Update ()
		{

		}

		public void RankShow ()
		{
		
				StartCoroutine ("GetScore");
		}

		public Rank (int score, string name)
		{
				
				StartCoroutine ("PostScore");
		}

		IEnumerator GetScore ()
		{
			
//				WindowTitel = "Loading";
	
		
				WWWForm form = new WWWForm ();
				form.AddField ("limit", 100);
		
				WWW www = new WWW (GetHighscoreUrl, form);
				yield return www;
//				scoreText.text = www.text;
				rankList = www.text.Split ('\n');
//				Debug.Log (www.text);
				showRank ();
		}

		void showRank ()
		{
				rankText = "";
				for (int i=0; i<rankList.Length-1; i++) {
						string[] temp = rankList [i].Split (',');
			Debug.Log(temp[1]);

						rankText += (i + 1) + ".   " + temp [0] + "     " + temp [1] + "\n";

				}


				scoreText.text = rankText;
		}

		public void postRank (string name, int score)
		{
				StartCoroutine (PostScore (name, score));
		}
	
		IEnumerator PostScore (string name, int score)
		{
		
				WWWForm form = new WWWForm ();
				form.AddField ("name", name);
				form.AddField ("score", score);
		
				WWW www = new WWW (PostScoreUrl, form);
//				WindowTitel = "Wait";
				yield return www;
				MANAGER.instance.toast (2);
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
