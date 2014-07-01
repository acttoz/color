using UnityEngine;
using System.Collections;

public class MSPMoreActionsExample : MonoBehaviour {


	public void InstaShare() {
	}

	public void InstaShareWithText() {
	}

	public void NativeShare() {
		SPShareUtility.ShareMedia("Invite your Friends!", "Let's play 'Space Balloon'! \n http://m.site.naver.com/0avai");
		Debug.Log ("share");
	}

	public void NativeShareWithImage() {
		SPShareUtility.ShareMedia("Share Camption", "22222222222");
	}
}
