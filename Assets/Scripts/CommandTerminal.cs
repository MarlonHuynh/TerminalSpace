using System.Collections; 
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEditor.Rendering;
using System;

public class CommandTerminal : MonoBehaviour
{
    [Header("Required")]
    public CameraMovement cameraMovement; 
    public GameManager gameManager;
    public GoalManager goalManager;
    public LevelLoader levelLoader; 
    public GameObject menu;
    public TextMeshProUGUI menuTitle;
    public TextMeshProUGUI menuBody;
    [Header("Debugging Vars")]
    private List<string[]> validLevels; 
    public int currentSystem = 0;  
    public List<string> junkNameList = new List<string>();
    public List<float> junkValueList = new List<float>();
    public int itemCount = 0;
    public int maxItemCount = 10;
    public int screenID = 0;
    public int menuCount = 0;
    public int logCount = 0;
    public int prevAutoNavCount = 0; 
    public int autoNavCount = 0; 
    public bool canInteract = false;
    private bool hasStarted = false;
    void Start()
    {
        menu.SetActive(true);
        validLevels = levelLoader.validLevels; 
    }
    IEnumerator EnableInteractionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canInteract = true;
        displayMenu();
    }
    void Update()
    {
        if (!canInteract)
        {
            menuTitle.text = "[BOOTING SYSTEM...]";
            menuBody.text = "";

            if (!hasStarted)
            {
                hasStarted = true;
                StartCoroutine(EnableInteractionAfterDelay(2f));
            }
        }
        else
        {
            Navigate();
        }
    } 
    void OnDisable()
    {
        canInteract = false;
        hasStarted = false;
    }
    public void addToStorage(string name, float value)
    {
        junkNameList.Add(name);
        junkValueList.Add(value);
    }

    /*
    ids:  0 - Main 
    1 - Remote Nav
    2 - Log
    3 - Storage
    4 - Exit  
    */
    private void Navigate()
    {
        // Back key for astronomicon
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (screenID)
            {
                case 0: // Main menu 
                    canInteract = false;
                    GameObject.Find("GameManager").GetComponent<GameManager>().goShip();
                    break;
                case 1: // AutoNav
                    displayMenu();
                    break;
                case 2: // Log
                    displayMenu();
                    break;
                case 3: // Storage
                    displayMenu();
                    break; 
                default: // Exit
                    canInteract = false;
                    GameObject.Find("GameManager").GetComponent<GameManager>().goShip();
                    break;
            }
        }
        // Space 
        if (Input.GetKeyDown(KeyCode.Space) && canInteract)
        {
            if (screenID == 0)
            { // On main screen 
                switch (menuCount)
                {
                    case 0: // Click remote nab
                        displayAutoNav();
                        break;
                    case 1: // Click Log 
                        if (validLevels[currentSystem][1] == "Inherent" || validLevels[currentSystem][1] == "Complete")
                        {
                            menuTitle.text = "Log";
                            menuBody.text = "System already logged.";
                            StartCoroutine(InlineRoutine()); 
                            IEnumerator InlineRoutine()
                            {
                                yield return new WaitForSeconds(1.5f);
                                displayMenu(); 
                            }
                        }
                        else
                        {
                            displayLog();
                        }
                        break;
                    case 2: // Click Storage
                        displayStorage();
                        break;
                    case 3: // Click Exit
                        canInteract = false;
                        GameObject.Find("GameManager").GetComponent<GameManager>().goShip();
                        break;
                }
            }
            else if (screenID == 1) // On nav screen
            {
                if (autoNavCount == validLevels.Count) // Exit
                {
                    displayMenu();
                }
                else
                { // Else do the appropriate navigation
                    currentSystem = autoNavCount; // Update the (current) system var
                    displayAutoNav(); // Update display
                    // Only loads level if player clicks on a different level
                    if (prevAutoNavCount != autoNavCount)
                    {
                        cameraMovement.takeHyperjumpFuel(); 
                        displayAutoNav(); // Update display
                        levelLoader.loadLevel(autoNavCount); 
                    }
                    prevAutoNavCount = autoNavCount; // Reset prevAutoNavCount so that the currently loaded system doesn't reload if player clicks aagin
                }
            }
            else if (screenID == 2)
            { // On log screen  
                switch (logCount)
                {
                    case 0: // No
                        displayMenu();
                        break;
                    case 1: // Yes
                        logProgress();
                        break;
                }
            }
            else if (screenID == 3)
            { // On Storage Screen
                displayMenu();
            }
        }
        // Up/Down 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (screenID == 0) // On main menu
            {
                menuUp();
            }
            else if (screenID == 1) // On nav
            {
                navUp(); 
            }
            else if (screenID == 2) // On log 
            {
                logUp();
            }
            else if (screenID == 3) // On storage
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (screenID == 0) // On main menu
            {
                menuDown();
            }
            else if (screenID == 1) // on nav
            {
                navDown(); 
            }
            else if (screenID == 2) // On log
            {
                logDown();
            }
            else if (screenID == 3) // on Storage
            {

            }
        }
    }
    private void menuDown()
    {
        if (menuCount > -1 && menuCount < 3)
        {
            menuCount++;
            displayMenu();
        }
    }
    private void menuUp()
    {
        if (menuCount > 0 && menuCount < 4)
        {
            menuCount--;
            displayMenu();
        }
    }
    private void logDown()
    {
        if (logCount == 0)
        {
            logCount++;
            displayLog();
        }
    }
    private void logUp()
    {
        if (logCount == 1)
        {
            logCount--;
            displayLog();
        }
    }
    private void navDown()
    {
         if (autoNavCount > -1 && autoNavCount < validLevels.Count)
        {
            prevAutoNavCount = autoNavCount; 
            autoNavCount++;
            displayAutoNav();
        }
    }
    private void navUp()
    {
        if (autoNavCount > 0 && autoNavCount < validLevels.Count + 1)
        {
            prevAutoNavCount = autoNavCount; 
            autoNavCount--;
            displayAutoNav();
        }
    }
    private void displayMenu()
    {
        screenID = 0;
        string s = "";
        switch (menuCount)
        {
            case 0:
                s = "> Remote Navigation\nLog\nStorage\nExit";
                break;
            case 1:
                s = "Remote Navigation\n> Log\nStorage\nExit";
                break;
            case 2:
                s = "Remote Navigation\nLog\n> Storage\nExit";
                break;
            case 3:
                s = "Remote Navigation\nLog\nStorage\n> Exit";
                break;
            default:
                break;
        }
        menuTitle.text = "Command";
        menuBody.text = s;
    }
    
    public void displayAutoNav()
    {
        screenID = 1;
        string s = "";
        // Display
        s += "Select location to hyperjump to.\n";
        s += "Fuel: " + cameraMovement.fuel + " / " + cameraMovement.maxFuel + " (max)\n";
        s += "Cost: 20 fuel\n\n"; 
        for (int i = 0; i < validLevels.Count + 1; i++)
        {
            if (autoNavCount == i) // Navigation carrot
            {
                s += "> ";
            }
            if (i < validLevels.Count) // Loop through levels
            {
                if (i == currentSystem)
                    s += "(Current System) ";
                s += validLevels[i][0] + "\n";
            }
            else if (i == validLevels.Count) // One past levels
            {
                s += "Exit\n";
            }
        }
        menuTitle.text = "Remote Navigation";
        menuBody.text = s;
    }

    private void displayLog()
    {
        screenID = 2;
        string s = "";
        // Check Junk 
        if (goalManager.isJunkComplete() == false)
        {
            s += "<color=red>CRITICAL: Not collecting all the junk in the system will result in severe disciplinary action.</color>\n";
        }
        else
        {
            s += "All junk obtained.\n";
        }
        // Check bodies
        if (goalManager.isBodiesComplete() == false)
        {
            s += "<color=orange>WARNING: Undocumented orbital bodies will result in reduced pay.</color=orange>\n";
        }
        else
        {
            s += "All bodies documented.\n";
        }
        s += "Log your progress on current system?\n(Unlocks next mission if applicable)\n";
        switch (logCount)
        {
            case 0:
                s += "> No\nYes";
                break;
            case 1:
                s += "No\n> Yes";
                break;
            default:
                break;
        }
        menuTitle.text = "Log";
        menuBody.text = s;
    } 

    private void displayStorage()
    {
        screenID = 3;
        string s = "Junk: " + junkNameList.Count + " / " + maxItemCount + " (max)\nCurrently held objects in storage:\n";
        if (junkNameList.Count <= 0)
        {
            s += "None";
        }
        for (int i = 0; i < junkNameList.Count; i++)
        {
            s += junkNameList[i] + " [" + junkValueList[i] + "C]\n";
        }
        menuTitle.text = "Storage";
        menuBody.text = s;
    }

    private void logProgress()
    {
        StartCoroutine(logDelay());
    }
    IEnumerator logDelay()
    {
        menuTitle.text = "Logging.";
        menuBody.text = "";
        yield return new WaitForSeconds(1f);
        menuTitle.text += ".";
        yield return new WaitForSeconds(1f);
        menuTitle.text += ".";
        yield return new WaitForSeconds(1f);
        if (goalManager.isJunkComplete() == true)
        {
            menuBody.text += "Logging successful.\n";
            yield return new WaitForSeconds(1.5f);
            menuBody.text += "Please report to your assigned handler to deliver collected junk or proceed to the next mission using navigation terminal.\n";
            levelLoader.markCompleteAndAddNewLevel(); 
            canInteract = true;
        }
        if (goalManager.isJunkComplete() == false)
        {
            menuBody.text += "<color=orange>Logging unsuccessful.</color=orange>\n";
            yield return new WaitForSeconds(0.3f);
            menuBody.text += "<color=red>CRITICAL WARNING: Missing salvage.</color=red>\n";
            yield return new WaitForSeconds(0.3f);
            menuBody.text += "<color=red>CRITICAL WARNING: Third infraction recorded.</color=red>\n";
            yield return new WaitForSeconds(0.3f);
            menuBody.text += "<color=orange>SYSTEM: Protocol-LOSS engaged.</color=orange>\n";
            yield return new WaitForSeconds(0.3f);
            menuBody.text += "<color=orange>SYSTEM: System lockdown in effect.</color=orange>\n";
            yield return new WaitForSeconds(0.3f);
            menuBody.text += "<color=orange>SYSTEM: Auto-navigation to Handler-88 authorized.</color=orange>\n";
            yield return new WaitForSeconds(0.3f);
            menuBody.text += "<color=orange>SYSTEM: Airlock override authorized.</color=orange>\n";
            yield return new WaitForSeconds(0.5f);
            gameManager.triggerEjectCutscene(); 
        } 
    }
}
