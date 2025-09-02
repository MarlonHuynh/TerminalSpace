using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PrintSnapManager : MonoBehaviour
{
    [Header("GameObjects")]
    public TerminalCommand commandTerminal; 
    public GameObject Camera3d;   
    public CameraMovement Camera3dScript;
    private ObjectOnScreenCheck objectOnScreenCheck;
    private GoalManager goalManager;
    public GameObject lowResDisplayObject;    // The UI gameobject to display the texture on
    public RenderTexture lowResRenderTexture; // The texture of the 3d cam
    public GameObject WarningClose;  
    public TextMeshProUGUI statusText; 
    public TextMeshProUGUI status2Text; 
    public TextMeshProUGUI printText;   
    public TextMeshProUGUI hookText;    
    private GameObject printBody; 
    private GameObject printJunk;
    public MiscSoundSFX miscSoundSFX; 
    [Header("States")]
    public bool canInteract = true;  
    public bool warning = false;  
    int dotCounter = 0;

    void Start()
    {
        objectOnScreenCheck = GetComponent<ObjectOnScreenCheck>();
        goalManager = GetComponent<GoalManager>();
    }
    public void snapPic()
    {
        if (canInteract)
        {
            canInteract = false;
            miscSoundSFX.playShutterBeep(); 
            printText.text = "Print";  
            StartCoroutine(bounceBtnText(printText)); 
            StartCoroutine(waitSnap());
            dotCounter = 0; 
        }
    }  

    public void hookObj(){
        if (canInteract)
        {
            canInteract = false;
            miscSoundSFX.playShutterBeep(); 
            hookText.text = "Hook";
            StartCoroutine(bounceBtnText(hookText)); 
            StartCoroutine(waitHook());
            dotCounter = 0;   
        } 
    }

    IEnumerator bounceBtnText(TextMeshProUGUI txt)
    { 
        txt.fontSize += 1f;
        yield return new WaitForSeconds(0.1f);
        txt.fontSize += 1f; 
        yield return new WaitForSeconds(0.1f);
        txt.fontSize -= 1f; 
        yield return new WaitForSeconds(0.1f);
        txt.fontSize -= 1f; 
    }
 

    IEnumerator waitHook()
    {    
        hookText.text = hookText.text + "."; 
        dotCounter ++; 
        yield return new WaitForSeconds(1f);
        if (dotCounter < 2)
        {
            StartCoroutine(waitHook());
        }
        else if (dotCounter >= 2)
        {
            // Wait 1 sec
            yield return new WaitForSeconds(1f);
            // Retrieve Junk object
            printJunk = Camera3d.GetComponent<CameraMovement>().currentJunk;
            objectOnScreenCheck.setTarget(printJunk);
            // Checks if centered or not centered, then display approprate text
            Color c = new Color(status2Text.color.r, status2Text.color.g, status2Text.color.b, 1f);
            status2Text.color = c;
            if (objectOnScreenCheck.checkInView() == true)
            {
                changeStatus2("Hooked.");
                status2Text.color = c;
                // Update goals
                if (printJunk.GetComponent<BodyStatus>().obtained == false)
                {
                    printJunk.GetComponent<BodyStatus>().obtained = true;
                    // Add to storage 
                    commandTerminal.addToStorage(printJunk.GetComponent<BodyStatus>().junkName, printJunk.GetComponent<BodyStatus>().junkValue);

                    if (printJunk.GetComponent<BodyStatus>().isJunk)
                    {
                        GetComponent<GoalManager>().currentJunkCount++;
                    }
                    GetComponent<GoalManager>().calcGoalText();
                    // DESTROY --Remove-- junk
                    Destroy(printJunk); 
                    //printJunk.SetActive(false);
                    Camera3d.GetComponent<CameraMovement>().junkState = 0;
                }
            }
            else if (objectOnScreenCheck.checkInView() == false)
            {
                changeStatus2("Nothing hooked.");
                status2Text.color = c;
            }
            // Return to Top View
            yield return new WaitForSeconds(2f);
            //hook = true;
            // Reset
            Color d = new Color(status2Text.color.r, status2Text.color.g, status2Text.color.b, 0f);
            status2Text.color = d;
            hookText.text = "Hook";
            // Allow player to interact again
            canInteract = true; 
        }
    }  
    IEnumerator waitSnap()
    {    
        printText.text = printText.text + "."; 
        dotCounter ++; 
        yield return new WaitForSeconds(1f);
        if (dotCounter < 2)
        {
            StartCoroutine(waitSnap());
        }
        else if (dotCounter >= 2)
        { // Snap Pic after 3 Dots
            // Display Pic
            lowResDisplayObject.GetComponent<RawImage>().texture = lowResRenderTexture;
            lowResDisplayObject.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
            Camera3d.GetComponent<Camera>().enabled = false;
            // Wait 1 sec
            yield return new WaitForSeconds(1f);
            // Alpha statusText or display Warning
            if (!warning)
            {
                changeStatusTextBasedOnText();
            }
            else if (warning)
            {
                WarningClose.SetActive(warning);
            }
            // Return to Top View
            yield return new WaitForSeconds(3f);
            // snap = true;
            // Reset
            Color d = new Color(statusText.color.r, statusText.color.g, statusText.color.b, 0f);
            statusText.color = d;
            status2Text.color = d;
            lowResDisplayObject.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
            if (warning)
            {
                WarningClose.SetActive(false);
            }
            Camera3d.GetComponent<Camera>().enabled = true;
            printText.text = "Print";
            // Allow player to interact again
            canInteract = true; 
        }
    }  
    
    public void changeStatusText(string str, string str2){
        statusText.text = str; 
        status2Text.text = str2; 
    }
    public void changeStatus(string str){
        statusText.text = str; 
    }
    public void changeStatus2(string str){
        status2Text.text = str; 
    }

    IEnumerator displayStatus()
    {
        Color c = new Color(statusText.color.r, statusText.color.g, statusText.color.b, 1f);
        if (statusText.text != "")
        {
            miscSoundSFX.playDingBeep();
        } 
        statusText.color = c;  // change to full alpha
        yield return new WaitForSeconds(1f);
         if (status2Text.text != "")
        {
            miscSoundSFX.playDingBeep();
        } 
        status2Text.color = c;  // change to full alpha
        yield return new WaitForSeconds(1f); 
    } 

    public void changeStatusTextBasedOnText(){
        string tempStatus1 = ""; 
        string tempStatus2 = ""; 

        int planetState = Camera3dScript.planetState;
        int junkState = Camera3dScript.junkState;
        //------------------------ JUNK IN RANGE -------------------- 
        if (junkState == 1)
        {  
            tempStatus1 += "Junk in proximity. Adjust rotation and hook to secure.";
            switch (planetState)
            {
                case 0:
                    tempStatus2 += "No planetary bodies detected.";
                    break;
                case 1:
                    tempStatus2 += "Too far from planetary body.";
                    break;
                case 2:
                    tempStatus2 += "Good distance from planetary body.";
                    break;
                case 3:
                    tempStatus2 += "Too close to planetary body!";
                    break;
                default:
                    tempStatus2 += "SCRIPT ERROR";
                    break;
            }
            changeStatusText(tempStatus1, tempStatus2);
            StartCoroutine(displayStatus());
            return;
        }

        //------------------------ NO JUNK IN RANGE -------------------- 
        switch (planetState){
            case 0: 
                tempStatus1 += "No planetary bodies detected.";  
                break; 
            case 1: 
                tempStatus1 += "Too far from planetary body."; 
                break; 
            case 2: // 2 is a good distance! Check if obtained
                BodyStatus status = Camera3dScript.currentBody.GetComponent<BodyStatus>();
                if (status != null)
                {
                    if (status.obtained)
                    {
                        tempStatus1 += "Already obtained";
                    }
                    else
                    {
                        tempStatus1 += "Good distance from planetary body."; 
                    } 
                } 
                break;  
            case 3: 
                tempStatus1 += "Too close to planetary body!"; 
                break; 
            default:  
                tempStatus1 += "SCRIPT ERROR"; 
                break; 
        } 
        if (planetState == 2 && Camera3dScript.currentBody != null){ // When state is 2 it means good distance
            printBody = Camera3dScript.currentBody; 
            objectOnScreenCheck.setTarget(printBody); 
            // Checks if centered 
            if (objectOnScreenCheck.checkInView() == true){  
                obtain(printBody);  // Update goals
            }
            else if (objectOnScreenCheck.checkInView() == false){
                tempStatus2 = "Not centered.";  
            } 
        }
        changeStatusText(tempStatus1, tempStatus2); 
        StartCoroutine(displayStatus());
    }
    public void obtain(GameObject o){
         if (printBody.GetComponent<BodyStatus>().obtained == false){
            printBody.GetComponent<BodyStatus>().obtained = true; 
            if (printBody.GetComponent<BodyStatus>().isStar){
                goalManager.currentStarCount++; 
            } 
            else if (printBody.GetComponent<BodyStatus>().isPlanet){
                goalManager.currentPlanetCount++; 
            }  
            else if (printBody.GetComponent<BodyStatus>().isJunk){
                goalManager.currentJunkCount++; 
            }  
            goalManager.calcGoalText(); 
        }
    }
}
