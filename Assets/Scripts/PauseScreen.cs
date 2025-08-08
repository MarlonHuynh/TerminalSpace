using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class PauseScreen : MonoBehaviour
{
    public bool canPause = false;
    private bool isPaused = false;
    public GameManager gameManager;
    public GameObject pauseUI;
    public TextMeshProUGUI pauseText;
    public AudioSource musicSource;
    public AudioClip pauseMusic;
    public AudioMixer masterMixer; 
    private string[] menuItems = { "Resume", "Options", "Exit" };
    private string[] optionItems = { "Volume: ", "Back" }; 
    [Header("Debug")] 
    public int screenID = 0; // 0 - Main, 1 - Options 
    public int mainCount = 0;
    public int optionsCount = 0; 

    void Update()
    {
        // Toggle pause when pressing Escape
        if (Input.GetKeyUp(KeyCode.Escape) && canPause)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (isPaused)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                if (screenID == 0)
                    NavUp();
                if (screenID == 1)
                    OpUp();
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                if (screenID == 0)
                    NavDown();
                if (screenID == 1)
                    OpDown();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (screenID == 0) // Main pause screen
                {
                    switch (mainCount)
                    {
                        case 0: // Resume
                            ResumeGame();
                            break;
                        case 1: // Options
                            displayOptionsText();
                            screenID = 1;
                            break;
                        case 2: // TitleScreen
                            gameManager.goTitleScreen();
                            ResumeGame();
                            break;
                    }
                }
                else if (screenID == 1 && optionsCount == 1)
                {
                    displayPauseText();
                    screenID = 0; 
                }
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (screenID == 1) // Options Pause screen
                {
                    switch (optionsCount)
                    {
                        case 0: // Volume
                            if (gameManager.volume < 100)
                                gameManager.volume++;
                            SetMasterVolume(gameManager.volume / 100f); 
                            displayOptionsText(); 
                            break; 
                    }
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (screenID == 1) // Options Pause screen
                {
                    switch (optionsCount)
                    {
                        case 0: // Volume
                            if (gameManager.volume > 0)
                                gameManager.volume--;
                            SetMasterVolume(gameManager.volume / 100f); 
                            displayOptionsText();
                            break; 
                    }
                }
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;   // Stop time
        isPaused = true;
        pauseUI.SetActive(true);
        pauseText.text = "> Resume\nOptions\nExit";
        displayPauseText();
        screenID = 0; 
        musicSource.clip = pauseMusic;
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;   // Resume time
        isPaused = false;
        pauseUI.SetActive(false);
    }

    void NavUp()
    {
        if (mainCount > 0)
        {
            mainCount--;
        }
        displayPauseText();
    }

    void NavDown()
    {
        if (mainCount < menuItems.Length - 1)
        {
            mainCount++;
        }
        displayPauseText();
    }

    void OpUp()
    {
        if (optionsCount > 0)
        {
            optionsCount--;
        }
        displayOptionsText();
    }

    void OpDown()
    {
        if (optionsCount < optionItems.Length - 1)
        {
            optionsCount++;
        }
        displayOptionsText();
    }


    // 0
    // 1
    // 2
    void displayPauseText()
    {
        string temp = "";
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == mainCount)
            {
                temp += "> ";
            }
            temp += menuItems[i] + "\n";
        }
        pauseText.text = temp;
    }

    void displayOptionsText()
    { 
        string temp = "";
        for (int i = 0; i < optionItems.Length; i++)
        {
            if (i == optionsCount)
            {
                temp += "> ";
            }
            temp += optionItems[i];
            if (i == 0)
            {
                temp += gameManager.volume + "/100";
            }
            temp += "\n";
        }
        pauseText.text = temp;
    }
    
    public void SetMasterVolume(float volume) // Vol from 0f to 1f 
    {
        // volume is expected in linear 0.0 to 1.0 range
        // Convert to decibel (logarithmic) scale:
        float dB;
        if (volume > 0)
            dB = Mathf.Log10(volume) * 20f;
        else
            dB = -80f; // silent

        masterMixer.SetFloat("MasterVolume", dB);
    }
}
