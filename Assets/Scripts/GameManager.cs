using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.Rendering; 

public class GameManager : MonoBehaviour
{   
    public GameObject spaceship;
    public GameObject terminals; 
    public GameObject TopViewCanvas; 
    public GameObject AstroCanvas;  
    public GameObject StorageCanvas; 
    // 
    public Camera threeDcamera; 
    public GameObject WarningCrash;  
    public GameObject BlackScreen; 
    public GameObject GameOver; 
    public TextMeshProUGUI gameOverText;  
    private bool is2d = true;     
    // Start is called before the first frame update
    void Start()
    { //
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0); // Example: Sorting along the Y-axis 

        threeDcamera.Render();
        terminals.SetActive(false); 
    }   
    public void goShip(){
        spaceship.SetActive(true); 

        terminals.SetActive(false); 
        TopViewCanvas.SetActive(false); 
        AstroCanvas.SetActive(false);  
        StorageCanvas.SetActive(false); 
    }

    public void goAstro(){
        spaceship.SetActive(false); 

        terminals.SetActive(true); 
        TopViewCanvas.SetActive(false); 
        AstroCanvas.SetActive(true);
        StorageCanvas.SetActive(false); 
    }

    public void goTopView(){
        spaceship.SetActive(false); 

        terminals.SetActive(true); 
        TopViewCanvas.SetActive(true); 
        AstroCanvas.SetActive(false);  
        StorageCanvas.SetActive(false); 
    }

    public void goStorage(){
        spaceship.SetActive(false); 

        terminals.SetActive(true); 
        TopViewCanvas.SetActive(false); 
        AstroCanvas.SetActive(false);  
        StorageCanvas.SetActive(true); 
    }
    public void triggerCrashCutscene(){
        if (GameObject.Find("WarningClose") != null){ 
            GameObject.Find("WarningClose").SetActive(false);
        } 
        WarningCrash.SetActive(true);
        GameObject.Find("3dCamera").GetComponent<CameraMovement>().movementEnabled = false; 
        StartCoroutine(WaitBlack(3)); 
    } 
    IEnumerator WaitBlack(float sec)
    { 
        yield return new WaitForSeconds(sec); // Wait for 3 seconds 
        WarningCrash.SetActive(false);
        BlackScreen.SetActive(true); 
        StartCoroutine(trueWait(2)); 
    } 
    IEnumerator trueWait(float sec)
    { 
        yield return new WaitForSeconds(sec);
        GameOver.SetActive(true);  
        StartCoroutine(fadeGameOverIn()); 
    }  
    IEnumerator fadeGameOverIn()
    {   
        float elapsedTime = 0f; 
        // Get the current color and ensure the alpha is 0
        Color textColor = gameOverText.color;
        textColor.a = 0f;
        gameOverText.color = textColor; 
        while (elapsedTime < 2f) // 2 is fade duration
        {
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Clamp01(elapsedTime / 2f);
            gameOverText.color = textColor;
            yield return null; // Wait for the next frame
        } 
        textColor.a = 1f; // Ensure the final alpha is 1
        gameOverText.color = textColor;
    }
}
