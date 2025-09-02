using UnityEngine;
using System.Collections;

public class ShipCutsceneManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameManager gameManager; 
    public GameObject BeepFX;
    public GameObject CrashFX;
    public Camera mainCamera; 
    public GameObject player;
    public GameObject playerSpr;
    public GameObject playerStartAnim;
    public Light spotLight;
    public Light flashingLight;
    public GameObject floorPivotR;
    public GameObject floorPivotL;
    private Color originalLightColor; 
    [Header("Intro Vars")]
    public bool introOn = false;
    public float flashingDelay = 0.5f;
    public int flashingNum = 10;
    private int flashingCount = 0;
    public float darkTime = 3f;
    public float fadeIn = 2f; // Time to fade from 0 to 500 
    public float shakeMagnitude = 0.2f; // The intensity of the shake
    public float shakeDuration = 5f; // Duration of the initial shake
    private float shakeTimeRemaining;
    public Vector3 originalCameraPosition;
    [Header("Eject Vars")]
    private bool isPullingDown = false; 
    private bool ejectShakeOn = false; 
    private float originalSpotLightIntensity; 
    public float burstForce = 10f;

    void Start()
    {
        originalCameraPosition = mainCamera.transform.position;
        originalSpotLightIntensity = spotLight.intensity;
        originalLightColor = flashingLight.color;
        spotLight.intensity = 0f;
    }
    public void checkIntro()
    {
        if (introOn)
        {
            BeepFX.SetActive(true);
            BeepFX.GetComponent<AudioControl>().playAlteredAudio();
            CrashFX.SetActive(true);
            CrashFX.GetComponent<AudioControl>().playAlteredAudio(); 
            shakeTimeRemaining = shakeDuration;
            playerSpr.SetActive(false);
            player.GetComponent<PlayerMovement>().movementEnabled = false;
            ejectShakeOn = true; 
            StartCoroutine(flashLight());  
        }
        else if (!introOn)
        { // off 
            playerSpr.SetActive(true);
            player.GetComponent<PlayerMovement>().movementEnabled = true;
            playerStartAnim.SetActive(false); 
            BeepFX.SetActive(false);
            CrashFX.SetActive(false);
        }
    } 

    IEnumerator flashLight()
    {
        while (flashingCount < flashingNum)
        {
            yield return new WaitForSeconds(flashingDelay);

            flashingLight.color = new Color(1f, 0.1f, 0f);  // Flash red
            flashingLight.intensity = 500f; 

            yield return new WaitForSeconds(flashingDelay);
 
            flashingLight.intensity = 75f;
            flashingCount++;
        }
        flashingLight.color = originalLightColor;
        flashingLight.intensity = 0f;
 
        playerStartAnim.GetComponent<FrameByFrameAnimation>().startWakeUp();
        yield return new WaitForSeconds(8f);

        if (flashingCount >= flashingNum)
        {
            spotLight.intensity = originalSpotLightIntensity;
            yield return new WaitForSeconds(0.1f);
            spotLight.intensity = 0f;
            yield return new WaitForSeconds(0.1f);
            spotLight.intensity = originalSpotLightIntensity;
            yield return new WaitForSeconds(0.1f);
            spotLight.intensity = 0f;
            yield return new WaitForSeconds(0.1f);
            spotLight.intensity = originalSpotLightIntensity;
        }
    } 

    // Coroutine to fade light intensity
    IEnumerator FadeLight(Light light, float duration, float startIntensity, float endIntensity)
    {
        float timeElapsed = 0f; 
        // Set the initial intensity
        light.intensity = startIntensity; 
        while (timeElapsed < duration)
        {
            // Increment the elapsed time
            timeElapsed += Time.deltaTime; 
            // Gradually change the intensity based on elapsed time
            light.intensity = Mathf.Lerp(startIntensity, endIntensity, timeElapsed / duration);

            // Wait until the next frame
            yield return null;
        } 
        // Ensure the final intensity is set
        light.intensity = endIntensity; 
        // Start Wakeup Animation
        playerStartAnim.GetComponent<FrameByFrameAnimation>().startWakeUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the camera shake
        if (introOn == true && ejectShakeOn == true && shakeTimeRemaining > 0)
        {
            if (shakeTimeRemaining > 0)
            {
                Debug.Log("Shaking! " + shakeTimeRemaining); 
                // Shake the camera by adding a random offset to the camera's position
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                mainCamera.transform.position = originalCameraPosition + shakeOffset; 

                // Decrease the shake time remaining
                shakeTimeRemaining -= Time.deltaTime;
            }
            else if (shakeTimeRemaining <= 0)
            {
                // Once the shake is finished, reset the camera position to its original
                mainCamera.transform.position = originalCameraPosition;
            }
        } 

        if (isPullingDown == true)
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.down * burstForce, ForceMode.Impulse);
        }  
    }

    public void ejectCutscene()
    { 
        flashingLight.intensity = 0f; 
        flashingCount = 0; 
        shakeTimeRemaining = 10f;
        ejectShakeOn = true; 
        StartCoroutine(flashLightEject(flashingLight));
    }

    IEnumerator flashLightEject(Light light)
    {
        
        ejectShakeOn = true; 
        while (flashingCount < 10) // Flash 5 times then eject
        {
            yield return new WaitForSeconds(flashingDelay);

            light.color = new Color(1f, 0.1f, 0f);  // Flash red
            light.intensity = 500f;

            yield return new WaitForSeconds(flashingDelay);

            light.intensity = 0f;
            flashingCount++;

            if (flashingCount == 3)
            {
                floorPivotL.GetComponent<FloorPanel>().open();
                floorPivotR.GetComponent<FloorPanel>().open();
                isPullingDown = true;
            }

            if (flashingCount == 4)
            {
                gameManager.goFloatingPlayer();
            }
        } 
    } 
}
