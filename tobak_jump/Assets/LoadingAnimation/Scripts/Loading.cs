using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	//to hide background set none for `background`
	private Rect loadingRect = new Rect(0, 0, 0, 0);
	public bool showBackground = true;
	public Texture2D background;
	public float loadingMargin = 20;
	
	//to hide text let `loadingText` empty
	public bool showText = true;
	private int counter = 0;
	public string loadingText = "Loading";
	public bool showLoadingTextPoints = true;
	public int loadingTextPointsNumber = 3;
	private Rect loadingTextRect;
	public GUIStyle loadingTextStyle;
	
	public bool showSprite = true;
	public Texture2D loadingSprite;
	public Vector2 loadingImageSize = new Vector2(60, 60);
	public int lastRowImagesNr = 4;
	private Rect loadingImageRectGroup;
	private Rect loadingImageRect;
	private float loadingImageRectTop = 0;
	private float loadingImageRectLeft = 0;
	
	public static bool isLoading = false;	//use this is exist only one instance in scene
	public bool isLoadingNow = true;		//use this for every instance
	
	//loading bar
	public bool showLoadingBar = false;
	private Rect loadingBarRect;
	public Texture2D loadingBarBackground;
	private Rect loadingBarFillRect = new Rect(0, 0, 0, 0);
	public Texture2D loadingBarFill;
	public float loadingBarHeight = 20;
	public float loadingBarFillMargin = 2;
	public int loadingBarPercent = 0;
	public string loadCompleteText = "Loaded";
	public enum loadingBarPercentPositions { None, Top, Hover, Bottom };
	public loadingBarPercentPositions loadingBarPercentPosition = loadingBarPercentPositions.Hover;
	public GUIStyle loadingBarPercentStyle;
	
	private Rect guiRectZero;
	
	void Awake() {
		if(isLoadingNow) {
			isLoading = true;
			
			SetLoadingRect(loadingRect);
			if(showLoadingTextPoints) {
				StartCoroutine("Counter");
			}
			StartCoroutine("LoadingSprite");
		}
		guiRectZero = new Rect(0, 0, 0, 0);
		loadingImageRect = new Rect(0, 0, loadingSprite.width, loadingSprite.height);
	}
	
	void OnGUI() {
		if(isLoadingNow) {
			GUI.depth = 0;
			
			if(showBackground) {
				GUI.DrawTexture(loadingRect, background);
			}
			
			if(showText) {
				if(loadingTextStyle != null) {
					GUI.Label(loadingTextRect, loadingText, loadingTextStyle);
				}
				else {
					GUI.Label(loadingTextRect, loadingText);
				}
			}
			
			if(showSprite) {
				GUI.BeginGroup(loadingImageRectGroup);
					loadingImageRect.Set(loadingImageRectLeft, loadingImageRectTop, loadingSprite.width, loadingSprite.height);
					GUI.DrawTexture(loadingImageRect, loadingSprite);
				GUI.EndGroup();
			}
			else if(showLoadingBar) {
				if(loadingBarPercent == 100 && loadingText != loadCompleteText) {
					StopCoroutine("Counter");
					loadingText = loadCompleteText;
				}
				GUI.DrawTexture(loadingBarRect, loadingBarBackground);
				loadingBarFillRect.Set(loadingBarRect.x + loadingBarFillMargin, 
									   loadingBarRect.y + loadingBarFillMargin, 
									   (loadingBarRect.width - 2 * loadingBarFillMargin) * 0.01f * loadingBarPercent, 
									   loadingBarRect.height - 2 * loadingBarFillMargin);
				GUI.DrawTexture(loadingBarFillRect, loadingBarFill);
				switch (loadingBarPercentPosition) {
					case loadingBarPercentPositions.Top:
						loadingBarFillRect.Set(loadingBarRect.x + loadingBarFillRect.width - 20, loadingBarRect.y - 20, 40, 30);
						GUI.Label(loadingBarFillRect, loadingBarPercent + "%", loadingBarPercentStyle);
						break;
					case loadingBarPercentPositions.Hover:
						loadingBarFillRect.Set(loadingBarRect.x, loadingBarRect.y, loadingBarRect.width, loadingBarRect.height);
						GUI.Label(loadingBarFillRect, loadingBarPercent + "%", loadingBarPercentStyle);
						break;
					case loadingBarPercentPositions.Bottom:
						loadingBarFillRect.Set(loadingBarRect.x + loadingBarFillRect.width - 20, loadingBarRect.y + loadingBarRect.height - 10, 40, 30);
						GUI.Label(loadingBarFillRect, loadingBarPercent + "%", loadingBarPercentStyle);
						break;
				}
			}
		}
	}
	
	public void SetLoadingRect(Rect newLoadingRect) {
		if(guiRectZero.Equals(newLoadingRect)) {
			loadingRect.Set(loadingMargin, loadingMargin, Screen.width - 2 * loadingMargin, Screen.height - 2 * loadingMargin);
		}
		else {
			loadingRect = newLoadingRect;
		}
		
		loadingTextRect = new Rect(loadingRect.x, loadingRect.y, loadingRect.width, loadingRect.height);
		loadingImageRectGroup = new Rect(loadingRect.x + loadingRect.width * 0.5f - loadingImageSize.x / 2, 
											 loadingRect.y + loadingRect.height * 0.5f - loadingImageSize.y / 2, loadingImageSize.x, loadingImageSize.y);
		loadingBarRect = new Rect(loadingRect.x + loadingRect.width * 0.1f, loadingRect.y + loadingRect.height * 0.5f - loadingBarHeight / 2, loadingRect.width * 0.8f, loadingBarHeight);
		
		if(showText && showSprite) {
			loadingTextRect.Set(loadingTextRect.x, loadingTextRect.y, loadingTextRect.width, loadingTextRect.height - loadingImageSize.y);
			loadingImageRectGroup.Set(loadingImageRectGroup.x, loadingImageRectGroup.y + loadingImageSize.y / 2, loadingImageRectGroup.width, loadingImageRectGroup.height);
		}
		else if(showText && showLoadingBar) {
			loadingTextRect.Set(loadingTextRect.x, loadingTextRect.y, loadingTextRect.width, loadingTextRect.height - loadingBarHeight);
			loadingBarRect.Set(loadingBarRect.x, loadingBarRect.y + loadingBarHeight / 2, loadingBarRect.width, loadingBarRect.height);
			if(loadingBarPercentPosition == loadingBarPercentPositions.Top || 
					loadingBarPercentPosition == loadingBarPercentPositions.Hover || 
					loadingBarPercentPosition == loadingBarPercentPositions.Bottom) {
				loadingTextRect.Set(loadingTextRect.x, loadingTextRect.y, loadingTextRect.width, loadingTextRect.height - 30);
				loadingBarRect.Set(loadingBarRect.x, loadingBarRect.y + 10, loadingBarRect.width, loadingBarRect.height);
			}
		}
	}
	
	//Start Loading
	public void StartLoading() {
		if(!isLoadingNow) {
			isLoadingNow = true;
			
			SetLoadingRect(loadingRect);
			if(showText) {
				StartCoroutine("Counter");
			}
			if(showSprite && !showLoadingBar) {
				StartCoroutine("LoadingSprite");
			}
		}
	}
	
	//Stop Loading
	public void StopLoading() {
		if(isLoadingNow) {
			isLoadingNow = false;
			
			StopCoroutine("Counter");
			StopCoroutine("LoadingSprite");
			counter = 0;
		}
	}
	
	//Counter coroutine (add points after `LoadingText`)
	private WaitForSeconds waitForHalfSecond = new WaitForSeconds(0.5f);
	IEnumerator Counter() {
		while(true) {
			yield return waitForHalfSecond;
			counter++;
			loadingText = loadingText + ".";
			if(counter > loadingTextPointsNumber) {
				loadingText = loadingText.Substring(0, loadingText.Length - 4);
				counter = 0;
			}
		}
	}
	
	//Sprite coroutine (calculate left and top margins)
	public float loadingSpriteFrame = 0.01f;
	private WaitForSeconds waitForSprite;
	IEnumerator LoadingSprite() {
		waitForSprite = new WaitForSeconds(loadingSpriteFrame);
		while(true) {
			yield return waitForSprite;
			
			loadingImageRectLeft -= loadingImageSize.x;
			if(-loadingImageRectLeft > loadingSprite.width - loadingImageSize.x) {
				loadingImageRectLeft = 0;
				loadingImageRectTop -= loadingImageSize.y;
			}
			
			if(-loadingImageRectTop > loadingSprite.height - 2 * loadingImageSize.y && loadingImageRectLeft == -loadingImageSize.x * lastRowImagesNr) {
				loadingImageRectLeft = 0;
				loadingImageRectTop = 0;
			}
			else if(-loadingImageRectTop > loadingSprite.height - loadingImageSize.y) {
				loadingImageRectTop = 0;
			}
		}
	}
	
	public void OnDestroy() {
		isLoading = false;
	}
}
