using UnityEngine;
using System.Collections;

public class ShipCutsceneManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameManager gameManager; 
    public GameObject BeepFX;
    public GameObject CrashFX;
    public Camera camera;
    public GameObject player;
    public GameObject playerSpr;
    public GameObject playerStartAnim;
    public Light light;
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
    public float shakeDuration = 0.5f; // Duration of the initial shake
    public float shakeTimeRemaining;
    private Vector3 originalCameraPosition;
    [Header("Eject Vars")]
    private bool isPullingDown = false; 
    private bool ejectShakeOn = false; 
    public float burstForce = 10f; 
  
    public void checkIntro(){
        Debug.Log("Checking intro"); 
        originalCameraPosition = camera.transform.position;
        originalLightColor = light.color; 
        if (introOn)
        {
            BeepFX.SetActive(true);
            BeepFX.GetComponent<AudioControl>().playAlteredAudio(); 
            CrashFX.SetActive(true);
            CrashFX.GetComponent<AudioControl>().playAlteredAudio(); 
            light.intensity = 0f;
            originalCameraPosition = camera.transform.position;
            shakeTimeRemaining = shakeDuration;
            playerSpr.SetActive(false);
            player.GetComponent<PlayerMovement>().movementEnabled = false;
            StartCoroutine(flashLight());
        }
        else if (!introOn)
        { // off 
            playerSpr.SetActive(true);
            player.GetComponent<PlayerMovement>().movementEnabled = true;
            playerStartAnim.SetActive(false);
            light.intensity = 500f;
            BeepFX.SetActive(false);
            CrashFX.SetActive(false);
        }
    } 

    IEnumerator flashLight()
    {
        while (flashingCount < flashingNum)
        {
            yield return new WaitForSeconds(flashingDelay);

            light.color = new Color(1f, 0.1f, 0f);  // Flash red
            light.intensity = 500f; 

            yield return new WaitForSeconds(flashingDelay);
 
            light.intensity = 0f;
            flashingCount++;
        }
        light.color = originalLightColor;

        if (flashingCount >= flashingNum)
        {
            // Start the fading process after the flashes
            StartCoroutine(fadeLightIn(light));
        }
    }

    IEnumerator fadeLightIn(Light light)
    { 
        light.intensity = 0f;
        yield return new WaitForSeconds(darkTime);
        StartCoroutine(FadeLight(this.light, fadeIn, 0f, 500f));
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
        /*
        // Handle the camera shake
        if (introOn == true)
        {
            if (shakeTimeRemaining > 0)
            {
                // Shake the camera by adding a random offset to the camera's position
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                camera.transform.position = originalCameraPosition + shakeOffset;

                // Gradually reduce the shake magnitude over time
                shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, Time.deltaTime / shakeDuration);

                // Decrease the shake time remaining
                shakeTimeRemaining -= Time.deltaTime;
            }
            else if (shakeTimeRemaining <= 0)
            {
                // Once the shake is finished, reset the camera position to its original
                camera.transform.position = originalCameraPosition;
            }
        }
        if (ejectShakeOn)
        {
            // Shake the camera by adding a random offset to the camera's position
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            camera.transform.position = originalCameraPosition + shakeOffset; 
        }

        if (isPullingDown == true)
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.down * burstForce, ForceMode.Impulse);
        } */
    }

    public void ejectCutscene()
    { 
        light.intensity = 0f; 
        flashingCount = 0; 
        shakeTimeRemaining = 10f;
        ejectShakeOn = true; 
        StartCoroutine(flashLightEject(light));
    }

    IEnumerator flashLightEject(Light light)
    {
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
