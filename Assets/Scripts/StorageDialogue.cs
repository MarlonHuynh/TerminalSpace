using System.Collections; 
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections.Generic;

public class StorageDialogue : MonoBehaviour
{
    public List<string> storageList = new List<string>();
    public int itemCount = 0; 
    public int maxItemCount = 0; 
    public GameObject menu;  
    public TextMeshProUGUI menuTitle; 
    public TextMeshProUGUI menuBody;     
    public int screenID = 0; 
    public int menuCount = 0;  
    private int logCount = 0;  
    public bool canInteract = false;    
    void Start(){ 
        menu.SetActive(true); 
    }
    IEnumerator EnableInteractionAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        canInteract = true; 
        switchMenuText(); 
    }  
    void Update(){
        if (!canInteract) {
            menuTitle.text = "[BOOTING SYSTEM...]"; 
            menuBody.text = "";
            StartCoroutine(EnableInteractionAfterDelay(2f));
        }
        else {
            Navigate(); 
        }
    } 
    void OnDisable(){
        canInteract = false; 
    } 
    public void addToStorage(string s){
        storageList.Add(s); 
    }
    public void addItemCount(){
        itemCount++;
    }
    public void setItemCount(int i){
        itemCount = i; 
    }
    public void setMaxItemCount(int i){
        maxItemCount = i; 
    }
     
    /*
    ids:  
    */
    private void Navigate() {
        // Back key for astronomicon
        if (Input.GetKeyDown(KeyCode.E)){
            switch (screenID) {
                case 0: // Main menu
                    canInteract = false; 
                    GameObject.Find("GameManager").GetComponent<GameManager>().goShip(); 
                    break;
                case 1: // Log
                    screenID = 0; 
                    break;
                case 2: // Storage
                    screenID = 0; 
                    break;  
                default: // Exit
                    canInteract = false; 
                    GameObject.Find("GameManager").GetComponent<GameManager>().goShip(); 
                    break;
            }
        } 
        // Space 
        if (Input.GetKeyDown(KeyCode.Space) && canInteract) {
            if (screenID == 0){ 
            }
        }
        // Up/Down 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            if (screenID == 0){
                menuUp();
            } 
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if (screenID == 0){
                menuDown();
            } 
        }
    }  
    private void menuDown(){
        if (menuCount > -1 && menuCount < 2){
            menuCount++; 
            switchMenuText(); 
        }
    }
    private void menuUp(){
        if (menuCount > 0 && menuCount < 3){
            menuCount--;  
            switchMenuText(); 
        }
    } 
    private void logDown(){
        if (logCount == 0){
            logCount++; 
            switchLogText(); 
        }
    }
    private void logUp(){
        if (logCount == 1){
            logCount--;  
            switchLogText(); 
        }
    } 
    private void switchMenuText(){
        string s = ""; 
        switch (menuCount){
            case 0: 
                s = "> Log\nStorage\nExit"; 
                break; 
            case 1: 
                s = "Log\n> Storage\nExit"; 
                break; 
            case 2: 
                s = "Log\nStorage\n> Exit"; 
                break;  
            default:  
                break; 
        }
        menuTitle.text = "Command"; 
        menuBody.text = s;
    } 

    private void switchLogText(){
        string s = ""; 
        switch (menuCount){
            case 0: 
                s = "Log out for the day?\nWarning: Not collecting all the junk in the system will result in disciplinary action.\n> No\nYes"; 
                break; 
            case 1: 
                s = "Log out for the day?\nWarning: Not collecting all the junk in the system will result in disciplinary action.\nNo\n> Yes";  
                break;
            default:  
                break; 
        }
        menuTitle.text = "Log"; 
        menuBody.text = s;
    }  

    private void switchStorageText(){
        string s = "Items: " + itemCount + "/" + maxItemCount + "\nCurrently held items in back storage:\n"; 
        for (int i = 0; i < storageList.Count; i ++){
            s += storageList[i] + "\n"; 
        }
        menuTitle.text = "Storage"; 
        menuBody.text = s;
    }  
}
