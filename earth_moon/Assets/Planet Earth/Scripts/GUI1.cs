using UnityEngine;
using System.Collections;

public class GUI1 : MonoBehaviour {

	public GameObject[] planets;
	public int currentPlanet;
	private GameObject displayedPlanet;
	// Use this for initialization
	void Start () {
//		currentPlanet = 0;
		displayedPlanet = Instantiate(planets[currentPlanet],transform.position,planets[currentPlanet].transform.rotation) as GameObject;
		displayedPlanet.transform.parent=transform;
	}
	
//	void OnGUI(){
//		GUILayout.BeginArea(new Rect(Screen.width-180, 20,150,500));
//		GUILayout.BeginVertical();
//
//		for(int i = 0; i<planets.Length; i++){
//			if(GUILayout.Button(planets[i].name)){
//				Destroy(displayedPlanet);
//				displayedPlanet = Instantiate(planets[i],transform.position,planets[i].transform.rotation) as GameObject;
//				displayedPlanet.transform.parent=transform;
//
//			}
//		}
//		GUILayout.EndVertical();
//
//
//		GUILayout.EndArea();
//	}
}
