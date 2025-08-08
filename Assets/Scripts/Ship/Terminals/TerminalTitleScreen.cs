 
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.Audio;

public class TerminalTitleScreen : MonoBehaviour
{
    private const int MAIN_MENU = 0;
    private const int OPTIONS = 1;
    private const int CREDITS = 2;
    [Header("Assignables")]

    public GameManager gameManager;
    public TextMeshProUGUI menuBody;
    public Image blackScreenToFadeIn;
    public float fadeSpeed = 0.5f;
    public AudioMixer masterMixer; 
    
    private string[] menuItems = { "Play", "Options", "Credits", "Quit" };
    private string[] optionItems = { "Volume: ", "Back" }; 
    [Header("Debug")]

    public int menuCount = 0;
    public int optionsCount = 0; 
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
                        gameManager.pressPlay();
                        break;
                    case 1: // Options
                        screenID = OPTIONS;
                        displayOptionsText();
                        break;
                    case 2: // Credits
                        screenID = CREDITS;
                        displayCredits();
                        break;
                    case 3: // Quit
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
            else if (screenID == OPTIONS && optionsCount == 1) // If on options screen
            {
                screenID = MAIN_MENU;
                displayTitleText();
            }
        }

        // Up/Down 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (screenID == 0)
                titleUp(); 
            else if (screenID == 1) 
                OpUp(); 
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (screenID == 0)
                titleDown(); 
            else if (screenID == 1) 
                OpDown(); 
        }
    }
    private void titleDown()
    {
        if (menuCount < menuItems.Length - 1)
        {
            menuCount++;
            displayTitleText();
        }
    }
    private void titleUp()
    {
        if (menuCount > 0)
        {
            menuCount--;
            displayTitleText();
        }
    } 
    void OpUp()
    {
        if (optionsCount > 0)
        {
            optionsCount--;
        }
        displayOptionsText();
    }

    void OpDown()
    {
        if (optionsCount < optionItems.Length - 1)
        {
            optionsCount++;
        }
        displayOptionsText();
    }

    private void displayTitleText()
    {
        string temp = "";
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == menuCount)
            {
                temp += "> ";
            }
            temp += menuItems[i] + "\n";
        }
        menuBody.text = temp;
    }

    private void displayCredits()
    {
        string s = "Created by Marlon 'sleepyzen' Huynh\nAdditional Credits coming soon.";
        s += "\n> Exit";
        menuBody.text = s;
    } 
    
    void displayOptionsText()
    { 
        string temp = "";
        for (int i = 0; i < optionItems.Length; i++)
        {
            if (i == optionsCount)
            {
                temp += "> ";
            }
            temp += optionItems[i];
            if (i == 0)
            {
                temp += gameManager.volume + "/100";
            }
            temp += "\n";
        }
        menuBody.text = temp;
    }
    
    public void SetMasterVolume(float volume) // Vol from 0f to 1f 
    {
        // volume is expected in linear 0.0 to 1.0 range
        // Convert to decibel (logarithmic) scale:
        float dB;
        if (volume > 0)
            dB = Mathf.Log10(volume) * 20f;
        else
            dB = -80f; // silent

        masterMixer.SetFloat("MasterVolume", dB);
    }
 
}