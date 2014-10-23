using UnityEngine;
using System.Collections;

public class Rank : MonoBehaviour
{
//	public Score[] score = new Score[10];
//위에서 선언한 스코어 클래스를 10개 할당해놓았는데.
//1위~10위까지 표시되는 라벨을 각각 참조합니다.

//	public Score summitscore;
// 이건 인풋박스로 입력하는 부분있잖아요? 그 부분의 라벨을 참조합니다.
 
	UILabel scoreLabel;
	
	
//위의 3개릐 라벨은, host랑 file명을 입력하면 해당주소를 url로 해서 www를 사용하도록 하려고 선언한거에요.
//원래같으면 필요 없는 부분이에요. url라벨은 표시해서 보여주기 위해서 있어요.
 
	string url = "";
// url은 이곳에 담깁니다. 

	public WWW www; 
//www를 사용하기 위해 
	public bool checkStart = false;
// 데이터를 파싱해서 적용하는 함수가 한번만 싫행되도록하기 위한 플래그에요.
//매 프레임 돌게하면 간편하고, 바로바로 최신화 안되는 문제도 없어지는데... 
//매프레임돌게 하는건 일단 아닌것같아서...

	public void Start ()
	{  
		
		
	}
//스타트에서는 SetURL 함수가 실행되서, URL를 세팅합니다.
 
	public void SetURL ()
	{  
		url = "http://actoz.dothome.co.kr/score.php";
	}
//SetURL은 위에서 말씀드린대로 Host / File 라벨을 가지고 URL을 구성하고.
//URL라벨에 url을 표시해요.

	IEnumerator WaitWWW (WWW www)
	{
		yield return www;
	}
//이건 유니티 www 클래스 스크립트 레퍼런스 예제에도 나와있고.
//www  생성후에 이 함수를 호출안해주면 제대로 안되더라구요...
//제 생각에는 다운로드를 기다리는 역할이라고 생각이 되요.
//저도 배우는 입장이여서 IEnumerator가 무엇인지 확실하게 모르겠습니다.
//아시분은 가르쳐주세요~.
 
 
	
 
	public void DownloadData (string schoolName) // 데이터 갱신하는 역할입니다.
	{ 
		SetURL ();
		scoreLabel = GetComponent<UILabel> ();
		checkStart = true;
//아까 말씀드린 파싱/데이터 입력 함수가 한번만 실행되게하려고 만든 플래그에요. 
//True로 바꿔서 실행될 여지가 생기도록 해줍니다.

		WWWForm form = new WWWForm ();
		form.AddField ("select", "show");
		form.AddField ("school", schoolName);
		www = new WWW (url, form);
		WaitWWW (www);
// 똑같은 걸 똑같이 자꾸 적는건 좀 아닌것같아요. 위의 폼만들고 www 불러주고...이하 같습니다.
      
	}
 
	public void SettingDownloadData () //데이터 파싱하고, 라벨에 입력하는함수에요.
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		string mTempRank;
		Debug.Log (www.text);
		string[] data = www.text.Split ('&'); 
// 위의 DownloadData()함수가 php를 호출해서 echo로 찍어놓은걸.
//&로 짜르면 레코드 단위로 짤리게 됩니다. 짤라서 선언한 1차원 배열에 집어넣고요.
  
		string[][] row = new string[data.Length][];
//2차원 배열을 선언하는데, 위에서 선언한 1차원배열의 레코드의 갯수(=data.Length) 만큼 할당합니다.
    
		for (int i=0; i<data.Length; i++) {
			row [i] = data [i].Split ('?');
		}
// for문을 돌면서 ?단위로 짜릅니다. 순서대로 필드단위로 데이터가 들어가게 되요. 
		row [0] [0] = row [0] [0].ToString ().Substring (69, 1);
//이제 데이터가 배열에 다 들어와있습니다.
//가령 row[2][2] 라면, 3번째순위인 레코드의 Point 필드가 들어와있는식으로요.
		for (int i =0; i< row.Length-1; i++) {
			if (checkStart) {
				if (i < 9) {
					sb.AppendLine ((i + 1).ToString () + ".  " + row [i] [0] + "학년 " + row [i] [1] + "반 " + row [i] [2] + " 기록:" + row [i] [3]);
				} else {
					sb.AppendLine ((i + 1).ToString () + ". " + row [i] [0] + "학년 " + row [i] [1] + "반 " + row [i] [2] + " 기록:" + row [i] [3]);
					
				}
					
				//				score [i].nameLabel.text = row [i] [0];
//				score [i].distanceLabel.text = row [i] [1];
//				score [i].pointLabel.text = row [i] [2];
				
    
			}
		}
		
//for문을 돌면서 라벨에 데이터를 집어넣어줍니다.
		scoreLabel.text = sb.ToString ();

		checkStart = false;
//플래그를 꺼서, 한번만 실행되도록 해줍니다.
	}
 
	// Update is called once per frame
	void Update ()
	{
		if (checkStart) {
//업데이트문을 돌면서 checkstart가 true이기만을 체크하고있습니다.
//true가 되는건 DownloadData()함수가 실행됬을 때 뿐인데요.
//그때부터 www함수의 멤버젼수중 하나인 isdone이 True가 될때까지체크하게 됩니다. 
			if (www.isDone) {
// isDone은 다운로드가 완료됬는지 여부에요..(그럼 Waitwww 함수는...?!).
				SettingDownloadData ();
//데이터를 파싱하고 라벨에 입력하는 함수를 실행합니다.
//위에서 말씀드렷듯이 함수 끝에 checkStart를 꺼주기 때문에.
//한번만 실행되고, 더는 실행되지 않아요.

			}
		}
	}
}