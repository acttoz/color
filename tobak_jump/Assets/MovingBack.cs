using UnityEngine;
using System.Collections;

public class MovingBack : MonoBehaviour
{
		public float position;
		// Use this for initialization
		void Start ()
		{
				position = 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void reset ()
		{
				position = 0;
		}

		void move ()
		{
				position -= 0.2f;
				Vector3 pointB = new Vector3 (0, position, 0);
				StartCoroutine (MoveObject (transform, transform.position, pointB, 0.1f));
		
		}

		IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
		{
				yield return new WaitForSeconds (0.1f);
				float i = 0.0f;
				float rate = 1.0f / time;
				while (i < 1.0f) {
						i += Time.deltaTime * rate;
						thisTransform.position = Vector3.Lerp (startPos, endPos, i);
						yield return null; 
				}
		
		
		}
}
