using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{

    public static int mushrooms = 0;
    public static int rocks = 0;
    public static int coconuts = 0;
    public static int sticks = 0;
    public GameObject uiManager;
    public AudioSource audioSource;

    private bool firstRockPickUp = true;
    public  static bool hasMatches = false;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FirstIsland")
        {
            mushrooms = 0;
            rocks = 0;
            coconuts = 0;
        }
        else if (SceneManager.GetActiveScene().name == "SecondIsland")
        {
            sticks = 0;
            // Rocks ???
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void MushroomPickup()
    {
        mushrooms++;
        playPickUpSound();
         uiManager.GetComponent<UIManager> ().inventoryUpdate();
    }



    void RockPickup()
    {
        if (firstRockPickUp)
        {
            firstRockPickUp = false;
             uiManager.GetComponent<UIManager> ().FirstRockPickup();
        }
        rocks++;
        RockThrower.canThrow = true;
        playPickUpSound();
         uiManager.GetComponent<UIManager> ().inventoryUpdate();
    }

    void CoconutPickup()
    {
        coconuts++;
        playPickUpSound();
         uiManager.GetComponent<UIManager> ().inventoryUpdate();
    }

    void StickPickup()
    {
        sticks++;
        playPickUpSound();
         uiManager.GetComponent<UIManager> ().inventoryUpdate();
    }

    void playPickUpSound()
    {
        audioSource.Play();
    }

    void MapPickup()
    {
        playPickUpSound();
         uiManager.GetComponent<UIManager> ().mapPickup();

    }

    void MatchesPickup(){
        playPickUpSound();
        uiManager.GetComponent<UIManager>().MatchesPickup();
    }


}