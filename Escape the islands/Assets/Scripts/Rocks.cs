using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocks : MonoBehaviour {

    private float switchTimer = 3.0f; // This timer is used to switch the rigibody and collider properties when the rock is thrown
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (SceneManager.GetActiveScene ().name == "FirstIsland") {
            if (this.GetComponent<Rigidbody> ().useGravity) {
                switchTimer -= Time.deltaTime;
                if (switchTimer <= 0) {
                    // When the timer is done, we switch the properties of the rock components so we can grab them and they don't go rolling in the deeeeeeeeeep 
                    this.GetComponent<Rigidbody> ().useGravity = false;
                    this.GetComponent<Rigidbody> ().isKinematic = true;
                    this.GetComponent<SphereCollider> ().isTrigger = true;
                }
            }
        }

    }

    void OnTriggerEnter (Collider col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.SendMessage ("RockPickup");
            Destroy (gameObject);
        }

    }

}