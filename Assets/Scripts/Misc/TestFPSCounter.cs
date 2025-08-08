using UnityEngine;
using TMPro;
using System.Collections;

public class TestFPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float delay = 1f; 
    private bool wait = false; 
    void Update()
    {
        if (!wait)
        {   
            wait = true; 
            StartCoroutine(waitFPS(delay));
        } 
    }

    IEnumerator waitFPS(float s)
    {
        yield return new WaitForSeconds(s);
        float fps = 1f / Time.unscaledDeltaTime;
        fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
        wait = false; 
    }
}
