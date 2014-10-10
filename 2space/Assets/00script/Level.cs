using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
		private int mLevel = 0;
		private int mSuperLevel = 0;
		public int levelLimit;
	public tk2dTextMesh lvText;
		int superTimer;
		public static Level mInstance;
		// Use this for initialization
		void Start ()
		{
		}

		public Level ()
		{
				mInstance = this;
		}

		public static Level instance {
				get {
						if (mInstance == null)
								new Level ();
						return mInstance;
				}
		}

		public void reset ()
		{
				lvText.text = "Lv.1";
		
		}
		// Update is called once per frame
		void Update ()
		{
	
		}

		public int level {
				get {
						return mLevel;
				}set {
						mLevel = value;
				}
		}

		public int superLevel {
				get {
						return mSuperLevel;
				}set {
						mSuperLevel = value;
				}
		}
		/// <summary>
		/////////////////////////////////////////////// Supers the mode count.///////////////////////////////////////////////////////////////////////////
		/// </summary>
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 						SUPER
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// 
		/// STOP
		

	
		void normalModeCount ()
		{
				superTimer--;
		
				if (superTimer < 0) {
			
			
						CancelInvoke ("normalModeCount");
						superLevel++;
						if (superLevel < 4)
								superMode (superLevel);
			
			
			
			
				}
		
		
		}
	
		public void superMode (int num)
		{
//				balloon.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
//				if (!onPlay)
//						return;
		
//				if (num == levelLimit) {
//						CancelInvoke ("zoneCreate");
//						GameObject tempZone = GameObject.FindGameObjectWithTag ("zone");
//						if (tempZone != null)
//								Destroy (tempZone);
//				}
//				zonePosition = new Vector2 (100, 100);
//				CancelInvoke ("backCreate");
//				CancelInvoke ("balloonStop");
//				CancelInvoke ("normalModeCount");
				//				balloon.SendMessage ("stopBalloon", false);
//				Debug.Log ("supermode" + num);
//				stopRate = 0;
				//				stopRate = stopRateControl;
//				scoreRate = 1;
//				if (num == 0) {
//						superTimer = 3;
//						superLevel = 1;
//						//						InvokeRepeating ("normalModeCount", 0.1f, 1f);
//				} else if (Level.instance.level != 6 && Level.instance.level != 7 && Level.instance.level != 8) {
//						zoneReset ();
//						if (num == 1)
//								InvokeRepeating ("zoneCreate", 0.1f, zoneCreateRate);
//						else if (num != levelLimit)
//								InvokeRepeating ("zoneCreate", 2f, zoneCreateRate);
//						superTimer = superTime;
//						InvokeRepeating ("superModeCount", 0.1f, 0.2f);
//				}
//		
//				balloon.SendMessage ("superMode", num);
//		
//		
//				//				enemy [0].SendMessage ("superMode", num);
//				//				enemy [1].SendMessage ("superMode", num);
//				//				enemy [2].SendMessage ("superMode", num);
//				bgm.SendMessage ("superMode", num);
//				if (num > 1 && num < 11 && !isCounting) {
//			
//						//levelUp Effect
//						isUndead = true;
//						undeadTime = 3;
//						lv.SendMessage ("levelUp");
//						GameObject objUI = GameObject.Find ("UI");
//						GameObject obj1 = Instantiate (effectSuper1, new Vector2 (0, 0), Quaternion.identity) as GameObject;
//						obj1.transform.parent = objUI.transform;
//						obj1.transform.localPosition = new Vector2 (0, 0);
//						audio.PlayOneShot (levelUp);
//						GameObject obj2 = Instantiate (super_back2, new Vector2 (0, 0), Quaternion.identity) as GameObject;
//						obj2.transform.parent = objUI.transform;
//						obj2.transform.localPosition = new Vector2 (0, 0);
//						if (num > 5) {
//								enemyBomb ();
//								turnLightSpeed (120);
//						}
//				}
//				isCounting = false;
//		
//				//********************score
//				CancelInvoke ("scoreCount");
//				switch (num) {
//				case 0:
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [0]);
//						InvokeRepeating ("backCreate", 0, levelRate [0] * 4);
//			
//						break;
//				case 1:
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 4);
//						break;
//				case 2:
//			//						enemyBomb ();
//						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 4);
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv.2";
//			
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//			//Level.instance.getLevel2
//						if (Level.instance.level == 2 && onPlay) {
//								PlayerPrefs.SetInt ("2", 1);
//								timeOut ();
//						}
//			
//						break;
//				case 3:
//			//						enemyBomb ();
//						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 4);
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv.3";
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//			//Level.instance.getLevel3
//						if (Level.instance.level == 3 && onPlay) {
//				
//								PlayerPrefs.SetInt ("3", 1);
//								timeOut ();
//				
//						}
//						break;
//			
//			
//			
//				case 4:
//			//						enemyBomb ();
//						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 6);
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv.4";
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						if (!isMonster)
//								balloonSprite.sprite = superBalloon4;
//			//Level.instance.getLevel4
//						if (Level.instance.level == 4 && onPlay) {
//								PlayerPrefs.SetInt ("4", 1);
//								timeOut ();
//						}
//						break;
//			
//				case 5:
//			//						enemyBomb ();
//						InvokeRepeating ("backCreate", 0, levelRate [num - 1] * 10);
//			
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv.5";
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						if (!isMonster)
//								balloonSprite.sprite = superBalloon5;
//			//Level.instance.getLevel5
//						if (Level.instance.level == 5 && onPlay) {
//								PlayerPrefs.SetInt ("5", 1);
//								timeOut ();
//						}
//						break;
//				case 6:
//						InvokeRepeating ("backCreate", 0, levelRate [4] * 5);
//						scoreRate = scoreRates [num - 6];
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv.6";
//			
//			
//			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
//						if (!isMonster)
//								balloonSprite.sprite = ufos [0];
//			//Level.instance.getLevel5
//			
//						break;
//				case 7:
//						InvokeRepeating ("backCreate", 0, levelRate [4] * 5);
//						scoreRate = scoreRates [num - 6];
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv." + num;
//			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
//						if (!isMonster)
//								balloonSprite.sprite = ufos [num - 6];
//			//Level.instance.getLevel5
//			
//						break;
//				case 8:
//						InvokeRepeating ("backCreate", 0, levelRate [4] * 7);
//						scoreRate = scoreRates [num - 6];
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv." + num;
//			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
//						if (!isMonster)
//								balloonSprite.sprite = ufos [num - 6];
//			//Level.instance.getLevel5
//			
//						break;
//				case 9:
//						InvokeRepeating ("backCreate", 0, levelRate [4] * 10);
//						scoreRate = scoreRates [num - 6];
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv." + num;
//			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
//						if (!isMonster)
//								balloonSprite.sprite = ufos [num - 6];
//			//Level.instance.getLevel5
//			
//						break;
//				case 10:
//						InvokeRepeating ("backCreate", 0, levelRate [4] * 10);
//						scoreRate = scoreRates [num - 6];
//						balloon.SendMessage ("undead", true);
//						lvText.text = "Lv." + num;
//			//						InvokeRepeating ("scoreCount", 0.1f, levelRate [num - 1]);
//						InvokeRepeating ("scoreCount", 0.1f, levelRate [4]);
//						if (!isMonster)
//								balloonSprite.sprite = ufos [num - 6];
//			//Level.instance.getLevel5
//			
//						break;
//			
//				case 20:
//						InvokeRepeating ("scoreCount", 0.1f, 0.1f);
//						InvokeRepeating ("backCreate", 0, levelRate [1] * 4);
//						break;
//				default:
//						break;
//			
//			
//				}
//		
//				InvokeRepeating ("balloonStop", 0.5f, 0.1f);
		}
	
}
