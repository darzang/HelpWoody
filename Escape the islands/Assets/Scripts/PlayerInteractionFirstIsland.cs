using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class PlayerInteractionFirstIsland : MonoBehaviour {

    public GameObject uiManager;
    /********************Conversation Related ********************/
    int conversationState = 0;
    bool isTalking = false;
    bool canLeave = false;
    bool firstConversation = true;
    public bool firstInteraction = true;

    /******************** Still to sort out ********************/
    public static bool questStarted = false;

    // Use this for initialization
    void Start () {
        Debug.Log (SceneManager.GetActiveScene ().name);
    }

    // Update is called once per frame
    void Update () {

        if (Inventory.coconuts >= 5 && Inventory.mushrooms >= 10 && UIManager.hasMap) {
            canLeave = true;
        }

        if (isTalking) {
            GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = false;
            HandleConversation ();
        } else {
            RaycastHit hit;
            //Debug.DrawRay (transform.position, transform.forward * 3);   to see it in scene view ? 
            if (Physics.Raycast (transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.tag == "Woody") {
                uiManager.GetComponent<UIManager> ().enableInteractCrosshair ();

                if (firstInteraction) {
                    uiManager.GetComponent<UIManager> ().firstInteraction ();
                }
                if (Input.GetButtonDown ("ButtonR")) {
                    isTalking = true;
                    firstInteraction = false;
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
        uiManager.GetComponent<UIManager> ().setConversationText ("Great, I see you have enough food for us to go, are you ready ?");
        uiManager.GetComponent<UIManager> ().setNegativeAnswerText ("Just give me a few minutes");
        uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Let's find your brother!");
        uiManager.GetComponent<UIManager> ().enableNegativePanel ();

        if (Input.GetButtonDown ("ButtonE")) {
            SceneManager.LoadScene ("SecondIsland");
        } else if (Input.GetButtonDown ("ButtonQ")) {
            uiManager.GetComponent<UIManager> ().disableConversationPanel ();
            isTalking = false;
            GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = true;

        }
    }
    void HandleConversation () {
        if (!canLeave) {
            HandleFirstConversation ();
        } else {
            handleSecondConversation ();
        }
    }

    void handleUserAnswerForFirstConversation () {
        switch (conversationState) {
            case 0:
                if (Input.GetButtonDown ("ButtonE")) {
                    conversationState = 1;
                } else if (Input.GetButtonDown ("ButtonQ")) {
                    conversationState = 3;
                }
                break;
            case 1:
                if (Input.GetButtonDown ("ButtonE")) {
                    conversationState = 2;
                } else if (Input.GetButtonDown ("ButtonQ")) {
                    conversationState = 3;
                }
                break;
            case 2:
                if (Input.GetButtonDown ("ButtonE")) {
                    conversationState = 4;
                }
                break;
            case 3:
                if (Input.GetButtonDown ("ButtonE")) {
                    conversationState = 1;
                }
                break;
            case 4:
                if (Input.GetButtonDown ("ButtonE")) {
                    GameObject.Find ("Player").GetComponent<CharacterController> ().enabled = true;
                    uiManager.GetComponent<UIManager> ().disableConversationPanel ();
                    isTalking = false;
                    questStarted = true;
                }
                break;

        }
    }

    void handlePanelForFirstConversation () {
        switch (conversationState) {
            case 0:
                //First talk
                uiManager.GetComponent<UIManager> ().enableNegativePanel ();
                uiManager.GetComponent<UIManager> ().setConversationText ("Hey you, out there on the road, always doing what you’re told, can you help me?");
                uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Tell me more?");
                uiManager.GetComponent<UIManager> ().setNegativeAnswerText ("Sorry I have better things to do");
                break;
            case 1:
                //Player accepts to help once
                uiManager.GetComponent<UIManager> ().enableNegativePanel ();
                uiManager.GetComponent<UIManager> ().setConversationText ("I need someone to help me find my brother. He went on a mission on the snowy island a week ago and didn’t come back… I’m afraid something terrible could have happened to him, we need to hurry!");
                uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Sure, what do you need ?");
                uiManager.GetComponent<UIManager> ().setNegativeAnswerText ("Seems too much dangerous to me");
                break;
            case 2:
                // Player accepts to help twice
                uiManager.GetComponent<UIManager> ().disableNegativePanel ();
                uiManager.GetComponent<UIManager> ().setConversationText ("I’ll get the boat ready, meanwhile you could look for some food to bring with us. There’s some mushrooms around the forest, and coconut near the beach. Don’t be afraid to throw rocks at the coconuts to make them fall.");
                uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Well let's go then!");
                break;
            case 3:
                uiManager.GetComponent<UIManager> ().disableNegativePanel ();
                uiManager.GetComponent<UIManager> ().setConversationText ("Okay then, good luck finding a way out of here without the help of the only man owning a boat here!");
                uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("Hmm.. You were saying ?");
                break;
            case 4:
                // Woodys talks about the map before the end of the conversation
                uiManager.GetComponent<UIManager> ().disableNegativePanel ();
                uiManager.GetComponent<UIManager> ().setConversationText ("Oh and I almost forgot, I lost my map a few days ago in the forest, it could be useful for our journey if you could manage to find it");
                uiManager.GetComponent<UIManager> ().setPositiveAnswerText ("I'm on it !");
                break;

        }
    }
}