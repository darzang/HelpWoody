using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushrooms : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SendMessage("MushroomPickup");
            Destroy(gameObject);
        }
    }


}
