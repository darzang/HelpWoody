using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject questPanel;
    public GameObject noticePanel;
    public GameObject mapPanel;
    public GameObject minimapPanel;
    public GameObject helpPanel;
    public GameObject conversationPanel;
    public GameObject negativeAnswerPanel;
    public GameObject positiveAnswerPanel;
    public GameObject inventoryPanel;
    public GameObject inventoryCoconut;
    public GameObject inventoryRocks;
    public GameObject inventoryMushrooms;
    public GameObject inventoryMap;
    public GameObject inventorySticks;
    public GameObject inventoryMatches;
    public GameObject inventoryFishes;

    public GameObject eyelidTop;
    public GameObject eyelidBottom;

    public Text coconutAmount;
    public Text mushroomAmount;
    public GameObject interactCrosshair;
    public GameObject crosshairImage;

    private float timeLeft;

    public static bool hasMap = false;
    private bool firstMapOpening = true;
    private bool firstQuestOpening = true;

    // Use this for initialization
    void Start () {
        eyelidBottom = GameObject.Find ("EyelidBottom");
        eyelidTop = GameObject.Find ("EyelidTop");
        OpenEyes();

        if (SceneManager.GetActiveScene ().name == "FirstIsland") {
            timeLeft = 4;
            noticePanel.GetComponentInChildren<Text> ().text = "Press H to open the controls panel";
            noticePanel.gameObject.SetActive (true);
            inventoryCoconut.GetComponentInChildren<Text> ().text = Inventory.coconuts.ToString ();
            inventoryRocks.GetComponentInChildren<Text> ().text = Inventory.rocks.ToString ();
            inventoryMushrooms.GetComponentInChildren<Text> ().text = Inventory.mushrooms.ToString ();
            inventoryMap.GetComponentInChildren<Text> ().text = hasMap ? "1" : "0";
        } else if (SceneManager.GetActiveScene ().name == "SecondIsland") {
            hasMap = true;
            firstMapOpening = false;
            firstQuestOpening = false;
            inventorySticks.GetComponentInChildren<Text> ().text = Inventory.sticks.ToString ();
            inventoryRocks.GetComponentInChildren<Text> ().text = Inventory.rocks.ToString ();
            inventoryMap.GetComponentInChildren<Text> ().text = "1";
        }
    }

    // Update is called once per frame
    void Update () {
        // inventoryPanel Management
        if (Input.GetButtonDown ("ButtonI")) {
            inventoryPanel.SetActive (!inventoryPanel.activeSelf);
        }

        // QuestPanel Management

        if (Input.GetButtonDown ("ButtonO")) {
            Blink ();
            if (PlayerInteractionFirstIsland.questStarted) {
                questPanel.SetActive (!questPanel.activeSelf);
            }
        }

        // HelpPanel Management

        if (Input.GetButtonDown ("ButtonH")) {
            helpPanel.SetActive (!helpPanel.activeSelf);
        }

        // Notice Panel Management
        if (noticePanel.gameObject.activeSelf) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                noticePanel.gameObject.SetActive (false);
                timeLeft = 4;
            }

        }

        // Map Management
        if (Input.GetButtonDown ("ButtonM") && hasMap) {

            if (!mapPanel.activeSelf) {
                mapPanel.gameObject.SetActive (true);
            } else {
                mapPanel.gameObject.SetActive (false);

                if (firstMapOpening && SceneManager.GetActiveScene ().name == "FirstIsland") {
                    noticePanel.GetComponentInChildren<Text> ().text = "You can press N to toggle the minimap";
                    noticePanel.gameObject.SetActive (true);
                    firstMapOpening = false;
                }
            }
        }

        // Minimap Management
        if (Input.GetButtonDown ("ButtonN") && !firstMapOpening) {
            minimapPanel.SetActive (!minimapPanel.activeSelf);
        }

        // Quest Panel Management
        if (PlayerInteractionFirstIsland.questStarted) {
            Image[] checkedIcons = questPanel.GetComponentsInChildren<Image> ();
            // checkedIcons[2] is the MapCheck, 3 is the foodCheck

            if (hasMap) {
                checkedIcons[2].GetComponentInChildren<Image> ().enabled = true;
            }
            if (Inventory.coconuts >= 5 && Inventory.mushrooms >= 10) {
                checkedIcons[3].GetComponentInChildren<Image> ().enabled = true;

            }

            if (firstQuestOpening) {
                questPanel.gameObject.SetActive (true);
                firstQuestOpening = false;
                noticePanel.GetComponentInChildren<Text> ().text = "Press O to toggle the quest panel";
                noticePanel.gameObject.SetActive (true);
            }
            updateQuestText ();

        }
    }

    public void inventoryUpdate () {
        if (SceneManager.GetActiveScene ().name == "FirstIsland") {
            inventoryCoconut.GetComponentInChildren<Text> ().text = Inventory.coconuts.ToString ();
            inventoryRocks.GetComponentInChildren<Text> ().text = Inventory.rocks.ToString ();
            inventoryMushrooms.GetComponentInChildren<Text> ().text = Inventory.mushrooms.ToString ();
            inventoryMap.GetComponentInChildren<Text> ().text = hasMap ? "1" : "0";
        } else if (SceneManager.GetActiveScene ().name == "SecondIsland") {
            inventorySticks.GetComponentInChildren<Text> ().text = Inventory.sticks.ToString ();
            inventoryRocks.GetComponentInChildren<Text> ().text = Inventory.rocks.ToString ();
            inventoryMap.GetComponentInChildren<Text> ().text = "1";

        }
    }

    public void updateQuestText () {
        if (SceneManager.GetActiveScene ().name == "FirstIsland") {
            coconutAmount.text = Inventory.coconuts.ToString () + " / 5";
            mushroomAmount.text = Inventory.mushrooms.ToString () + " / 10";
        }
    }

    public void FirstRockPickup () {
        noticePanel.gameObject.SetActive (true);
        noticePanel.GetComponentInChildren<Text> ().text = "Use the right click to toggle the crosshair";
        noticePanel.GetComponentInChildren<Text> ().fontSize = 35;
    }

    public void mapPickup () {
        hasMap = true;
        noticePanel.SetActive (true);
        noticePanel.GetComponentInChildren<Text> ().text = "Press M to open the map !";
        inventoryMap.GetComponentInChildren<Text> ().text = hasMap ? "1" : "0";
    }

    public void MatchesPickup () {
        inventoryMatches.GetComponentInChildren<Text> ().text = "1";
    }

    public void enableInteractCrosshair () {
        interactCrosshair.gameObject.SetActive (true);
        if (crosshairImage.activeSelf) {
            toggleCrosshair ();
        }
    }

    public void disableInteractCrosshair () {
        interactCrosshair.gameObject.SetActive (false);
    }

    public void firstInteraction () {
        noticePanel.SetActive (true);
        noticePanel.GetComponentInChildren<Text> ().text = "Press R to interact with Woody";
        noticePanel.GetComponentInChildren<Text> ().fontSize = 41;
    }

    public void enableNoticePanelWithTextAndSize (string text, int size) {
        noticePanel.GetComponentInChildren<Text> ().text = text;
        noticePanel.GetComponentInChildren<Text> ().fontSize = size;
        noticePanel.SetActive (true);

    }
    public void disableNoticePanel () {
        noticePanel.SetActive (false);

    }

    public void enableConversationPanel () {
        conversationPanel.SetActive (true);

    }

    public void disableConversationPanel () {
        conversationPanel.SetActive (false);

    }

    public void enableNegativePanel () {
        negativeAnswerPanel.SetActive (true);
    }
    public void disableNegativePanel () {
        negativeAnswerPanel.SetActive (false);
    }

    public void setConversationText (string speech) {
        conversationPanel.GetComponentInChildren<Text> ().text = speech;
    }

    public void setPositiveAnswerText (string speech) {
        positiveAnswerPanel.GetComponentInChildren<Text> ().text = speech;
    }

    public void setNegativeAnswerText (string speech) {
        negativeAnswerPanel.GetComponentInChildren<Text> ().text = speech;
    }

    public void toggleCrosshair () {
        crosshairImage.gameObject.SetActive (!crosshairImage.gameObject.activeSelf);
    }
    public void Blink () {
        eyelidBottom.GetComponent<Animation> ().Play ("EyelidBottomBlink");
        eyelidTop.GetComponent<Animation> ().Play ("EyelidTopBlink");
    }
    public void OpenEyes () {
        eyelidBottom.GetComponent<Animation> ().Play ("EyelidBottomOpen");
        eyelidTop.GetComponent<Animation> ().Play ("EyelidTopOpen");
    }

    public void CloseEyes () {
        eyelidBottom.GetComponent<Animation> ().Play ("EyelidBottomClose");
        eyelidTop.GetComponent<Animation> ().Play ("EyelidTopClose");
    }



}