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
    public PauseScreen pauseScreen;
    public Image blackScreen;
    private float fadeSpeed = 0.5f; 
    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip titleMusic;
    public AudioClip defaultMusic;
    [Header("Cams")]
    public Camera threeDcamera;
    [Header("Game Over")]
    public GameObject WarningCrash;
    public GameObject BlackScreen;
    public GameObject GameOver;
    public TextMeshProUGUI gameOverText;
    [Header("Vars")]
    public bool firstLoadCutscene = false;
    public int volume = 100; 
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
        pauseScreen.canPause = false;
        musicSource.clip = titleMusic;
        musicSource.time = 0f;
        musicSource.volume = 1f; 
        musicSource.Play();

        titleScreenObj.SetActive(true);
        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(false);
        navigationCanvasObj.SetActive(false);
        astroCanvasObj.SetActive(false);
        commandCanvasObj.SetActive(false);
    }
    public void goShip()
    {
        pauseScreen.canPause = true;
        musicSource.clip = defaultMusic;
        musicSource.time = 0f;
        musicSource.volume = 0.5f; 
        musicSource.Play();

        if (firstLoadCutscene == false)
        {
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
        pauseScreen.canPause = false;

        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(true);
        navigationCanvasObj.SetActive(false);
        astroCanvasObj.SetActive(true);
        commandCanvasObj.SetActive(false);
    }
    public void goTopView()
    {
        pauseScreen.canPause = false;

        titleScreenObj.SetActive(false);
        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(true);
        navigationCanvasObj.SetActive(true);
        astroCanvasObj.SetActive(false);
        commandCanvasObj.SetActive(false);
    }
    public void goStorage()
    {
        pauseScreen.canPause = false;

        titleScreenObj.SetActive(false);
        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(true);
        navigationCanvasObj.SetActive(false);
        astroCanvasObj.SetActive(false);
        commandCanvasObj.SetActive(true);
    }
    public void goFloatingPlayer()
    {
        pauseScreen.canPause = false;

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

    public void pressPlay()
    {
        StartCoroutine(FadeOutAndGoShip()); 
    }
    
    IEnumerator FadeOutAndGoShip()
    {
        Color c = blackScreen.color;
        c.a = 0f; // start transparent
        blackScreen.color = c;

        // Make Opaque
        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeSpeed;
            blackScreen.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(1f); // Wait additional sec after screen is black

        // Go to ship
        goShip();

        // End Transparent
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            blackScreen.color = c;
            yield return null;
        }
        c.a = 0f;  
        blackScreen.color = c;
    }
}
