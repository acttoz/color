using UnityEngine;
using System.Collections;

public class src_balloon : MonoBehaviour
{
		public GameObject item;

		void OnTriggerEnter (Collider myTrigger)
		{
				if ((myTrigger.transform.tag == "boss" || myTrigger.transform.tag == "enemy")) {
						MANAGER.instance.StartCoroutine ("Remove");
						
				}
//				if ((myTrigger.transform.tag == "boss" || myTrigger.transform.tag == "enemy") && exist && !isUndead && isMonster) {
//						
//			
//			
//				}
				if (myTrigger.transform.tag == "item") {
						Item.instance.getItem ();
//						audio.PlayOneShot (itemSound);
						Instantiate (item, myTrigger.gameObject.transform.position, Quaternion.identity);
						Destroy (myTrigger.gameObject);
			
			
				}
		 
		}

		 
	 
}
