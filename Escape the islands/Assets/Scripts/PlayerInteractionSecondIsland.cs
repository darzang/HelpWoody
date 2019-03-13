using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class PlayerInteractionSecondIsland : MonoBehaviour {

	public GameObject uiManager;

	public GameObject campfire;

	/********************Conversation Related ********************/
	int conversationState = 0;
	bool isTalking = false;
	bool firstConversation = true;

	/******************** Still to sort out ********************/
	public static bool questStarted = false;

	public int questState = 0;

	// Use this for initialization
	void Start () {
		conversationState = 0;
	}

	// Update is called once per frame
	void Update () {

		// TO REMOVE 

		if (Input.GetButtonDown ("ButtonQ")) {
			questState = 1;

		}

		// If we have what it takes to go to next step
		if (Inventory.rocks >= 8 && Inventory.sticks >= 8 && Inventory.hasMatches) {
			if (questState == 0) {
				questState = 1;
			}
		}

		if (isTalking) {
			GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = false;
			HandleConversation ();
		} else {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.tag == "Woody") {
				uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();
				if (Input.GetButtonDown ("ButtonR")) {
					isTalking = true;
					uiManager.GetComponent<UIManager> ().disableInteractCrosshair ();
					uiManager.GetComponent<UIManager> ().disableNoticePanel ();
					uiManager.GetComponent<UIManager> ().enableConversationPanel ();
				}
			} else {
				uiManager.GetComponent<UIManager> ().disableInteractCrosshair ();
			}
		}
	}

	void HandleFirstConversation () {
		handlePanelForFirstConversation ();
		handleUserAnswerForFirstConversation ();
	}
	void handleSecondConversation () {
		handlePanelForSecondConversation ();
		handleUserAnswerForSecondConversation ();
	}
	void HandleConversation () {
		switch (questState) {
			case 0:
				HandleFirstConversation ();
				break;
			case 1:
				handleSecondConversation ();
				break;

		}
	}

	void handleUserAnswerForFirstConversation () {
		switch (conversationState) {
			case 0:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 1;
				}
				break;
			case 1:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 2;
				}
				break;
			case 2:
				if (Input.GetButtonDown ("ButtonE")) {
					GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = true;
					uiManager.GetComponent<UIManager> ().disableConversationPanel ();
					isTalking = false;
					conversationState = 0;
				}
				break;
		}
	}

	void handleUserAnswerForSecondConversation () {
		switch (conversationState) {
			case 0:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 1;
				}
				break;
			case 1:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 2;
				}
				break;
			case 2:
				if (Input.GetButtonDown ("ButtonE")) {
					GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = true;
					uiManager.GetComponent<UIManager> ().disableConversationPanel ();
					isTalking = false;
					conversationState = 0;
					campfire.gameObject.SetActive(true);
				}
				break;
		}
	}

	void handlePanelForSecondConversation () {
		switch (conversationState) {
			case 0:
				uiManager.GetComponent<UIManager> ().setConversationText ("Oh great you have all we need for the campfire, let's get started!");
				uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Found anything about your brother?");
				break;
			case 1:
				uiManager.GetComponent<UIManager> ().setConversationText ("Well I didn't see any sign of him, but I've seen that the penguin king and his friends are back, maybe we should try to ask him");
				uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("And which one is the king?");
				break;
			case 2:
				uiManager.GetComponent<UIManager> ().setConversationText ("He's... different, you should be able to notice him without too much trouble");
				uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Okay, I'm on it!");
				break;

		}
	}

	void handlePanelForFirstConversation () {
		switch (conversationState) {
			case 0:
				//First talk
				uiManager.GetComponent<UIManager> ().disableNegativePanel ();
				uiManager.GetComponent<UIManager> ().setConversationText ("So, Here we are, a bit cold right ? We should start a fire before freezing to death !");
				uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("What can I do to help ?");
				break;
			case 1:
				//Player accepts to help once
				uiManager.GetComponent<UIManager> ().setConversationText ("You could collect some sticks and somes rocks so that we can prepare the campfire, meanwhile I'll investigate on my missing brother");
				uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Anything else ?");
				break;
			case 2:
				uiManager.GetComponent<UIManager> ().setConversationText ("Oh yeah, my brother brought a chest with him when he left, there may be some matches inside. Find the chest and we won't freeze to death !");
				uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("I'm on it !");
				break;

		}
	}
}