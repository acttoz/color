using UnityEngine;
using System.Collections;

public class Pdh_maneger : MonoBehaviour {

	GameObject Btn_Select;
	GameObject Btn_Story;
	string check_story;
	bool check_select;
	bool check_telling;
	GameObject Btn_Back;
	GameObject Btn_ViewCon;
	GameObject Clipped_View;
	GameObject Dialog;
	GameObject sprite_gom1;
	GameObject sprite_story;
	GameObject Sprite_bgscrollview;
	GameObject Clipped;

	GameObject sprite_telling;

	// Use this for initialization
	void Start () {
		check_story="0";
		check_select=false;
		check_telling=false;
	
	    sprite_story=GameObject.Find("Sprite_story");
		sprite_gom1=GameObject.Find("sprite_gom");
		sprite_telling=GameObject.Find("sprite_telling");

		Btn_ViewCon=GameObject.Find("Btn_ViewCon");
		Btn_Back=GameObject.Find("Btn_Back");
		Btn_Story=GameObject.Find("Btn_Story");


		Sprite_bgscrollview=GameObject.Find("Sprite_bgscrollview");


		Btn_Select=GameObject.Find("Btn_Select");
		Dialog=GameObject.Find("Dialog");
		Clipped_View=GameObject.Find("Clipped_View");


		sprite_story.SetActive(false);
		Btn_Back.SetActive(false);
		Btn_Story.SetActive(false);
		Sprite_bgscrollview.SetActive(false);
		Clipped_View.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) // 아이폰에서도 작용하는지 모르겠음 
		{
			Application.LoadLevel(1);
		}
	}
	
	void showgom()
	{
		check_story="gom";
		ShowCon ();
	}

	void showchunyu()
	{
		check_story="chunyu";
		ShowCon ();
	}

	void showlion()
	{
		check_story="lion";
		ShowCon ();
	}

	void showssang()
	{
		check_story="ssang";
		ShowCon ();
	}

	void showdok()
	{
		check_story="dok";
		ShowCon ();
	}

	void showorion()
	{
		check_story="orion";
		ShowCon ();
	}

	void showandro()
	{
		check_story="andro";
		ShowCon ();
	}

	void showbekjo()
	{
		check_story="bekjo";
		ShowCon ();
	}

	void showmokdong()
	{
		check_story="mokdong";
		ShowCon ();
	}


	void ShowClipped()
	{
		if(check_select==false)
		{
			Sprite_bgscrollview.SetActive(true);
			Clipped_View.SetActive(true);
			check_select=true;
		}

		else if(check_select==true)
		{
			Sprite_bgscrollview.SetActive(false);
			Clipped_View.SetActive(false);
			check_select=false;
		}
	}

	void UnShowClipped()
	{
		Sprite_bgscrollview.SetActive(false);
		Clipped_View.SetActive(false);
		check_select=false;
	}

	void Refresh()
	{
		Dialog.SetActive(true);
		Btn_Select.SetActive(true);
		Btn_Back.SetActive(false);
		//Btn_Story.SetActive(false);
		sprite_story.SetActive(false);
		ShowClipped();
		check_story="0";
		check_telling=false;
	}

	void ShowCon()
	{
		Btn_Back.SetActive(true);
		Btn_Story.SetActive(true);

		Btn_ViewCon.SetActive(false);
		Dialog.SetActive(false);
		Btn_Select.SetActive(false);
		sprite_telling.SetActive(false);
		check_select=false;
		
		UnShowClipped();

		if(check_story=="gom")
		{
			sprite_story.SetActive(true);

			sprite_gom1.GetComponent<UISprite>().spriteName="con_bear";

			sprite_gom1.SetActive(true);
		}

		else if(check_story=="chunyu")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_chunyu";
			
			sprite_gom1.SetActive(true);
		}

		else if(check_story=="lion")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_saja";
			
			sprite_gom1.SetActive(true);
		}

		else if(check_story=="dok")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_doksuri";
			
			sprite_gom1.SetActive(true);
		}

		else if(check_story=="mokdong")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_mokdong";
			
			sprite_gom1.SetActive(true);
		}


		else if(check_story=="ssang")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_ssang";
			
			sprite_gom1.SetActive(true);
		}

		else if(check_story=="orion")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_orion";
			
			sprite_gom1.SetActive(true);
		}

		else if(check_story=="andro")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_andro";
			
			sprite_gom1.SetActive(true);
		}

		else if(check_story=="bekjo")
		{
			sprite_story.SetActive(true);
			
			sprite_gom1.GetComponent<UISprite>().spriteName="con_bekjo";
			
			sprite_gom1.SetActive(true);
		}
		
	}

	void SelectStory()
	{
		sprite_telling.SetActive(false);
		Btn_ViewCon.SetActive(false);
		Btn_Story.SetActive(true);
		check_telling=!check_telling;
	}

	void ShowDialog()
	{
		if(check_telling==false)
		{
			Btn_Story.SetActive(false);
			Btn_ViewCon.SetActive(true);
			if(check_story=="gom")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_bear";
			}

			else if(check_story=="chunyu")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_chunyu";
			}

			else if(check_story=="andro")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_andro";
			}

			else if(check_story=="bekjo")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_bekjo";
			}

			else if(check_story=="dok")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_eagle";
			}

			else if(check_story=="ssang")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_ssang";
			}

			else if(check_story=="orion")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_orion";
			}

			else if(check_story=="lion")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_lion";
			}

			else if(check_story=="mokdong")
			{
				sprite_telling.GetComponent<UISprite>().spriteName="story_mokdong";
			}

			sprite_telling.SetActive(true);
			check_telling=!check_telling;
		}

		else if(check_telling==true)
		{
			Btn_Story.SetActive(false);
			sprite_telling.SetActive(false);
			check_telling=!check_telling;
		}
			
	}


}
