using UnityEngine;
using System.Collections;

public class CreateItem : MonoBehaviour
{
	public GameObject Item;
	public string[] localName;
	// Use this for initialization
	void Start ()
	{
//		InitItem ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void InitItem ()
	{
		
//		UIGrid grid = GetComponentInChildren<UIGrid> ();
//		UIPanel panel = GetComponent<UIPanel> ();
//		UIDraggablePanel draggablePanel = GetComponent<UIDraggablePanel> ();
		//모두 10개의 Item을 생성합니다.
		for (int i = 0; i < localName.Length; i++) {
			//일단 생성합니다. 무조건...
			//생성된 GameObject의 부모가 누구인지 명확히 알려줍니다. (내가 니 애비다!!)
			GameObject obj = Instantiate (Item, new Vector3 (0f, 0f, -20f), Quaternion.identity) as GameObject;
//			NGUITools.AddChild (GameObject.Find ("LocalGrid"), Item);
			obj.transform.parent = this.transform;
//			obj.transform.position = new Vector3 (0, 0, -30f);
			//NGUI는 자동이 너무많이 짜증나니 수동으로 Scale을 조정해줍니다.
			obj.transform.localScale = new Vector3 (1f, 1f, 1f);
			//Label에 i값을 넣습니다. 
			obj.GetComponentInChildren<UILabel> ().text = localName [i];
//			obj.GetComponentInChildren<UILabel> ().text = "localName [i]";
		}
		//Prefab을 생성한 이후에 Position이 모두 같아서 겹쳐지므로 Reposition시키도록 합니다.
		GetComponent<UIGrid> ().Reposition ();
	}
}
