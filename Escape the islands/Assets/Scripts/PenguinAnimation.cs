using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinAnimation : MonoBehaviour {

	private GameObject Pathpoints;
	private Transform[] targets;
	private Transform targetTransform;
	private bool targetsReady = false;

	private float speed = 5;

	void Start () {

		Pathpoints = GameObject.Find ("PathPoints");
		targets = new Transform[Pathpoints.transform.childCount];
		for (int i = 0; i < Pathpoints.transform.childCount; i++) {
			targets[i] = Pathpoints.transform.GetChild (i).transform;
		}
		targetsReady = true;
		int targetIndex = Random.Range (0, Pathpoints.transform.childCount);
		targetTransform = targets[targetIndex];
		Debug.Log ("Target is at " + targetTransform.position);
		this.transform.Rotate (0, 90, 0);

	}

	void Update () {
		if (targetsReady) {

			float step = speed * Time.deltaTime;

			if (targetTransform.position == this.transform.position) {
				int targetIndex = Random.Range (0, Pathpoints.transform.childCount);
				targetTransform = targets[targetIndex];
				transform.LookAt (targetTransform);
				this.transform.Rotate (0, 90, 0);
				Debug.Log ("New target");

			} else {
				transform.position = Vector3.MoveTowards (transform.position, targetTransform.position, step);
			}

		}

	}

}