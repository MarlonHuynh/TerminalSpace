using System.Collections; 
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;

public class TerminalTitleScreen : MonoBehaviour
{
    private const int MAIN_MENU = 0;
    private const int CREDITS = 1;
    public GameManager gameManager;
    public TextMeshProUGUI menuBody;
    public int menuCount = 0;
    public int screenID = 0; // 0 = main, 1 = credits
    void Start()
    {
        screenID = 0;
    }
    void Update()
    {
        // Space 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (screenID == MAIN_MENU) // If on title screen
            {
                switch (menuCount)
                {
                    case 0: // Play
                        pressPlay();
                        break;
                    case 1: // Credits
                        screenID = CREDITS;
                        displayCredits();
                        break;
                    case 2: // Quit
                        Application.Quit();
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        break;
                }
            }
            else if (screenID == CREDITS) // If on credits screen
            {
                screenID = MAIN_MENU;
                displayTitleText();
            }
        }

        // Up/Down 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (screenID == 0)
            {
                titleUp();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (screenID == 0)
            {
                titleDown();
            }
        }
    }
    private void titleDown()
    {
        if (menuCount > -1 && menuCount < 2)
        {
            menuCount++;
            displayTitleText();
        }
    }
    private void titleUp()
    {
        if (menuCount > 0 && menuCount < 3)
        {
            menuCount--;
            displayTitleText();
        }
    }
    private void displayTitleText()
    {
        string s = "";
        for (int i = 0; i < 3; i++)
        {
            if (menuCount == i) { s += "> "; }
            switch (i)
            {
                case 0:
                    s += "Play\n";
                    break;
                case 1:
                    s += "Credits\n";
                    break;
                case 2:
                    s += "Quit\n";
                    break;
            }
        }
        menuBody.text = s;
    }
    private void pressPlay()
    {
        gameManager.goShip();
    }
    private void displayCredits()
    {
        string s = "Created by Marlon 'sleepyzen' Huynh\nAdditional Credits coming soon.";
        s += "\n> Exit";
        menuBody.text = s;
    }
}