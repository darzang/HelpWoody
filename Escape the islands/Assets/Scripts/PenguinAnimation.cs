using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinAnimation : MonoBehaviour {

	private GameObject Pathpoints;
	private Transform[] targets;
	private Transform targetTransform;
	private bool targetsReady = false;

	private Animation anim = new Animation ();

	private float speed = 5;

	void Start () {

		anim = this.gameObject.GetComponent<Animation> ();
		Pathpoints = GameObject.Find ("PathPoints");
		targets = new Transform[Pathpoints.transform.childCount];
		for (int i = 0; i < Pathpoints.transform.childCount; i++) {
			targets[i] = Pathpoints.transform.GetChild (i).transform;
		}
		targetsReady = true;
		int targetIndex = Random.Range (0, Pathpoints.transform.childCount);
		targetTransform = targets[targetIndex];
		this.transform.Rotate (0, 90, 0);
	}

	void Update () {
		float step = speed * Time.deltaTime;
		if (targetsReady) {

			if (targetTransform.position == this.transform.position) {
				int targetIndex = Random.Range (0, Pathpoints.transform.childCount);
				targetTransform = targets[targetIndex];
				transform.LookAt (targetTransform);
				this.transform.Rotate (0, 90, 0);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, targetTransform.position, step);
				if (this.transform.position.y > targetTransform.position.y) {
					anim.Play ("run");
					speed= 10;
				} else {
					anim.Play ("walk");
					speed = 5;
				}
			}

		}

	}

}