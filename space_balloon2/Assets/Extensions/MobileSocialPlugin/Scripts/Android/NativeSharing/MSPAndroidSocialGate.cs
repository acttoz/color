using UnityEngine;
using System.Collections;

public class MSPAndroidSocialGate  {

	public static void StartShareIntent(string caption, string message, string packageNamePattern = "") {
		StartShareIntentWithSubject(caption, message, "", packageNamePattern);
	}
	
	public static void StartShareIntentWithSubject(string caption, string message, string subject, string packageNamePattern = "") {
		AndroidNative.StartShareIntent("Invite your Friends!", "Let's play 'Space Balloon'! \n http://m.site.naver.com/0avai", subject, packageNamePattern);
	}
	
	
 



}

