using UnityEngine;
using System.Collections;

public class Ranking : MonoBehaviour
{
		Rank cRank;
		// Use this for initialization
		void Start ()
		{
				cRank = GetComponent<Rank> ();
				cRank.RankShow ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
