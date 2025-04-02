using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PrintLogic : MonoBehaviour
{ 
    public GameObject Camera3d;   
    private CameraMovement Camera3dScript; 
    public GameObject lowResDisplayObject;    // The UI gameobject to display the texture on
    public RenderTexture lowResRenderTexture; // The texture of the 3d cam
    public GameObject WarningClose;  
    public TextMeshProUGUI statusText; 
    public TextMeshProUGUI status2Text; 
    public TextMeshProUGUI printText;   
    public TextMeshProUGUI hookText;   
    private GameObject currentBody; 
    private GameObject currentJunk; 
    bool snap = true;  
    bool hook = true; 
    public bool warning = false;  
    int dotCounter = 0; 

    public void start(){
        Camera3dScript = Camera3d.GetComponent<CameraMovement>();
    }
    public void snapPic(){
        printText.text = "Print";   
        StartCoroutine(waitSnap());   
        dotCounter = 0; 
    }  

    public void hookObj(){   
        hookText.text = "Hook";  
        StartCoroutine(waitHook());   
        dotCounter = 0; 
    }
    IEnumerator waitHook()
    {    
        hookText.text = hookText.text + "."; 
        dotCounter ++; 
        yield return new WaitForSeconds(1f);  
        if (dotCounter < 2){
            StartCoroutine(waitHook());   
        }
        else if (dotCounter >= 2){  
            // Wait 1 sec
            yield return new WaitForSeconds(1f);  
            // Retrieve Junk object
            currentJunk = Camera3d.GetComponent<CameraMovement>().currentJunk; 
            GetComponent<ObjectOnScreenCheck>().setTarget(currentJunk); 
            // Checks if centered or not centered, then display approprate text
            Color c = new Color(status2Text.color.r,status2Text.color.g,status2Text.color.b, 1f);
            status2Text.color = c;   
            if (GetComponent<ObjectOnScreenCheck>().checkInView() == true){
                changeStatus2("Hooked."); 
                status2Text.color = c; 
                // Update goals
                if (currentJunk.GetComponent<BodyStatus>().obtained == false){
                    currentJunk.GetComponent<BodyStatus>().obtained = true; 
                    if (currentJunk.GetComponent<BodyStatus>().isJunk){
                        GetComponent<GoalManager>().goalJunk++; 
                    } 
                    GetComponent<GoalManager>().calcGoalText(); 
                    // Remove junk
                    currentJunk.SetActive(false); 
                    Camera3d.GetComponent<CameraMovement>().junkState = 0;  
                }
            }
            else if (GetComponent<ObjectOnScreenCheck>().checkInView() == false){
                changeStatus2("Nothing hooked."); 
                status2Text.color = c; 
            }   
            // Return to Top View
            yield return new WaitForSeconds(2f); 
            hook = true;   
            // Reset
            Color d = new Color(status2Text.color.r,status2Text.color.g,status2Text.color.b, 0f);
            status2Text.color = d;   
            hookText.text = "Hook";  
        }
    }  
    IEnumerator waitSnap()
    {    
        printText.text = printText.text + "."; 
        dotCounter ++; 
        yield return new WaitForSeconds(1f);  
        if (dotCounter < 2){
            StartCoroutine(waitSnap());   
        }
        else if (dotCounter >= 2){ // Snap Pic after 3 Dots
            // Display Pic
            lowResDisplayObject.GetComponent<RawImage>().texture = lowResRenderTexture;  
            lowResDisplayObject.GetComponent<RawImage>().color = new Color (1f, 1f, 1f, 1f);  
            Camera3d.GetComponent<Camera>().enabled = false;
            // Wait 1 sec
            yield return new WaitForSeconds(1f); 
            // Alpha statusText or display Warning
            if (!warning){
                changeStatusTextBasedOnText();  
            }
            else if (warning){ 
                WarningClose.SetActive(warning); 
            }
            // Return to Top View
            yield return new WaitForSeconds(2f); 
            snap = true;   
            // Reset
            Color d = new Color(statusText.color.r,statusText.color.g,statusText.color.b, 0f);
            statusText.color = d; 
            status2Text.color = d; 
            lowResDisplayObject.GetComponent<RawImage>().color = new Color (1f, 1f, 1f, 0f);  
            if (warning){ 
                WarningClose.SetActive(false); 
            } 
            Camera3d.GetComponent<Camera>().enabled = true; 
            printText.text = "Print";  
        }
    }  
     
    public void changeStatus(string str){
        statusText.text = str; 
    }
    public void changeStatus2(string str){
        status2Text.text = str; 
    }

    public void changeStatusTextBasedOnText(){
        int planetState = Camera3d.GetComponent<CameraMovement>().planetState;
        int junkState = Camera3d.GetComponent<CameraMovement>().junkState;
        if (junkState == 1){
            changeStatus("Junk in proximity. Adjust rotation and hook to secure."); 
        }
        else if (junkState != 1){ // Else if there's no junk in range
            Color c = new Color(statusText.color.r,statusText.color.g,statusText.color.b, 1f);
            statusText.color = c; 
            if (planetState == 2 && Camera3dScript.currentBody != null && currentBody.GetComponent<BodyStatus>().obtained == true){
                changeStatus("Already obtained.");
                return; 
            }
            switch (planetState){
                case 0: 
                    changeStatus("No planetary bodies or junk detected."); 
                    break; 
                case 1: 
                    changeStatus("Too far from planetary body."); 
                    break; 
                case 2: 
                    changeStatus("Good distance from planetary body."); 
                    break;  
                case 3: 
                    changeStatus("Too close to planetary body!"); 
                    break; 
                default:  
                    changeStatus("ERROR"); 
                    break; 
            } 
            if (planetState == 2 && Camera3dScript.currentBody != null){ // When state is 2 it means good distance
                currentBody = Camera3d.GetComponent<CameraMovement>().currentBody; 
                GetComponent<ObjectOnScreenCheck>().setTarget(currentBody); 
                // Checks if centered 
                if (GetComponent<ObjectOnScreenCheck>().checkInView() == true){
                    changeStatus2("Centered and obtained."); 
                    status2Text.color = c;  
                    obtain(currentBody);  // Update goals
                }
                else if (GetComponent<ObjectOnScreenCheck>().checkInView() == false){
                    changeStatus2("Not centered."); 
                    status2Text.color = c; 
                } 
            }
        }
    }

    public void obtain(GameObject o){
         if (currentBody.GetComponent<BodyStatus>().obtained == false){
            currentBody.GetComponent<BodyStatus>().obtained = true; 
            if (currentBody.GetComponent<BodyStatus>().isStar){
                GetComponent<GoalManager>().goalStar++; 
            } 
            else if (currentBody.GetComponent<BodyStatus>().isPlanet){
                GetComponent<GoalManager>().goalPlanet++; 
            }  
            else if (currentBody.GetComponent<BodyStatus>().isJunk){
                GetComponent<GoalManager>().goalJunk++; 
            }  
            GetComponent<GoalManager>().calcGoalText(); 
        }
    }
}
