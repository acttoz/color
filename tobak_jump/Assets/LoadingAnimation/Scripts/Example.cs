using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {
	public Transform loadingPrefab;
	public Transform loadingPrefab2;
	public Transform loadingPrefab3;
	private Transform loadingClone;
	private Loading loadingComp;
	
	private Rect defaultRect = new Rect(10, 210, 200, 150);
	
	void Start () {
		if(!loadingComp)
			loadingComp = this.gameObject.GetComponent<Loading>();
	}
	
	void OnGUI() {
		GUI.enabled = true;
		if(Loading.isLoading || loadingComp.isLoadingNow) {
			GUI.enabled = false;
		}
		
		if(GUI.Button(new Rect(10, 10, 280, 30), "Big Loading for All Screen")) {
			StartCoroutine(DisplayBigLoading(loadingPrefab));
		}
		if(GUI.Button(new Rect(10, 50, 280, 30), "Big Loading for My Rect")) {
			StartCoroutine(DisplayBigLoadingBox(true, true, defaultRect));
		}
		if(GUI.Button(new Rect(10, 90, 280, 30), "Big Loading for My Rect Without Text")) {
			StartCoroutine(DisplayBigLoadingBox(false, true, defaultRect));
		}
		if(GUI.Button(new Rect(10, 130, 280, 30), "Big Loading for My Rect Without Background")) {
			StartCoroutine(DisplayBigLoadingBox(true, false, defaultRect));
		}
		if(GUI.Button(new Rect(10, 170, 280, 30), "Small Loading")) {
			StartCoroutine(DisplayBigLoadingBox(false, false, new Rect(10, 210, 60, 60)));
		}
		if(GUI.Button(new Rect(300, 10, 280, 30), "Big Loading 2")) {
			StartCoroutine(DisplayBigLoading(loadingPrefab2));
		}
		if(GUI.Button(new Rect(300, 50, 280, 30), "Big Loading 3")) {
			StartCoroutine(DisplayBigLoading(loadingPrefab3));
		}
		if(GUI.Button(new Rect(300, 90, 280, 30), "Bar Loading")) {
			StartCoroutine(DisplayLoadingBar(false, true));
		}
		if(GUI.Button(new Rect(300, 130, 280, 30), "Bar Loading with text")) {
			StartCoroutine(DisplayLoadingBar(true, true));
		}
		if(GUI.Button(new Rect(300, 170, 280, 30), "Bar Loading without percent")) {
			StartCoroutine(DisplayLoadingBar(false, false));
		}
	}
	
	IEnumerator DisplayBigLoading(Transform prefab) {
		loadingClone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as Transform;
		yield return new WaitForSeconds(2);
		Destroy(loadingClone.gameObject);
	}
	
	IEnumerator DisplayBigLoadingBox(bool withText, bool withBg, Rect loadingRect) {
		loadingComp.SetLoadingRect(loadingRect);
		loadingComp.showText = withText;
		loadingComp.showBackground = withBg;
		loadingComp.showSprite = true;
		loadingComp.showLoadingBar = false;
		loadingComp.loadingText = "Loading";
		loadingComp.StartLoading();
		yield return new WaitForSeconds(2);
		loadingComp.StopLoading();
	}
	
	IEnumerator DisplayLoadingBar(bool withText, bool withPercent) {
		loadingComp.SetLoadingRect(defaultRect);
		loadingComp.showSprite = false;
		loadingComp.showLoadingBar = true;
		loadingComp.showBackground = true;
		loadingComp.showText = withText;
		loadingComp.loadingText = "Loading";
		if(!withPercent) {
			loadingComp.loadingBarPercentPosition = Loading.loadingBarPercentPositions.None;
		}
		else {
			loadingComp.loadingBarPercentPosition = Loading.loadingBarPercentPositions.Hover;
		}
		loadingComp.loadingBarPercent = 0;
		loadingComp.StartLoading();
		yield return new WaitForSeconds(1f);
		loadingComp.loadingBarPercent = 20;
		yield return new WaitForSeconds(1f);
		if(withPercent) {
			loadingComp.loadingBarPercentPosition = Loading.loadingBarPercentPositions.Top;
		}
		loadingComp.loadingBarPercent = 45;
		yield return new WaitForSeconds(1f);
		if(withPercent) {
			loadingComp.loadingBarPercentPosition = Loading.loadingBarPercentPositions.Hover;
		}
		loadingComp.loadingBarPercent = 67;
		yield return new WaitForSeconds(1f);
		if(withPercent) {
			loadingComp.loadingBarPercentPosition = Loading.loadingBarPercentPositions.Bottom;
		}
		loadingComp.loadingBarPercent = 82;
		yield return new WaitForSeconds(1f);
		if(withPercent) {
			loadingComp.loadingBarPercentPosition = Loading.loadingBarPercentPositions.Hover;
		}
		loadingComp.loadingBarPercent = 100;
		yield return new WaitForSeconds(2);
		loadingComp.StopLoading();
	}
}
