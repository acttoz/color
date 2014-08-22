using UnityEngine;using System.Collections;

public class Game_Manager: MonoBehaviour
{
	public GameObject blueBox, moon, particle, mExplosion, planeEnergy, energyBar, backGround, gaugeX1, x1sprite;
	public GameObject bottle, paper, can, pet, mTorch, _OnSuccess, _screenPause;
	public Color color;
	public Color scoreColor;
	public ArrayList trashList = null;
	public static  float  trashSpeed = 12f;
	public static int hitNum = 5;
	public static string firstTrash = "none";
	int trashNum = 1;
	int mixNum;
	GameObject  wrongScreen;
	int trashId;
	float superNum = 1;
	int superLevel = 0;
	int seconds = 0;
	int mScore = 0;
	int mScoreCount = 0;
	private TextMesh textMesh;
	private TextMesh mScoreTextMesh;
	private TextMesh mHighScoreTextMesh;
	private bool onPlay = false;
	// Use this for initialization
	void Start ()
	{
		
		wrongScreen = GameObject.Find ("Sprite wrong screen");
		gaugeX1.animation.Stop ();
		
//		gaugeX1.animation.Play ("x2");
//		energy.GetComponent<Physics>.gravity = new Vector3(9.81f,0f,0f);
//		mTorch = GameObject.Find ("myTorch");
		mTorch.SetActive (false);
		
		trashList = new ArrayList ();
		moon.animation.Play ("ready");
		bottle.name = trashNum.ToString ();
		paper.name = trashNum.ToString ();
		can.name = trashNum.ToString ();
		pet.name = trashNum.ToString ();
		mixNum = Random.Range (0, 6);
		GameObject.Find ("Ready").SetActive (true);
		
//		energyBar.transform.localScale = new Vector3 (0.2f, 0.4516919f, 2.283373f);
		
		//타이머 
		
		textMesh = GameObject.Find ("TimerText").GetComponent <TextMesh> ();
		GameObject.Find ("TimerText").GetComponent <MeshRenderer> ().material.color = color;
		     
		
		
		
		textMesh.text = seconds.ToString ();
		
		
		
	}
	
	void onStart ()
	{
		
		wrongScreen.active = false;
		wrongScreen.GetComponent<tk2dSprite> ().color = new Color (1, 1, 1, 0.8f);
		_screenPause.SetActive (false);
		gaugeX1.animation.Play ();
		GameObject.Find ("landSprite").animation.Play ();
		GameObject.Find ("trashbags").animation.Play ();
		StartCoroutine ("PlayStart");
		GameObject.Find ("Ready").SetActive (false);
	}
	
	IEnumerator PlayStart ()
	{
		yield return new WaitForSeconds (0.8f);
		onPlay = true;
		createTrash ();
		InvokeRepeating ("count", 0.1f, 0.1f);
	}
	
	void count ()
	{
		seconds += 1;
		textMesh.text = (seconds).ToString ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (energyBar.transform.localScale.x > 0.8555521f && !_OnSuccess.active) {
			onSuccess ();	
			
		}
		if (TrashBox.trashes.Length < 1 && onPlay) {
			
			//열혈모드!!
			switch (superLevel) {
			case 0:
				gaugeX1.animation ["x2"].speed = 2f;
				x1sprite.SendMessage ("Change", "x2");
				superNum = 2;
				Trashes.difficulty = 1;
				mTorch.GetComponent<ParticleSystem> ().startColor = new Color (255, 255, 255);
				break;
			case 1:
				x1sprite.SendMessage ("Change", "x3");
				gaugeX1.animation ["x2"].speed = 3f;
				superNum = 3;
				Trashes.difficulty = 2;
				mTorch.GetComponent<ParticleSystem> ().startColor = new Color (255, 0, 255);
				break;
			case 2:
				x1sprite.SendMessage ("Change", "x4");
				gaugeX1.animation ["x2"].speed = 4f;
				superNum = 4;
				Trashes.difficulty = 3;
				mTorch.GetComponent<ParticleSystem> ().startColor = new Color (0, 255, 255);
				break;
				
				
			
			}
			backGround.GetComponent<tk2dSprite> ().color = new Color (0, 0, 0);
			mTorch.SetActive (true);
			superLevel++;
			
			trashSpeed = 16f;
			createTrash ();
			
		}
		
		if (wrongScreen.active) {
			if (hitNum < 1) {
				wrongScreen.SetActive (false);
				wrongText.SetActive(false);
				hitNum = 5;
			}
		}
		
	}
	
	void createTrash ()
	{
		
		if (mixNum < 1) {
			
		trashId = Random.Range (0, 4);
			//테스트용
//			trashId = 0;
			
			
			mixNum = mixNum = Random.Range (0, 3);
		}
		
		bottle.name = trashNum.ToString ();
		paper.name = trashNum.ToString ();
		can.name = trashNum.ToString ();
		pet.name = trashNum.ToString ();
		mixNum--;
		trashNum ++;
				
		Debug.Log ("mix" + mixNum);
		Debug.Log ("trashId" + trashId);
				
		//테스트용
//				Instantiate (bottle);
//				trashList.Add ("bottle");
		GameObject childObject;	
				
		switch (trashId) {
		case 0:
					
			childObject = Instantiate (bottle, new  Vector3 (0.001f, 0.6f, -3f), Quaternion.identity)as GameObject;
			childObject.transform.parent = GameObject.Find ("parentTrash").transform;
			trashList.Add ("bottle");
			break;
		case 1:
			childObject = Instantiate (paper, new  Vector3 (0.001f, 0.6f, -3f), Quaternion.identity)as GameObject;
			childObject.transform.parent = GameObject.Find ("parentTrash").transform;
			trashList.Add ("paper");
			break;
		case 2:
			childObject = Instantiate (can, new  Vector3 (0.001f, 0.6f, -3f), Quaternion.identity)as GameObject;
			childObject.transform.parent = GameObject.Find ("parentTrash").transform;
			trashList.Add ("can");
			break;
		case 3:
			childObject = Instantiate (pet, new  Vector3 (0.001f, 0.6f, -3f), Quaternion.identity)as GameObject;
			childObject.transform.parent = GameObject.Find ("parentTrash").transform;
			trashList.Add ("pet");
			break;
		default:
			childObject = Instantiate (can, new  Vector3 (0.001f, 0.6f, -3f), Quaternion.identity)as GameObject;
			childObject.transform.parent = GameObject.Find ("parentTrash").transform;
			trashList.Add ("can");
			break;
		}
				
			
		firstTrash = trashList [0].ToString ();
		Debug.Log (trashList.Count);
		Debug.Log (firstTrash);
			
	}
	
	void OnTap ()
	{
		StartCoroutine ("rolldownPlay");
		//loadScene("game");
	
	}

	IEnumerator rolldownPlay ()
	{


		yield return new WaitForSeconds( 1f );
	}
	
	void loadScene (string temp)
	{
		Application.LoadLevel (temp); 
		//DontDestroyOnLoad(GameObject.FindGameObjectWithTag("umbrella"));
	}
	
	void explosion (Vector3 touchPosition)
	{
		
		Instantiate (particle, new Vector3 (touchPosition.x, touchPosition.y, touchPosition.z), Quaternion.identity);
		Instantiate (mExplosion, new Vector3 (touchPosition.x, touchPosition.y, touchPosition.z), Quaternion.identity);
		
		//완료 테스트용.
//		planeEnergy.transform.position += new Vector3 (0.03f * superNum, 0, 0);
		energyBar.transform.localScale += new Vector3 (0.01f * superNum, 0, 0);
		
//		planeEnergy.transform.position += new Vector3 (0.003f * superNum, 0, 0);
//		energyBar.transform.localScale += new Vector3 (0.003f * superNum, 0, 0);
		
	}
	public GameObject wrongText;
	void mWrong ()
	{
		gaugeX1.animation ["x2"].speed = 1f;
		x1sprite.SendMessage ("Change", "x1");
		wrongText.SetActive(true);
		wrongScreen.SetActive (true);
		superLevel = 0;
		superNum = 1;
		if (mTorch.active)
			mTorch.SetActive (false);
		Trashes.difficulty = 1;
		backGround.GetComponent<tk2dSprite> ().color = new Color (255, 255, 255);
	}
	
	void onSuccess ()
	{
//		Destroy (energyBar);
		
		CancelInvoke ("count");
		mScore = seconds;
		_screenPause.SetActive (true);
		_screenPause.GetComponent<tk2dSprite> ().color = new Color (0, 0, 0, 0);
		_screenPause.GetComponent<TBFingerDown> ().enabled = false;
		Trashes.stop = true;
		gaugeX1.animation.Stop ();
		
		_OnSuccess.SetActive (true);
		StartCoroutine (fadeIn (0.5f));
		
		//High
		mHighScoreTextMesh = GameObject.Find ("HighScore").GetComponent <TextMesh> ();
		GameObject.Find ("HighScore").GetComponent <MeshRenderer> ().material.color = scoreColor;
		if (!PlayerPrefs.HasKey ("HIGHSCORE")) {
			mHighScoreTextMesh.text = ("아직 기록이 없습니다.");
			mHighScoreTextMesh.characterSize = 0.1f;
		} else {
			mHighScoreTextMesh.text = (PlayerPrefs.GetInt ("HIGHSCORE")).ToString ();
			mHighScoreTextMesh.characterSize = 0.14f;
		}
			
		
		mScoreTextMesh = GameObject.Find ("Score").GetComponent <TextMesh> ();
		GameObject.Find ("Score").GetComponent <MeshRenderer> ().material.color = scoreColor;
		
		
		mScoreTextMesh.text = "0";
		Debug.Log (mScore);
		InvokeRepeating ("scoreCount", 1f, 0.01f);
	}
	
	void scoreCount ()
	{
		
		mScoreCount += 1;
		mScoreTextMesh.text = (mScoreCount).ToString ();
		
		if (mScore == mScoreCount) {
//			if (!PlayerPrefs.HasKey ("HIGHSCORE") || PlayerPrefs.GetInt ("HIGHSCORE") > mScore) {
				PlayerPrefs.SetInt ("HIGHSCORE", mScore);
				mHighScoreTextMesh.text = (PlayerPrefs.GetInt ("HIGHSCORE")).ToString ();
				mHighScoreTextMesh.characterSize = 0.14f;
				GameObject.Find ("HighScore").animation.Play ();
				GameObject.Find ("newSprite").animation.Play ();
				//랭킹업로드 하기
				if (PlayerPrefs.GetInt ("HIGHSCORE") < 1000)
					UpLoadData ();
			
//			}
			CancelInvoke ("scoreCount");
		}
			
	}
	
	public void UpLoadData () //업로드 데이터, DB에 데이터를 추가하는 함수에요.
	{
		string url = "http://actoz.dothome.co.kr/score.php";
		WWW www; 
		WWWForm form = new WWWForm ();  //우선 폼을 선언해주고요.
  
		form.AddField ("select", "submit");
		form.AddField ("grade", "5");
		form.AddField ("ban", "3");
		form.AddField ("name", "최은경");
		form.AddField ("score", "342");
//		
// 필드를 추가해줍니다.
   
		www = new WWW (url, form);
//www를 url이랑 위에서 만든 폼으로 생성하고.
		WaitWWW (www);  
//이건 www만들면 그아래에 바로 쓰면 되는것같습니다.
	}

	IEnumerator WaitWWW (WWW www)
	{
		yield return www;
	}
	
	void newHighScore ()
	{
		if (GameObject.Find ("HighScore").active) {
			GameObject.Find ("HighScore").SetActive (false);
		} else {
			GameObject.Find ("HighScore").SetActive (true);
		}
	}
	
	IEnumerator fadeIn (float time)
	{
		int alphaNum = 0;
      
		while (alphaNum < 8) {
			alphaNum ++;
			_screenPause.GetComponent<tk2dSprite> ().color = new Color (0, 0, 0, alphaNum * 0.05f);
			yield return null;
		}
		
		
		
		
	}
	
	void Awake ()
	{
		resetScene ();
	}
	
	void resetScene ()
	{
		trashSpeed = 12f;
		hitNum = 5;
		firstTrash = "none";
		TrashBox.trashId = 1;
//	public static GameObject[] trashes;
		
		Trashes.stop = false;
		Trashes.difficulty = 1;
		CancelInvoke ();
		
		GameObject.Find ("Ready").SetActive (true);
	}

	
}
