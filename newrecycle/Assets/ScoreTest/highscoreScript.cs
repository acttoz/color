using UnityEngine;
using System.Collections;

[System.Serializable]
public class Score
{	
	public UILabel nameLabel;
	public UILabel distanceLabel;
	public UILabel pointLabel;
	
}


public class highscoreScript : MonoBehaviour {
	
	public Score[] score = new Score[10];
	public Score summitscore;
	
	public UILabel urlLabel;
	public UILabel hostLabel;
	public UILabel fileLabel;
	
	public string url = "";
	
	public WWW www; 
	
	public bool checkStart = false;
	
	public void Start()
	{
		SetURL();
		DownloadData();
		
	}
	

	public void SetURL()
	{
	
		url="http://"+hostLabel.text+"/"+fileLabel.text;
		urlLabel.text = url;
	}
	
	IEnumerator WaitWWW(WWW www)
	{
		yield return www;
	}
	
	public void Clear()
	{
		WWWForm form = new WWWForm();
		form.AddField("select","clear");
	
			
		www = new WWW(url,form);
		
		WaitWWW(www);
			for(int i =0; i< 10; i++) 
		{						
			score[i].nameLabel.text ="-";
			score[i].distanceLabel.text ="-";
			score[i].pointLabel.text = "-";
		}
		
	
	}
	
	
	public void UpLoadData()
	{
		
	
		WWWForm form = new WWWForm();
		form.AddField("select","submit");
		form.AddField("id",SystemInfo.deviceUniqueIdentifier.ToString());
		form.AddField("name",summitscore.nameLabel.text);
		form.AddField("distance",summitscore.distanceLabel.text);
		form.AddField("point",summitscore.pointLabel.text);
			
		www = new WWW(url,form);
	
		WaitWWW(www);
		
			
		DownloadData();
		
	}
	
	
	public void DownloadData()
	{	
		checkStart = true;
		WWWForm form = new WWWForm();
		form.AddField("select","show");
		www = new WWW(url,form);
		WaitWWW(www);
		
		
				
	}
	
	public void SettingDownloadData()
	{
		string[] data = www.text.Split('&');
		
		string[][] row = new string[data.Length][];
		
		
		for(int i=0;i<data.Length;i++)
		{
		row[i] = data[i].Split('?');
		}
		
		
			
		for(int i =0; i< row.Length-1; i++) 
		{
			if(checkStart)
			{
			
			
			score[i].nameLabel.text = row[i][0];
			score[i].distanceLabel.text =row[i][1];
			score[i].pointLabel.text = row[i][2];
				
			}
		}
		
		checkStart = false;
		}
	
		

	void Update () {
		
		if(checkStart)
		{
			if(www.isDone)
			{
				SettingDownloadData();
				checkStart = false;
		
			}
		}
	
	}

}
