using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class PlayerInteractionSecondIsland : MonoBehaviour {

	public GameObject uiManager;

	/********************Conversation Related ********************/
	int conversationState = 0;
	bool isTalking = false;
	bool firstConversation = true;

	/******************** Quest related ********************/
	private static bool questStarted = false;

	private int questState = 0;
	public GameObject campfire;

	private GameObject firstWall;
	private GameObject secondWall;

	// Use this for initialization
	void Start () {
		conversationState = 0;
		firstWall = GameObject.Find ("FirstWall");
		secondWall = GameObject.Find ("SecondWall");
	}

	// Update is called once per frame
	void Update () {

		// If we have what it takes to go to next step
		if (Inventory.rocks >= 8 && Inventory.sticks >= 8 || Input.GetButtonDown ("ButtonQ")) {
			if (questState == 0) {
				questState = 1;
			}
		}

		if (isTalking) {
			if (GameObject.Find ("Player").GetComponent<CharacterController> ().enabled) {
				GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = false;
			}
			HandleConversation ();
		} else {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.tag == "Woody" && questState != 2) {
				uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();
				if (Input.GetButtonDown ("ButtonR")) {
					isTalking = true;
					uiManager.GetComponent<UIManager> ().disableInteractCrosshair ();
					uiManager.GetComponent<UIManager> ().disableNoticePanel ();
					uiManager.GetComponent<UIManager> ().enableConversationPanel ();
				}
			} else if (Physics.Raycast (transform.position, transform.forward, out hit, 6) && hit.collider.gameObject.name == "PenguinChef") {
				Debug.Log ("facing penguin");
				if (questState == 2) {
					uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();
					if (Input.GetButtonDown ("ButtonR")) {
						isTalking = true;
						uiManager.GetComponent<UIManager> ().disableInteractCrosshair ();
						uiManager.GetComponent<UIManager> ().disableNoticePanel ();
						uiManager.GetComponent<UIManager> ().enableConversationPanel ();
					}
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

	void handlePenguinConversation () {
		handlePanelForPenguinConversation ();
		handleUserAnswerForPenguinConversation ();
	}
	void HandleConversation () {
		switch (questState) {
			case 0:
				HandleFirstConversation ();
				break;
			case 1:
				handleSecondConversation ();
				break;
			case 2:
				handlePenguinConversation ();
				break;

		}
	}

	#region User Answers Handling
	void handleUserAnswerForFirstConversation () {
		switch (conversationState) {
			case 0:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 1;
				}
				break;
			case 1:
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
					campfire.gameObject.SetActive (true);
					questState = 2;
					Destroy (firstWall);
				}
				break;
		}
	}

	void handlePanelForPenguinConversation () {
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
					conversationState = 3;
				}
				break;
			case 3:
				if (Input.GetButtonDown ("ButtonE")) {
					GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = true;
					uiManager.GetComponent<UIManager> ().disableConversationPanel ();
					isTalking = false;
					conversationState = 0;
					questState = 3;
					Destroy(secondWall);
				}
				break;
		}
	}

	#endregion

	#region Panel Handling

	void handlePanelForSecondConversation () {
		switch (conversationState) {
			case 0:
				setConversationPanelTexts (
					"Oh great you have all we need for the campfire, let's get started !",
					"Found anything about your brother?"
				);
				break;
			case 1:
				setConversationPanelTexts (
					"Well I didn't see any sign of him, but I've seen that the penguin chef and his friends are back, maybe we should try to ask him",
					"And which one is it ?"
				);
				break;
			case 2:
				setConversationPanelTexts (
					"He's the only one standing on our side of the island, and he's a bit taller and brighter",
					"Okay, I'll go ask him"
				);
				break;

		}
	}

	void handlePanelForFirstConversation () {
		switch (conversationState) {
			case 0:
				uiManager.GetComponent<UIManager> ().disableNegativePanel ();
				setConversationPanelTexts (
					"So, Here we are, a bit cold right ? We should start a fire before freezing to death !",
					"What can I do to help ?"
				);
				break;
			case 1:
				//Player accepts to help once
				setConversationPanelTexts (
					"You could collect some sticks and somes rocks so that we can prepare the campfire, meanwhile I'll investigate on my missing brother",
					"I'm on it!"
				);
				break;
		}
	}

	void handleUserAnswerForPenguinConversation () {
		switch (conversationState) {
			case 0:
				uiManager.GetComponent<UIManager> ().disableNegativePanel ();
				setConversationPanelTexts (
					"Hello there traveller, whats brings you here ?",
					"Explain the situation"
				);
				break;
			case 1:
				setConversationPanelTexts (
					"I'm afraid this man was stealing our fishes when we got back so we had to lock him in a cage until we decide its fate.",
					"Try to find an agreement"
				);
				break;
			case 2:
				setConversationPanelTexts (
					"This man stole several fish from us, if you find a way to refund them, I could give you the key to the lock",
					"Offer coconuts and mushrooms instead"
				);
				break;
			case 3:
				setConversationPanelTexts (
					"That seems like a fair trade, I haven't had coconuts in year ! Here's the key, the cage is a bit further on my left.",
					"Take the key and go away"
				);
				break;
		}
	}

	void setConversationPanelTexts (string main, string positive, string negative = "") {
		uiManager.GetComponent<UIManager> ().setConversationText (main);
		uiManager.GetComponent<UIManager> ().setPositiveAnswerText (positive);
		uiManager.GetComponent<UIManager> ().setNegativeAnswerText (negative);
	}

	#endregion
}