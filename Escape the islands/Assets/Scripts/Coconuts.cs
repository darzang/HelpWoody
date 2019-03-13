using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconuts : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SendMessage("CoconutPickup");
            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "Rock")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;

        }
    }
}
