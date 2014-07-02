using UnityEngine;
using System.Collections;

public class SPShareUtility  {

	 

 


	public static void ShareMedia(string caption, string message) {
		ShareMedia(caption, message, null);
	}
	
	public static void ShareMedia(string caption, string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			MSPAndroidSocialGate.StartShareIntent(caption, message);
			break;
		case RuntimePlatform.IPhonePlayer:
			MSPIOSSocialManager.instance.ShareMedia(message);
			break;
		}
	}



}
