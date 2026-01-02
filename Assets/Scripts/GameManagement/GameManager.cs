using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject ShipLowResRenderCanvas; 
    public GameObject spaceshipObj;
    public GameObject terminalsObj;
    public GameObject titleScreenObj;
    public GameObject navigationCanvasObj;
    public GameObject astroCanvasObj;
    public GameObject commandCanvasObj;
    public GameObject floatingSceneObj;
    public MiscSoundSFX miscSoundSFX; 
    public FloatingThroughSpace floatingThroughSpace;
    public FuelTankMovement fuelTankMovement;
    public CameraMovement cameraMovement;
    public ShipCutsceneManager shipCutsceneMananger;
    public PauseScreen pauseScreen;
    public Image blackScreen;
    private float fadeSpeed = 0.5f; 
    [Header("Music")]
    public AudioSource musicSource; 
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            // Create a pointer event at the mouse position
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            // Raycast to all UI elements under the mouse
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    Debug.Log("Clicked on: " + result.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Clicked on nothing");
            }
        }
    }

    public void goTitleScreen()
    {
        ShipLowResRenderCanvas.SetActive(true); 
        pauseScreen.canPause = false;
        miscSoundSFX.playTitleMusic();

        titleScreenObj.SetActive(true);
        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(false);
        navigationCanvasObj.SetActive(false);
        astroCanvasObj.SetActive(false);
        commandCanvasObj.SetActive(false);
    }
    public void goShip()
    {
        ShipLowResRenderCanvas.SetActive(true); 
        pauseScreen.canPause = true;
        miscSoundSFX.playShipMusic();
        miscSoundSFX.playAmbientMusic(); 
        
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
        ShipLowResRenderCanvas.SetActive(false); 
        pauseScreen.canPause = false;

        spaceshipObj.SetActive(false);
        terminalsObj.SetActive(true);
        navigationCanvasObj.SetActive(false);
        astroCanvasObj.SetActive(true);
        commandCanvasObj.SetActive(false);
    }
    public void goTopView()
    {
        ShipLowResRenderCanvas.SetActive(false); 
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
        ShipLowResRenderCanvas.SetActive(false); 
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
        ShipLowResRenderCanvas.SetActive(false); 
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
        float startVolume = musicSource.volume;
        float t = 0f;

        // Make Opaque while fading music
        while (c.a < 1f)
        {
            t += Time.deltaTime * fadeSpeed;

            // Fade screen
            c.a = Mathf.Clamp01(t);
            blackScreen.color = c;

            // Fade music
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        } 

        yield return new WaitForSeconds(1f); // Extra silence pause

        // Go to ship
        goShip();

        // Make blackscreen disappear. 
        c = blackScreen.color; 
        c.a = 0f;  
        blackScreen.color = c;
    }
}
