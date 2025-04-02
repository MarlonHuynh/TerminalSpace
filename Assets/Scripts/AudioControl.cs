using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource component
    public float audioTimeStart = 20f; 
    public float waitUntilFade = 2f; 
    public float fadeDuration = 8.5f;
    private void Start()
    { 
        audioSource = gameObject.GetComponent<AudioSource>(); 

        // Set the AudioClip and start the audio
        audioSource.time = audioTimeStart;
        audioSource.Play();

        // Start the coroutine to skip to 20 seconds and apply fade out at 35 seconds
        StartCoroutine(PlayAndFadeAudio());
    }

    private IEnumerator PlayAndFadeAudio()
    {  
 
        yield return new WaitForSeconds(waitUntilFade);  
 
        float startVolume = audioSource.volume;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is set to 0 at the end of the fade
        audioSource.volume = 0f;

        // Stop the audio after it has faded out
        audioSource.Stop();
    }
}
