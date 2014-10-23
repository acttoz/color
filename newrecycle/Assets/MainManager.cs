using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour
{
	public GameObject mSchoolGrid, mAnchor, mGradeGrid, mClassGrid, mRankGrid, mRank, mInputName, mInputSchool, mSchoolLabel;
	string listFlag;
	string mChooseItem;
	string result;
	string resultSchool;
	// Use this for initialization
	void Start ()
	{
		
		
		resultSchool = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (PlayerPrefs.GetString ("SCHOOL")));
		string resultGrade = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (PlayerPrefs.GetString ("GRADE")));
		string resultClass = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (PlayerPrefs.GetString ("CLASS")));
		string resultName = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (PlayerPrefs.GetString ("NAME")));
		Debug.Log (resultSchool + resultGrade + resultClass + resultName);
		mInputName.SetActive (false);
		mInputSchool.SetActive (false);	
		if (PlayerPrefs.HasKey ("NAME") && PlayerPrefs.HasKey ("SCHOOL")) {
			onRankGrid ();
			mSchoolLabel.GetComponent<UILabel> ().text = resultSchool;
		} else {
			onInputSchool ();
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	void PopUp (string chooseItem)
	{
		mChooseItem = chooseItem;
		mAnchor.SendMessage ("popup", mChooseItem);
		Debug.Log (chooseItem);
		
		if (string.Equals ("name", listFlag))
			mInputName.SetActive (false);
	}
	
	void yesButton ()
	{
		switch (listFlag) {
		case "school":
			result = System.Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (mChooseItem));
			createTable (mChooseItem);
			PlayerPrefs.SetString ("SCHOOL", result);
			PlayerPrefs.Save ();
			gameObject.SendMessage ("ResetPosition");
			gameObject.SendMessage ("onGradeGrid");
			break;
		case "grade":
			result = System.Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (mChooseItem));
			PlayerPrefs.SetString ("GRADE", result);
			PlayerPrefs.Save ();
			gameObject.SendMessage ("ResetPosition");
			gameObject.SendMessage ("onClassGrid");
			break;
		case "class":
			result = System.Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (mChooseItem));
			PlayerPrefs.SetString ("CLASS", result);
			PlayerPrefs.Save ();
			gameObject.SendMessage ("ResetPosition");
			gameObject.SendMessage ("inputName");	
			break;
		case"name":
			result = System.Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes (mChooseItem));
			resultSchool = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (PlayerPrefs.GetString ("SCHOOL")));
			PlayerPrefs.SetString ("NAME", result);
			PlayerPrefs.Save ();
			mInputName.SetActive (false);
			onRankGrid ();
			mSchoolLabel.GetComponent<UILabel> ().text = resultSchool;
			break;
			
		}
	}

	void noButton ()
	{
		if (string.Equals ("name", listFlag))
			mInputName.SetActive (true);
		
	}
	
	void searchSchool (string keyWord)
	{
		onSchoolGrid (keyWord);
		listFlag = "school";
	}
	
	void onInputSchool ()
	{
		mInputSchool.SetActive (true);	
	}
	
	void onRankGrid ()
	{
		mRankGrid.SetActive (true);
		mRank.SendMessage ("DownloadData", resultSchool);
	}

	void onSchoolGrid (string selectedLocal)
	{
		listFlag = "school";
		mInputSchool.SetActive (false);
		mSchoolGrid.SetActive (true);
		mSchoolGrid.SendMessage ("DownloadData", selectedLocal);
	}

	void onGradeGrid ()
	{
		listFlag = "grade";
		mSchoolGrid.SetActive (false);
		mGradeGrid.SetActive (true);
		mGradeGrid.SendMessage ("InitItem");
	}
	
	void onClassGrid ()
	{
		listFlag = "class";
		mGradeGrid.SetActive (false);
		mClassGrid.SetActive (true);
		mClassGrid.SendMessage ("InitItem");
	}
	
	void inputName ()
	{
		listFlag = "name";
		mClassGrid.SetActive (false);
		mInputName.SetActive (true);	
	}
	
	public void createTable (string schoolName) // 데이터 갱신하는 역할입니다.
	{ 
		string url = "http://actoz.dothome.co.kr/score.php";
//아까 말씀드린 파싱/데이터 입력 함수가 한번만 실행되게하려고 만든 플래그에요. 
//True로 바꿔서 실행될 여지가 생기도록 해줍니다.

		WWWForm form = new WWWForm ();
		form.AddField ("select", "CreateTable");
		form.AddField ("name", schoolName);
		WWW www = new WWW (url, form);
		WaitWWW (www);
// 똑같은 걸 똑같이 자꾸 적는건 좀 아닌것같아요. 위의 폼만들고 www 불러주고...이하 같습니다.
      
	}

	IEnumerator WaitWWW (WWW www)
	{
		yield return www;
	}
}
