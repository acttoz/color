using UnityEngine;
using System.Collections;

public class OnClick : MonoBehaviour
{
	GameObject mManager, mGrid;
	public float time_press = 0;
	public float time_release = 0;
	private bool inUse = false;
	private float delta_time;
	// Use this for initialization
	void Start ()
	{
		mGrid = GameObject.Find ("LocalGrid");
		mManager = GameObject.Find ("Panel");
		
	}
	
	// Update is called once per frame
	 
    
	void OnPress (bool isPressed)
	{
                
		if (isPressed) {			
			time_press = Time.realtimeSinceStartup;	
			inUse = true;
		} else {
			time_release = Time.realtimeSinceStartup;
                
                
			if ((time_release - time_press) < 0.2f) {
				this.MyOnClick ();
			}	
                
			inUse = false;	
		}
	}
    
	void OnDrag ()
	{
		this.inUse = false;	
	}
    
	void MyOnClick ()
	{
		// YOUR CODE for CLICK HERE
		
		string parentName;
		string result;
		parentName = transform.parent.name;
		
		mManager.SendMessage ("ResetPosition");
		mManager.SendMessage ("PopUp", this.gameObject.GetComponentInChildren<UILabel> ().text);
	
		
		
			
		
		
		
		
        
	}

	void MyOnHold ()
	{
		// YOUR CODE for HOLD here
        
	}

	void Update ()
	{
        
		if (inUse) {
            
            
			delta_time = Time.realtimeSinceStartup - time_press;
			if (delta_time > 0.5f) {				
				if (Input.touchCount < 2) {
					this.MyOnHold ();
                    
				}
			}
        
		}
	}
    
}