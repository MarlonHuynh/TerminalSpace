using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject spaceshipObj;
    public GameObject terminalsObj;
    public GameObject titleScreenObj; 
    public GameObject navigationCanvasObj;
    public GameObject astroCanvasObj;
    public GameObject commandCanvasObj;
    public GameObject floatingSceneObj;
    public FloatingThroughSpace floatingThroughSpace; 
    public FuelTankMovement fuelTankMovement;
    public CameraMovement cameraMovement;
    public ShipCutsceneManager shipCutsceneMananger;
    [Header("Cams")]
    public Camera threeDcamera;
    [Header("Game Over")]
    public GameObject WarningCrash;
    public GameObject BlackScreen;
    public GameObject GameOver;
    public TextMeshProUGUI gameOverText; 
    [Header("Vars")]
    public bool firstLoadCutscene = false; 
    // Start is called before the first frame update
    void Start()
    { //
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0); // Example: Sorting along the Y-axis 

        threeDcamera.Render();
        terminalsObj.SetActive(false);
        floatingSceneObj.SetActive(false);

        goTitleScreen(); 
    }

    public void goTitleScreen()
    {
        titleScreenObj.SetActive(true); 
        spaceshipObj.SetActive(false); 
        terminalsObj.SetActive(false);
            navigationCanvasObj.SetActive(false);
            astroCanvasObj.SetActive(false);
            commandCanvasObj.SetActive(false);  
    }
    public void goShip()
    {
        if (firstLoadCutscene == false){
            firstLoadCutscene = true; 
            shipCutsceneMananger.checkIntro(); 
        }
        titleScreenObj.SetActive(false); 
        spaceshipObj.SetActive(true); 
        terminalsObj.SetActive(false);
            navigationCanvasObj.SetActive(false);
            astroCanvasObj.SetActive(false);
            commandCanvasObj.SetActive(false); 
            fuelTankMovement.setFuel(cameraMovement.fuel);
    } 
    public void goAstro()
    {
        spaceshipObj.SetActive(false); 
        terminalsObj.SetActive(true);
            navigationCanvasObj.SetActive(false);
            astroCanvasObj.SetActive(true);
            commandCanvasObj.SetActive(false);
    } 
    public void goTopView()
    {
        titleScreenObj.SetActive(false); 
        spaceshipObj.SetActive(false); 
        terminalsObj.SetActive(true);
            navigationCanvasObj.SetActive(true); 
            astroCanvasObj.SetActive(false);
            commandCanvasObj.SetActive(false);
    } 
    public void goStorage()
    {
        titleScreenObj.SetActive(false); 
        spaceshipObj.SetActive(false); 
        terminalsObj.SetActive(true);
            navigationCanvasObj.SetActive(false);
            astroCanvasObj.SetActive(false);
            commandCanvasObj.SetActive(true);
    }
    public void goFloatingPlayer()
    {
        titleScreenObj.SetActive(false); 
        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(false);
            navigationCanvasObj.SetActive(false);
            astroCanvasObj.SetActive(false);
            commandCanvasObj.SetActive(false);
        floatingSceneObj.SetActive(true);
        floatingThroughSpace.startFloat(); 
        
    }
    public void triggerCrashCutscene()
    {
        if (GameObject.Find("WarningClose") != null)
        {
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
    public void triggerEjectCutscene()
    {
        goShip();
        shipCutsceneMananger.ejectCutscene(); 
    }
}
