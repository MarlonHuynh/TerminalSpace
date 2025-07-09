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
    public GameObject lowResDisplayObject;    // The UI gameobject to display the texture on
    public RenderTexture lowResRenderTexture; // The texture of the 3d cam
    public GameObject WarningClose;  
    public TextMeshProUGUI statusText; 
    public TextMeshProUGUI status2Text; 
    public TextMeshProUGUI printText;   
    public TextMeshProUGUI hookText;   
    private GameObject printBody; 
    private GameObject printJunk;
    [Header("States")]
    public bool canInteract = true; 
    //public bool snap = true;  
    //public bool hook = true; 
    public bool warning = false;  
    int dotCounter = 0;  
    public void snapPic(){ 
        if (canInteract)
        {
            printText.text = "Print";
            canInteract = false;
            StartCoroutine(waitSnap());
            dotCounter = 0; 
        }   
    }  

    public void hookObj(){    
        if (canInteract)
        {
            hookText.text = "Hook";
            canInteract = false;
            StartCoroutine(waitHook());
            dotCounter = 0; 
        } 
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
            GetComponent<ObjectOnScreenCheck>().setTarget(printJunk);
            // Checks if centered or not centered, then display approprate text
            Color c = new Color(status2Text.color.r, status2Text.color.g, status2Text.color.b, 1f);
            status2Text.color = c;
            if (GetComponent<ObjectOnScreenCheck>().checkInView() == true)
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
                    // Remove junk
                    printJunk.SetActive(false);
                    Camera3d.GetComponent<CameraMovement>().junkState = 0;
                }
            }
            else if (GetComponent<ObjectOnScreenCheck>().checkInView() == false)
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
            yield return new WaitForSeconds(2f);
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
    
    public void changeStatus(string str, string str2){
        statusText.text = str; 
        status2Text.text = str2; 
    }
    public void changeStatus(string str){
        statusText.text = str; 
    }
    public void changeStatus2(string str){
        status2Text.text = str; 
    }

    public void changeStatusToFullAlpha(){
        Color c = new Color(statusText.color.r,statusText.color.g,statusText.color.b, 1f);
        statusText.color = c;  // change to full alpha
        status2Text.color = c;  // change to full alpha
    }

    public void changeStatusTextBasedOnText(){
        Debug.Log("Changing status text!");
        int planetState = Camera3dScript.planetState;
        int junkState = Camera3dScript.junkState;
        if (junkState == 1){ // If Junk in range
            Debug.Log("Junk in range!");
            changeStatus("Junk in proximity. Adjust rotation and hook to secure.", ""); 
            changeStatusToFullAlpha(); 
            return;
        }
        // Else if there's no junk in range and already obtained
        Debug.Log("Junk not in range!");
        if (planetState == 2 && Camera3dScript.currentBody != null && Camera3dScript.currentBody != null){
            BodyStatus status = Camera3dScript.currentBody.GetComponent<BodyStatus>(); 
            if (status != null)
            { 
                if (status.obtained){
                    changeStatus("Already obtained.", ""); 
                    changeStatusToFullAlpha(); 
                    return;
                }

            }  
        } 
        // Else if there's no junk in range print the normal texts
        Debug.Log("Planet state: " + planetState); 
        switch (planetState){
            case 0: 
                changeStatus("No planetary bodies or junk detected.", ""); 
                break; 
            case 1: 
                changeStatus("Too far from planetary body.", ""); 
                break; 
            case 2: 
                changeStatus("Good distance from planetary body.", ""); 
                break;  
            case 3: 
                changeStatus("Too close to planetary body!", ""); 
                break; 
            default:  
                changeStatus("ERROR", ""); 
                break; 
        } 
        if (planetState == 2 && Camera3dScript.currentBody != null){ // When state is 2 it means good distance
            printBody = Camera3dScript.currentBody; 
            GetComponent<ObjectOnScreenCheck>().setTarget(printBody); 
            // Checks if centered 
            if (GetComponent<ObjectOnScreenCheck>().checkInView() == true){  
                obtain(printBody);  // Update goals
            }
            else if (GetComponent<ObjectOnScreenCheck>().checkInView() == false){
                changeStatus2("Not centered.");  
            } 
        }
        changeStatusToFullAlpha(); 
    }
    public void obtain(GameObject o){
         if (printBody.GetComponent<BodyStatus>().obtained == false){
            printBody.GetComponent<BodyStatus>().obtained = true; 
            if (printBody.GetComponent<BodyStatus>().isStar){
                GetComponent<GoalManager>().currentStarCount++; 
            } 
            else if (printBody.GetComponent<BodyStatus>().isPlanet){
                GetComponent<GoalManager>().currentPlanetCount++; 
            }  
            else if (printBody.GetComponent<BodyStatus>().isJunk){
                GetComponent<GoalManager>().currentJunkCount++; 
            }  
            GetComponent<GoalManager>().calcGoalText(); 
        }
    }
}
