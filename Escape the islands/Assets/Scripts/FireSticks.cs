using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSticks : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag=="Player"){
			col.SendMessage("StickPickup");
			Destroy(gameObject);
		}
	}
}
