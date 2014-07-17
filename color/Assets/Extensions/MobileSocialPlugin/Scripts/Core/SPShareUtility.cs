using UnityEngine;
using System.Collections;

public class SPShareUtility  {

	 


	 


	public static void ShareMedia(string caption, string message) {
		ShareMedia(caption, message, null);
	}
	
	public static void ShareMedia(string caption, string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			MSPAndroidSocialGate.StartShareIntent("SpaceBalloon", "Monsters is waiting for your FINGER. Help them to back to planet.\n http://m.site.naver.com/0aHBg", texture);
			break;
		case RuntimePlatform.IPhonePlayer:
			MSPIOSSocialManager.instance.ShareMedia(message, texture);
			break;
		}
	}



}
