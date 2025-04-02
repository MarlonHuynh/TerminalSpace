using UnityEngine;
using TMPro;
using System.Collections;

public class TextFade : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; 
    private bool startFadeIn = false; 
    public float fadeInDuration = 5f;  // Fade-in duration in seconds
    public float still = 10f; 
    private bool stillDone = false; 
    public float fadeOutDuration = 5f; // Fade-out duration in seconds

    private float currentTime = 0f;
    private bool isFadingIn = true;
    private bool isFadingOut = false;

    void Start()
    {
        // Get the TextMeshPro component
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Set the initial opacity to 0 (fully transparent)
        Color color = textMeshPro.color;
        color.a = 0f;
        textMeshPro.color = color;
    } 

    IEnumerator waitStill(float delay) {
        yield return new WaitForSeconds(delay); 
        stillDone = true; 
    } 

    public void fadeIn(){
        startFadeIn = true; 
    }

    void Update()
    { 

        // If fading in
        if (isFadingIn && startFadeIn)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(currentTime / fadeInDuration);  // Fade in over the specified duration
            Color color = textMeshPro.color;
            color.a = alpha;
            textMeshPro.color = color;

            // If fade-in is complete, start fading out
            if (currentTime >= fadeInDuration)
            {
                isFadingIn = false;
                isFadingOut = true;
                currentTime = 0f;  // Reset timer for fade-out
            }
        }

        StartCoroutine(waitStill(still)); 

        // If fading out
        if (isFadingOut && stillDone)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - currentTime / fadeOutDuration);  // Fade out over the specified duration
            Color color = textMeshPro.color;
            color.a = alpha;
            textMeshPro.color = color;

            // If fade-out is complete, stop the process
            if (currentTime >= fadeOutDuration)
            {
                isFadingOut = false;
            }
        }
    }
}
