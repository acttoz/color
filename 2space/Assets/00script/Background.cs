using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
		public static Background mInstance;
		public GameObject[] stars = new GameObject[6];
		public float zoneCreateRate;
		private Vector2 zonePosition;
		public GameObject[] spaces;
		private int spaceId = 0;
		public GameObject backElement, lightSpeed, oZone;
	
		public Background ()
		{
				mInstance = this;
		}
	
		public static Background instance {
				get {
						if (mInstance == null)
								new Background ();
						return mInstance;
				}
		}

		public int spaceID {
				get {
						return spaceId;
				}set {
						spaceId = value;
				}
		}
		// Use this for initialization
		void Start ()
		{
	
		}

		public void reset ()
		{
				GameObject[] temp;
				temp = GameObject.FindGameObjectsWithTag ("back");
				for (int i=0; i<temp.Length; i++) {
						Destroy (temp [i]);
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		//BACK
		void backCreate ()
		{
				if (Level.instance.superLevel > 0 && Level.instance.superLevel < 20) {
						float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100f;
						if (spaceId < 2) {
								Instantiate (backElement, new Vector3 (tempX, (MANAGER.mUp + 3), 0), Quaternion.identity);
						} else {
								//				Debug.Log(Level.instance.superLevel);
								if (Level.instance.superLevel > 5) {
										Instantiate (lightSpeed, new Vector3 (tempX, 8, 0), Quaternion.identity);
								} else {
										Instantiate (stars [Random.Range (0, 6)], new Vector3 (tempX, (MANAGER.mUp + 3), 0), Quaternion.identity);
								}
						}
				}  
		}
	
		void zoneCreate ()
		{
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						tempZone.animation.Play ("anim_zoneOut");
				float tempX = (Random.Range (MANAGER.mLeft * 100, MANAGER.mRight * 100)) / 100;
				float tempY = (Random.Range (MANAGER.mDown * 100, MANAGER.mUp * 100)) / 100;
				if (Level.instance.level > 1) {
						zonePosition = new Vector2 (tempX, tempY);
			
						GameObject obj = Instantiate (oZone, new Vector3 (tempX, tempY, 0), Quaternion.identity) as GameObject;
						 
				}
		}
	
		void zoneReset ()
		{
				CancelInvoke ("zoneCreate");
				GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
				if (tempZone != null)
						Destroy (tempZone);
		}
	
		public void planet (int spaceId)
		{
				Instantiate (spaces [spaceId], new Vector2 (0, 12.8f), Quaternion.identity);
		}
	
		//		void monsterOrange ()
		//		{
		//				isMonster = false;
		//				if (Level.instance.superLevel < levelLimit) {
		//						Level.instance.superLevel++;
		//						superMode (Level.instance.superLevel);
		//				}
		//		}
	
		
}
