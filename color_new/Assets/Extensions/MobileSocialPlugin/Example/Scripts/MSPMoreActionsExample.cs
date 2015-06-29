using UnityEngine;
using System.Collections;

public class MSPMoreActionsExample : MonoBehaviour {

	public Texture2D imageForPosting;

 

	public void NativeShare() {
		SPShareUtility.ShareMedia("Share Camption", "Share Message");
	}

	public void NativeShareWithImage() {
		SPShareUtility.ShareMedia("Share Camption", "Share Message", imageForPosting);
	}
}
