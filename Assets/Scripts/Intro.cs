using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour
{
    public bool introOn = true; 
    public Camera camera; 
    public GameObject player; 
    public GameObject playerSpr; 
    public GameObject playerStartAnim; 
    public Light lightToFade;
    public float flashingDelay = 0.5f; 
    public int flashingNum = 10; 
    private int flashingCount = 0; 
    public float dark = 3f; 
    public float fadeIn = 2f; // Time to fade from 0 to 500

    public float shakeMagnitude = 0.2f; // The intensity of the shake
    public float shakeDuration = 0.5f; // Duration of the initial shake
    private float shakeTimeRemaining;
    private Vector3 originalCameraPosition;
 
    void Start()
    {
        lightToFade.color = new Color(234f / 255f, 255f / 255f, 219f / 255f);
        lightToFade.intensity = 0f; 
        originalCameraPosition = camera.transform.position;
        shakeTimeRemaining = shakeDuration; 

        if (introOn)
        { 
            playerSpr.SetActive(false);
            player.GetComponent<PlayerMovement>().movementEnabled = false;  
            StartCoroutine(flashLight(lightToFade)); 
        }
        else if (!introOn){ 
            playerSpr.SetActive(true);
            player.GetComponent<PlayerMovement>().movementEnabled = true;  
            playerStartAnim.SetActive(false); 
            lightToFade.intensity = 500f; 
        }
    }

    IEnumerator flashLight(Light light)
    {
        while (flashingCount < flashingNum)
        {
            yield return new WaitForSeconds(flashingDelay);
            
            light.color = new Color(1f, 0.1f, 0f);  // Flash red
            light.intensity = 500f;

            // Start shaking during the flashing phase
            shakeTimeRemaining = shakeDuration;

            yield return new WaitForSeconds(flashingDelay);
            
            light.color = new Color(234f / 255f, 255f / 255f, 219f / 255f);  // Back to original color
            light.intensity = 0f; 
            flashingCount++; 
        }

        if (flashingCount >= flashingNum)
        { 
            // Start the fading process after the flashes
            StartCoroutine(darkenLight(light));
        }
    }

    IEnumerator darkenLight(Light light)
    {
        light.color = new Color(234f / 255f, 255f / 255f, 219f / 255f);
        light.intensity = 0f;
        yield return new WaitForSeconds(dark);
        StartCoroutine(FadeLight(lightToFade, fadeIn, 0f, 500f));
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
        if (shakeTimeRemaining > 0 && introOn == true)
        {
            // Shake the camera by adding a random offset to the camera's position
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            camera.transform.position = originalCameraPosition + shakeOffset;

            // Gradually reduce the shake magnitude over time
            shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, Time.deltaTime / shakeDuration);

            // Decrease the shake time remaining
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Once the shake is finished, reset the camera position to its original
            camera.transform.position = originalCameraPosition;
        }
    }
}
