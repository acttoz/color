using UnityEngine;
using System.Collections;

public class color_mat : MonoBehaviour
{
	private bool isPlus = false;
	private bool isMinus = false;
//		private Transform whiteMAT_position;
//		private GameObject whiteMat;
//		private SpriteRenderer whiteMatSprite;
//		private Animation[] whiteMAT_anim;
//		private SpriteRenderer matSprite;
	private EasySprite_HSV saturation;
//		private EasySprite_Pattern pattern;
	public GameObject effectSuccess;
	public GameObject effectSuccessBack;
	public GameObject oClone;
	public AudioClip sSuccess;
//	public EasySprite_HSV cHSV;
	public Texture2D rainbow;
	public int bossID;

	void Start ()
	{
//				whiteMat = gameObject.transform.parent.gameObject;
//				whiteMatSprite = whiteMat.GetComponent<SpriteRenderer> ();
//				matSprite = GetComponent<SpriteRenderer> ();
//				pattern = GetComponent<EasySprite_Pattern> ();
		if (gameObject.name.Equals ("mat"))
			makeRainbow ();
	}
	// Use this for initialization
	void reset ()
	{
//				saturation.enabled = false;
		
//				if (oClone != null)
//						Destroy (oClone);
			
		CancelInvoke ("minusColor");
		CancelInvoke ("plusColor");
//				whiteMAT_position = GetComponentInChildren<Transform> ();
//		whiteMAT_position.localPosition = new Vector3 (0, 0, 0);
//				whiteMAT_anim = GetComponentsInChildren<Animation> ();
//				whiteMatSprite.sortingOrder = matSprite.sortingOrder - 1;
//				whiteMatSprite.sortingLayerName = matSprite.sortingLayerName;
//				GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
		InvokeRepeating ("plusColor", 0, 0.01f);
		InvokeRepeating ("minusColor", 0, 0.01f);
		offColor ();
	}

	void makeRainbow ()
	{
//				oClone = Instantiate (this.gameObject, this.transform.position, Quaternion.identity) as GameObject;
//				oClone.tag = "Untagged";
//				pattern = oClone.AddComponent ("EasySprite_Pattern") as EasySprite_Pattern;
//				pattern.__MainTex2 = rainbow;
//				pattern._AutoScrollY = true;
//				pattern._AutoScrollSpeedY = -2.5f;
//				//				pattern.enabled = true;
//				oClone.GetComponent<SpriteRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder + 1;
//				oClone.GetComponent<PolygonCollider2D> ().enabled = false;
//				oClone.renderer.material.shader = Shader.Find ("EasySprite2D/Pattern_EasyS2D");
//		
//				oClone.SetActive (false);
		//				saturation.enabled = true;
		saturation = GetComponent ("EasySprite_HSV") as EasySprite_HSV;
		saturation._HueShift = 0;
		renderer.material.shader = Shader.Find ("EasySprite2D/HSV_EasyS2D");
	}
	
	// Update is called once per frame
	void Update ()
	{
				 
	}

	void plusColor ()
	{
//				Debug.Log (GetComponent<SpriteRenderer> ().color.a + "");
		if (!isPlus || !STATE._STATE.Equals ("gIDLE")) {
			endPlus ();
			return;
		}
		if (saturation._Saturation < 3) {
			starPlus ();

//						cpnt_whiteMAT_sprite [1].color = new Color (1, 0, 0);
//						pattern._Alpha = 1 - saturation._Saturation;
			saturation._Saturation += RATE.colorPlusRate / 3000f;
//						pattern._Alpha -= RATE.colorPlusRate / 10000f;
			saturation._ValueBrightness += RATE.colorPlusRate / 6000f;
						
		} else {

			//////SUCCESS
			///
			saturation._Saturation = 1;
			//						pattern._Alpha -= RATE.colorPlusRate / 10000f;
			saturation._ValueBrightness = 1;
			Instantiate (effectSuccess, transform.position, Quaternion.identity);
			Instantiate (effectSuccessBack,new Vector2(0,0), Quaternion.identity);
			endPlus ();
			audio.PlayOneShot (sSuccess);
			CancelInvoke ("minusColor");
			CancelInvoke ("plusColor");
			animation.Play ("bloom");
			//success
			STATE.mats++;
			Debug.Log ("mats" + STATE.mats + " " + STATE.matsAll);
			if (STATE.mats == STATE.matsAll)
				STATE._STATE = "gSUCCESS";
			
		}
	}
	void successAnim(){
			animation.Play ("success");
		
	}

	void endPlus ()
	{
		if (audio.isPlaying)
			audio.Stop ();
//				if (whiteMat.animation.isPlaying)
//						whiteMat.animation.Stop ();
		if (animation.isPlaying)
			animation.Stop ();
//				pattern.enabled = false;
//				oClone.SetActive (false);
//				saturation.enabled = true;
	}

	void starPlus ()
	{
		if (!audio.isPlaying)
			audio.Play ();
		//						if (!whiteMat.animation.isPlaying)
		//								whiteMat.animation.Play ();
		if (!animation.isPlaying)
			animation.Play ();
//				oClone.SetActive (true);

//				pattern.enabled = true;
//				saturation.enabled = false;
				
	}

	void minusColor ()
	{
		if (isMinus && saturation._Saturation > 0) {
			endPlus ();
			saturation._Saturation -= RATE.colorPlusRate / 3000f;
			saturation._ValueBrightness -= RATE.colorPlusRate / 6000f;
			
//						whiteMatSprite.color = new Color (1, 1, 1);
		}
	}

	void onColor ()
	{
		isPlus = true;
		isMinus = false;
	}

	void offColor ()
	{
		isMinus = true;
		isPlus = false;
	}
 
}
