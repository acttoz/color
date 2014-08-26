using UnityEngine;
using System.Collections;

public class VALUE : MonoBehaviour
{
		public Color _DEEPBLUE, _LIGHTBLUE, _ORANGE;
		public static Color DEEPBLUE, LIGHTBLUE, ORANGE;
		bool isPlaying = false;
		public AudioClip[] note;
		public GameObject bar;
		public int[] prtMelody;
		public GameObject[] barposition;
		int audioTime;
		public static int[] melody = new int[]{
		2,9,9,9,9,9,
		9,9,9,9,9,9,	
		9,9,9,9,9,9,
		9,9,9,9,9,9
	    };
		// Use this for initialization
		void Start ()
		{
				DEEPBLUE = _DEEPBLUE;
				LIGHTBLUE = _LIGHTBLUE;
				ORANGE = _ORANGE;
		}
	
		// Update is called once per frame
		void Update ()
		{
				prtMelody = melody;
		}

		void audioStart ()
		{
				if (!isPlaying) {
						audioTime = 0;
						InvokeRepeating ("audioPlay", 0.5f, 0.5f);
						isPlaying = true;
				}
		}

		void audioPlay ()
		{
				
				bar.transform.position = new Vector2 (barposition [audioTime].transform.position.x, bar.transform.position.y);
				if (melody [audioTime] == 9) {
						int tempAudioTime = audioTime;
//						Debug.Log ("1 " + tempAudioTime);
						while (melody[tempAudioTime]==9) {
								tempAudioTime--;
//								Debug.Log ("2 " + tempAudioTime);
						}
		 
//						Debug.Log ("3 " + tempAudioTime);
				
						audio.PlayOneShot (note [melody [tempAudioTime]]);
//						Debug.Log ("9PLay");
				} else {
						audio.PlayOneShot (note [melody [audioTime]]);
//						Debug.Log ("justPlay");
				}


				if (audioTime > 22) {
						audioStop ();
				} else {
						audioTime++;
				}
		}

		void audioStop ()
		{
				CancelInvoke ("audioPlay");
				isPlaying = false;
				bar.transform.position = new Vector2 (barposition [24].transform.position.x, bar.transform.position.y);
		}

		 

}
