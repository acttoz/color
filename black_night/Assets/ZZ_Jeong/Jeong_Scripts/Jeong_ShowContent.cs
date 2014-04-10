using UnityEngine;
using System.Collections;

public class Jeong_ShowContent : MonoBehaviour {
	
	public GameObject Btn_ShowMore;


	public string[] MainText;

	public string[] TempText; 
	public string MyOrbit;

	// it's for Detail  Planet
	public GameObject Camera_Main;
	public GameObject Camera_Sun;
	public GameObject Camera_Earth;
	public GameObject Camera_Jup;
	public GameObject Camera_Mar;


	// It's added in Jan 27th by Jeong
	public GameObject Camera_Mecury;
	public GameObject Camera_Venus;
	public GameObject Camera_Tosung;
	public GameObject Camera_Chunwangsung;
	public GameObject Camera_Haewangsung;
	public GameObject Camera_Moon;
	//

	 public GameObject CallPicture;



	public bool btnBool;
	// Use this for initialization
	void Start () 
	{
		btnBool = true;

		Btn_ShowMore.SetActive(false);
		CallPicture.SetActive(false);

		// It's for Datail Camera FALSE
		Camera_Main.SetActive(true);
		Camera_Sun.SetActive(false);
		Camera_Earth.SetActive(false);
		Camera_Mar.SetActive(false);
		Camera_Jup.SetActive(false);

		// Jan 27 
		Camera_Mecury.SetActive (false);
		Camera_Venus.SetActive(false);
		Camera_Tosung.SetActive(false);
		Camera_Chunwangsung.SetActive(false);
		Camera_Haewangsung.SetActive(false);
		Camera_Moon.SetActive(false);



	}

	void Btn_Back()
	{
		Camera_Main.SetActive(true);
		Camera_Sun.SetActive(false);
		Camera_Earth.SetActive(false);
		Camera_Mar.SetActive(false);
		Camera_Jup.SetActive(false);

		// Jan 27 
		Camera_Mecury.SetActive (false);
		Camera_Venus.SetActive(false);
		Camera_Tosung.SetActive(false);
		Camera_Chunwangsung.SetActive(false);
		Camera_Haewangsung.SetActive(false);
		Camera_Moon.SetActive(false);
	}

	void CallPic()
	{
		btnBool =! btnBool;

		switch(MyOrbit)
		{
		case "0Sol" :

			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(true);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);


			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);

			break;
		case "ear" :

			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(true);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);

			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);

		//	Application.LoadLevel ("Jeong_Earth");
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="earth_info";
			break;
		case "jup" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";


			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(true);

			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);

			break;

		case "mar" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(true);
			Camera_Jup.SetActive(false);

			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);
			
			break;

		case "mer" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);
			
			// Jan 27 
			Camera_Mecury.SetActive (true);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);
			
			break;

		case "ven" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);
			
			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(true);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);
			
			break;

		case "sat" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);
			
			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(true);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);
			
			break;

		case "ura" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);
			
			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(true);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(false);
			
			break;

		case "nep" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);
			
			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(true);
			Camera_Moon.SetActive(false);
			
			break;

		case "mon" :
			//CallPicture.SetActive(true);
			//GameObject.Find ("ChangeSprite").GetComponent<UISprite>().spriteName="jupiter_info";
			
			
			Camera_Main.SetActive(false);
			Camera_Sun.SetActive(false);
			Camera_Earth.SetActive(false);
			Camera_Mar.SetActive(false);
			Camera_Jup.SetActive(false);
			
			// Jan 27 
			Camera_Mecury.SetActive (false);
			Camera_Venus.SetActive(false);
			Camera_Tosung.SetActive(false);
			Camera_Chunwangsung.SetActive(false);
			Camera_Haewangsung.SetActive(false);
			Camera_Moon.SetActive(true);
			
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	


		if(btnBool == true)
		{
			//CallPicture.SetActive(false);
		}

		if(btnBool == false)
		{
			//CallPicture.SetActive(true);
		}

		GameObject.Find ("Label1").GetComponent<UILabel>().text=MainText[0];
		GameObject.Find ("Label2").GetComponent<UILabel>().text=MainText[1];
		GameObject.Find ("Label3").GetComponent<UILabel>().text=MainText[2];


		GameObject.Find ("Label101").GetComponent<UILabel>().text=TempText[0];
		GameObject.Find ("Label102").GetComponent<UILabel>().text=TempText[1];
		GameObject.Find ("Label103").GetComponent<UILabel>().text=TempText[2];
		GameObject.Find ("Label104").GetComponent<UILabel>().text=TempText[3];
		GameObject.Find ("Label105").GetComponent<UILabel>().text=TempText[4];
		GameObject.Find ("Label106").GetComponent<UILabel>().text=TempText[5];

	}



	void ShowContent(string orbitName)
	{
		Debug.Log ("show_"+orbitName);
			MyOrbit = orbitName;

		switch(orbitName)
		{
		case "0Sol" : 

			MainText[0]="태양은 태양계의 중심에 있어.";
			MainText[1]="태양에서 나온 에너지가 지구에 가서";
			MainText[2]="모든 생명의 생존을 가능하게 한단다";

			TempText[0] = "태양";
			TempText[1] = "태양계의 중심";
			TempText[2] = "크기(지름) : 139만 2천km";
			TempText[3] = "무게 : 지구의 33만배";
			TempText[4] = "표면온도 : 약 6000도";
			TempText[5] = "";




			Btn_ShowMore.SetActive (true);



			break;
		
		case "ear" :

			MainText[0]="지구는 약 46억년전에 생성되었어.";
			MainText[1]="엷은 대기층과 물이 있어 생명체가";
			MainText[2]="살 수 있는 행성이란다.";


			TempText[0] = "지구";
			TempText[1] = "크기(지름) : 12700km";
			TempText[2] = "하루 : 23.93시간";
			TempText[3] = "일년 : 365.26일";
			TempText[4] = "태양까지의 거리 : 1억 5천만km";
			TempText[5] = "위성의 수 : 1개";

		
			Btn_ShowMore.SetActive (true);

			break;

		case "ven" :
			
			MainText[0]="금성은 샛별로도 불리는데 ";
			MainText[1]="크기와 무게가 거의 비슷해서";
			MainText[2]="쌍둥이별로 불리기도 한단다";
			
			
			TempText[0] = "금성";
			TempText[1] = "크기(지름) : 12100km (지구의 0.9배)";
			TempText[2] = "하루 : 243일";
			TempText[3] = "일년 : 224일";
			TempText[4] = "태양까지의 거리 : 1억 8백만km";
			TempText[5] = "위성 : 없음";
			
			
			Btn_ShowMore.SetActive (true);
			
			break;
		
		

		case "jup" :
		
			MainText[0]="목성은 태양계에서 가장 큰 행성이란다";
			MainText[1]="행성자체가 기체로 이루어져 있고 ";
			MainText[2]="많은 위성을 거느리고 있단다.";


			TempText[0] = "목성";
			TempText[1] = "크기(지름) : 14만km";
			TempText[2] = "하루 : 10시간";
			TempText[3] = "일년 : 12년";
			TempText[4] = "태양까지의 거리 : 7억 8천만km";
			TempText[5] = "위성의 수 : 61개";




			Btn_ShowMore.SetActive (true);

		break;

		

		case "mon" :
			
			MainText[0]="달은 지구의 위성이란다.";
			MainText[1]="우리가 바라보는 달은";
			MainText[2]="항상 달의 앞모습이란다.";
			
			
			TempText[0] = "달";
			TempText[1] = "크기(지름) : 3500km (지구의 0.27배)";
			TempText[2] = "자전주기 : 27.3일";
			TempText[3] = "공전주기 : 27.3일";
			TempText[4] = "지구까지의 거리 : 38만 4천km";
			TempText[5] = "지구의 하나뿐인 위성";
			
			
			
			
			Btn_ShowMore.SetActive (true);
			
			break;



		case "mer" :
			
			MainText[0]="수성은 태양계에서 가장 ";
			MainText[1]="태양과 가까운 행성이란다.";
			MainText[2]="";
			
			
			TempText[0] = "수성";
			TempText[1] = "크기(지름) : 5000km (지구의 0.4배)";
			TempText[2] = "하루 : 59일";
			TempText[3] = "일년 : 88일";
			TempText[4] = "태양까지의 거리 : 5800만km";
			TempText[5] = "위성 : 없음";
			
			
			
			
			Btn_ShowMore.SetActive (true);
			
			break;



		case "mar" :
			
			MainText[0]="화성은 지구와 닮은 점이 많아서";
			MainText[1]="화성에 대한 탐사가 계속 ";
			MainText[2]="이루어지고 있지.";
			
			
			TempText[0] = "화성";
			TempText[1] = "크기(지름) : 6800km (지구의 0.5배)";
			TempText[2] = "하루 : 24.6시간";
			TempText[3] = "일년 : 687년";
			TempText[4] = "태양까지의 거리 : 2억 3천만 km";
			TempText[5] = "위성의 수 : 2개";
			
			
			
			
			Btn_ShowMore.SetActive (true);
			
			break;



		case "sat" :
			
			MainText[0]="토성은 위성의 수가 아주 많고";
			MainText[1]="행성을 둘러싸고 있는 띠가 ";
			MainText[2]="아주 아름답단다.";
			
			
			TempText[0] = "토성";
			TempText[1] = "크기(지름) : 12만km (지구의 9.5배)";
			TempText[2] = "하루 : 10.7시간";
			TempText[3] = "일년 : 29.5년";
			TempText[4] = "태양까지의 거리 : 14억 3천만 km";
			TempText[5] = "위성의 수 : 60개";
			
			
			
			
			Btn_ShowMore.SetActive (true);
			
			break;



		case "ura" :
			
			MainText[0]="천왕성은 파란빛으로 보이는 ";
			MainText[1]="아름다운 행성이란다. ";
			MainText[2]="주로 얼음과 기체로 이루어졌지";
			
			
			TempText[0] = "천왕성";
			TempText[1] = "크기(지름) : 5만 1천 km (지구의 4배)";
			TempText[2] = "하루 : 17시간";
			TempText[3] = "일년 : 84년";
			TempText[4] = "태양까지의 거리 : 28억 km";
			TempText[5] = "위성의 수 : 27개";
			
			
			
			
			Btn_ShowMore.SetActive (true);
			
			break;

		case "nep" :
			
			MainText[0]="해왕성은 천왕성과 같이 ";
			MainText[1]="거대 얼음행성으로 불리기도 한단다. ";
			MainText[2]="메탄이 많아 푸른빛으로 보이지.";
			
			
			TempText[0] = "해왕성";
			TempText[1] = "크기(지름) : 4만 9천km (지구의 3.8배)";
			TempText[2] = "하루 : 16시간";
			TempText[3] = "일년 : 165년";
			TempText[4] = "태양까지의 거리 : 45억 km";
			TempText[5] = "위성의 수 : 13개";
			
			
			
			
			Btn_ShowMore.SetActive (true);
			
			break;
		}
	


	}
}
