using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility.Inspector;

public class RockThrower : MonoBehaviour {

	public Rigidbody rockPrefab;

	public float throwSpeed = 10.0f;

	public static bool canThrow = false;
	public static bool crosshairEnabled = false;
	public GameObject uiManager;
	void Start () {

	}

	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			uiManager.GetComponent<UIManager> ().toggleCrosshair ();
			crosshairEnabled = !crosshairEnabled;
		}

		if (crosshairEnabled && Input.GetButtonDown ("Fire1") && canThrow) {
			Rigidbody newRock = Instantiate (rockPrefab, transform.position, transform.rotation) as Rigidbody;
			newRock.tag = "Rock";
			newRock.velocity = transform.forward * throwSpeed;
			Physics.IgnoreCollision (transform.root.GetComponent<Collider> (), newRock.GetComponent<Collider> (), true);
			Inventory.rocks--;
			uiManager.GetComponent<UIManager> ().inventoryUpdate ();

			if (Inventory.rocks < 1) {
				canThrow = false;
			}
		}
	}
}