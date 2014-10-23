using UnityEngine;
using System.Collections;

public class Trashes : MonoBehaviour
{
	//Camera cam;
	float speed;
	public static bool stop = false;
	public static int difficulty=1;
//	public float enemyCreateTime;
	public  string trashColor;
	GameObject GameManager;
	public int  createTerm ;
	int firstCreate = 48;
	float mTrashSpeed;
	public GameObject planeEnergy;
	
	// Use this for initialization
	void Start ()
	{
		GameManager = GameObject.Find ("GAMEMANAGER");
//		GameManager = GameObject.Find ("gauge plane");
//		speed = Game_Manager.trashSpeed;
//		mTrashSpeed = Game_Manager.trashSpeed;
		//transform.position = new Vector3 (-0.0165f, 0.4465f, 48.9f);
		//cam = GameObject.Find ("CameraTrash").camera;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//히트로 깨우기
		
		
		
//		if (0.6f - Game_Manager.trashSpeed * 0.001f * createTerm - 0.01f < transform.position.y && transform.position.y < 0.6f - Game_Manager.trashSpeed * 0.001f * createTerm + 0.01f && !stop) {
//			GameManager.SendMessage ("createTrash");
//		}
		if (stop) {
		}
		if (!stop) {
		
			if (-3 - firstCreate == transform.position.z) {
				GameManager.SendMessage ("createTrash");
			}
							
		
		
		
		
			speed = Game_Manager.trashSpeed;
			transform.position -= new  Vector3 (0f, 0.002f * speed, (int)speed);
			transform.localScale += new  Vector3 (0.001f * speed, 0.001f * speed, 0.001f * speed);

			
			if (transform.position.y < -0.7f) {
				stop = true;
				//난이도는 72의 약수 
				Game_Manager.trashSpeed = difficulty;
				firstCreate = createTerm;
			}
		
		}
	
	}

	void explosion ()
	{
		
			
		GameManager.SendMessage ("explosion", this.transform.position);
		stop = false;
			
			
		Destroy (this.gameObject);
		
	}
	
	
	
	
//	IEnumerator wrong ()
//	{
//		yield return new WaitForSeconds( 1f );
//		Game_Manager.wrongScreen.active = false;
//	}
	
	

	
}
