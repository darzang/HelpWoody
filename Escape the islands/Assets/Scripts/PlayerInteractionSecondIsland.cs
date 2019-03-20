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

	private GameObject buzz;
	private GameObject door;

	private bool buzzIsFollowing = false;

	private GameObject player;
	private float speed = 5;

	// Use this for initialization
	void Start () {
		conversationState = 0;
		firstWall = GameObject.Find ("FirstWall");
		secondWall = GameObject.Find ("SecondWall");
		buzz = GameObject.Find ("Buzz");
		door = GameObject.Find ("door");
		player = GameObject.Find ("Player");
	}

	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;

		if (buzzIsFollowing) {
			buzz.transform.LookAt (player.transform);
			if (Vector3.Distance (player.transform.position, buzz.transform.position) >= 7.5) {
				buzz.transform.position = Vector3.MoveTowards (buzz.transform.position, player.transform.position, step);
				buzz.GetComponent<Animation> ().Play ("Walk");
			} else {
				buzz.GetComponent<Animation> ().Play ("Idle");

			}
			Debug.Log (Vector3.Distance (player.transform.position, buzz.transform.position));
		}

		if (Input.GetButtonDown ("ButtonI")) {
			unlockBuzz ();
		}

		// If the player picked up enough materials to advance in the quest
		if (Inventory.rocks >= 8 && Inventory.sticks >= 8 || Input.GetButtonDown ("ButtonQ")) {
			if (questState == 0) {
				questState = 1;
			}
		}
		if (isTalking) {
			GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = false;
			HandleConversation ();
		} else {
			RaycastHit hit;

			// If we're in front of Woody
			if (Physics.Raycast (transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.tag == "Woody" && questState != 2) {
				uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();
				if (Input.GetButtonDown ("ButtonR")) {
					LaunchConversation ();
				}
			}
			// If we're facing the penguin chef
			else if (Physics.Raycast (transform.position, transform.forward, out hit, 6) && hit.collider.gameObject.name == "PenguinChef" && questState == 2) {
				uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();
				if (Input.GetButtonDown ("ButtonR")) {
					LaunchConversation ();
				}
			} else if (Physics.Raycast (transform.position, transform.forward, out hit, 6) && hit.collider.gameObject.name == "Buzz" && questState == 3) {
				uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();
				if (Input.GetButtonDown ("ButtonR")) {
					LaunchConversation ();
				}
			}
			// If we're facing the first invisible wall before being supposed to
			else if (Physics.Raycast (transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.name == "FirstWallCollider") {
				uiManager.GetComponent<UIManager> ().enableNoticePanelWithTextAndSize ("I think I should go collect what Woody asked me before going further...", 20);
			}
			// If we're facing the second invisible wall before being supposed to
			else if (Physics.Raycast (transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.name == "SecondWallCollider") {
				uiManager.GetComponent<UIManager> ().enableNoticePanelWithTextAndSize ("I think I should go talk to the penguin chef before going further...", 20);
			}
			// Otherwise
			else {
				uiManager.GetComponent<UIManager> ().disableInteractCrosshair ();
			}
		}
	}

	void LaunchConversation () {
		isTalking = true;
		uiManager.GetComponent<UIManager> ().disableInteractCrosshair ();
		uiManager.GetComponent<UIManager> ().disableNoticePanel ();
		uiManager.GetComponent<UIManager> ().enableConversationPanel ();
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

	void handleBuzzConversation () {
		handlePanelForBuzzConversation ();
		handleUserAnswerForBuzzConversation ();
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
			case 3:
				handleBuzzConversation ();
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
					Destroy (secondWall);
				}
				break;
		}
	}

	void handlePanelForBuzzConversation () {
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
					conversationState = 4;
				}
				break;
			case 4:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 5;
				}
				break;
			case 5:
				if (Input.GetButtonDown ("ButtonE")) {
					conversationState = 6;
				}
				break;
			case 6:
				if (Input.GetButtonDown ("ButtonE")) {
					GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = true;
					uiManager.GetComponent<UIManager> ().disableConversationPanel ();
					isTalking = false;
					conversationState = 0;
					questState = 4;
					unlockBuzz ();
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

	void handleUserAnswerForBuzzConversation () {
		switch (conversationState) {
			case 0:
				uiManager.GetComponent<UIManager> ().disableNegativePanel ();
				setConversationPanelTexts (
					"Hey you there ! Can you help me ? These monsters locked me up here !",
					"Why is that?"
				);
				break;
			case 1:
				setConversationPanelTexts (
					"Well I may have been trying to steal some fish from their stock, but I was starving and didn't have other choice!",
					"Woody sent me here"
				);
				break;
			case 2:
				setConversationPanelTexts (
					"I knew I could count on him ! He's a terrific brother isn't he ?",
					"Yeah, not like you apparently"
				);
				break;
			case 3:
				setConversationPanelTexts (
					"What do you mean by that you little prick ?! Do you have something to say ?",
					"Hmm.. I have the key to your lock"
				);
				break;
			case 4:
				setConversationPanelTexts (
					"Oh, What are you waiting for then ?!",
					"A little acknowledgement maybe ?"
				);
				break;
			case 5:
				setConversationPanelTexts (
					"Okay fine, Thank you for saving my life ! Happy now ?",
					"I guess it won't get better anyway"
				);
				break;
			case 6:
				setConversationPanelTexts (
					"Come on now, I'm freezing out here !",
					"*Open the door*"
				);
				break;
		}
	}

	void setConversationPanelTexts (string main, string positive, string negative = "") {
		uiManager.GetComponent<UIManager> ().setConversationText (main);
		uiManager.GetComponent<UIManager> ().setPositiveAnswerText (positive);
		uiManager.GetComponent<UIManager> ().setNegativeAnswerText (negative);
	}

	void unlockBuzz () {
		uiManager.GetComponent<UIManager> ().Blink ();
		door.GetComponent<Animation> ().Play ("DoorOpening");
		buzz.transform.SetPositionAndRotation (new Vector3 (83.4f, 31.2f, 240.2f), buzz.transform.rotation);
		buzzIsFollowing = true;
	}

	#endregion
}