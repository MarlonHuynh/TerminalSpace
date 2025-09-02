using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class ShopkeeperDialogue : MonoBehaviour
{
    [Header("Shopkeeper UI")] 
    public PlayerMovement playerMovement;   
    public GameObject shopKeepingCanvas; 
    public TextMeshProUGUI ShopkeeperText; 
    public GameObject lineRenderer; 
    public GameObject headPivot; 
    public UnityEngine.UI.Image ShopkeeperImage;
    public Transform panelTransform; 
    public GameObject ShopkeeperDialogueButtonPrefab; 
    [Header("Shopkeeper Audio")]
    public AudioSource talkingSource;
    public AudioClip typeSound;
    [Header("Shopkeeper Sprites")]
    public Sprite shp_normal;
    public Sprite shp_smile;
    public Sprite shp_realsmile;
    public Sprite shp_unamused;
    public Sprite shp_angry;
    public Sprite shp_side;
    public Sprite shp_closeup;
    public Sprite shp_sulking;
    public Sprite stk_smile;
    public Sprite player_think; 
    public bool dialogueDone = false;   

    private int count = 0;
    private bool isTyping = false;
    private bool nextLoadExist = true;
    public float baseDelay = 1.5f;
    public float charDelayFactor = 0.05f; 
    public bool firstTalkedYet = false; 

    private bool sentenceFullyDisplayedBySpace = false;
    [Header("Dialogue Data")]  
    public DialogueData currentDialogue; 
    private Dictionary<string, Sprite> expressionMap;
    public float shopkeeperVolume; 

    void Start()
    {
        expressionMap = new Dictionary<string, Sprite>()
        {
            { "shp_normal", shp_normal },
            { "shp_smile", shp_smile },
            { "shp_realsmile", shp_realsmile },
            { "shp_unamused", shp_unamused },
            { "shp_angry", shp_angry },
            { "shp_side", shp_side },
            { "shp_closeup", shp_closeup },
            { "shp_sulking", shp_sulking },
            { "stk_smile", stk_smile },
            { "player_think", player_think }
        };

        // Ensure the audio source is set to loop
        if (talkingSource != null)
        {
            talkingSource.loop = true; // Enable looping
            talkingSource.volume = shopkeeperVolume;
        }
    }

    public void startText()
    {
        if (firstTalkedYet == false)
        {
            firstTalkedYet = true; 
        }
        
        if (dialogueDone) return;
        playerMovement.movementEnabled = false;
        StartCoroutine(DelayBeforeStartText());
    }

    IEnumerator DelayBeforeStartText()
    {
        yield return new WaitForSeconds(0.1f); // Small delay before starting
        StartCoroutine(AutoLoadText());  // Start the actual text loading
    }

     IEnumerator AutoLoadText()
    {
        // Clear old buttons first
        foreach (Transform child in panelTransform)
        {
            Destroy(child.gameObject);
        }
        nextLoadExist = true;
        count = 0;

        // Complete all the dialogue
        while (nextLoadExist)
        {
            DialogueLine line = currentDialogue.lines[count];
            if (!shopKeepingCanvas.activeSelf)
            {
                shopKeepingCanvas.SetActive(true);
            }

            yield return StartCoroutine(TypeText(line.text, line.expressionKey));
            yield return new WaitForSeconds(baseDelay);
            count++;

            if (count >= currentDialogue.lines.Count)
            {
                nextLoadExist = false;
                break;
            }
        }
        Debug.Log("Completed Dialogue"); 
        // At the end, checks if the dialogue has a button. 
        if (!nextLoadExist && currentDialogue.hasButtons && currentDialogue.buttons != null)
        {
            foreach (ButtonLine buttonLine in currentDialogue.buttons)
            {
                GameObject newButton = Instantiate(ShopkeeperDialogueButtonPrefab, panelTransform);
                TextMeshProUGUI btnText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = buttonLine.buttonText;
                }

                Button btnComponent = newButton.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.onClick.RemoveAllListeners();
                    btnComponent.onClick.AddListener(() =>
                    {
                        StartNextDialogue(buttonLine.nextDialogue);
                    });
                }
            }
        }
        else
        {
            dialogueDone = true;
            shopKeepingCanvas.SetActive(false); 
            playerMovement.movementEnabled = true;
        }
    }

    void StartNextDialogue(DialogueData nextDialogue)
    {
        currentDialogue = nextDialogue;
        dialogueDone = false;
        StartCoroutine(AutoLoadText());
    } 

    IEnumerator TypeText(string sentence, string expressionKey)
    {
        isTyping = true; 

        // PLAY SOUND
        if (talkingSource != null && typeSound != null)
        {
            talkingSource.clip = typeSound;
            if (expressionKey.StartsWith("stk"))
            { // Static
                talkingSource.pitch = 0.75f;
                talkingSource.volume = shopkeeperVolume;
            }
            else if (expressionKey.StartsWith("pla")) // Player
                talkingSource.pitch = 0f;
            else 
            { // Shopkeeper
                talkingSource.pitch = 1f;
                talkingSource.volume = shopkeeperVolume;
            }


            talkingSource.Play();
            yield return new WaitForSeconds(0.05f);
        }

        // CHANGE IMAGE
        if (expressionMap.TryGetValue(expressionKey, out Sprite newSprite))
        {
            ShopkeeperImage.sprite = newSprite;
        } 
        // TYPE TEXT
        ShopkeeperText.text = "";
        foreach (char letter in sentence)
        {
            if (ShopkeeperText.text == sentence)
                break; 

            ShopkeeperText.text += letter;
            yield return new WaitForSeconds(charDelayFactor);
        } 
        isTyping = false;

        // FADE OUT AUDIO
        if (talkingSource != null && talkingSource.isPlaying)
        {
            StartCoroutine(FadeOutAudio(0.2f));
        }
    }

    IEnumerator FadeOutAudio(float fadeDuration) {
        float startVolume = talkingSource.volume; 
        while (talkingSource.volume > 0)
        {
            talkingSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        } 
        talkingSource.volume = 0; 
        talkingSource.Stop(); 
    } 
}