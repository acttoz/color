using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{
	public GameObject popupPrefab;
	public GameObject popupChoice;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void popup (string chooseItem)
	{
		Debug.Log(chooseItem);
		popupChoice.GetComponent<UILabel> ().text = chooseItem;
		NGUITools.AddChild (gameObject, popupPrefab);
		popupPrefab.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
	}
}
