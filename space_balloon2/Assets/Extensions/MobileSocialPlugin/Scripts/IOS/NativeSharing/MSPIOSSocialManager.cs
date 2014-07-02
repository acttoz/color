//#define SA_DEBUG_MODE
////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////




using UnityEngine;
using System.Collections;

#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class MSPIOSSocialManager : EventDispatcher {


	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	 
	[DllImport ("__Internal")]
	private static extern void _MSP_MediaShare(string text, string encodedMedia);
	#endif



	private static MSPIOSSocialManager _instance = null;


	 


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void ShareMedia(string text) {
		ShareMedia(text, null);
	}

	public void ShareMedia(string text, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			if(texture != null) {
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);
				_MSP_MediaShare(text, bytesString);
			} else {
				_MSP_MediaShare(text, "http://naver.com");
			}

		#endif
	}

	 
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public static MSPIOSSocialManager instance  {
		get {
			if(_instance == null) {
				GameObject go =  new GameObject("IOSSocialManager");
				_instance = go.AddComponent<MSPIOSSocialManager>();
			}

			return _instance;
		}
	}
	
 
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------


	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
