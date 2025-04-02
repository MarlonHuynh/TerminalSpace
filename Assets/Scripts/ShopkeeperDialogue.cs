using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopkeeperDialogue : MonoBehaviour
{
    public GameObject shopKeepingCanvas; 
    public TextMeshProUGUI ShopkeeperText; 
    public Image ShopkeeperImage;
    
    // Audio variables
    public AudioSource audioSource;
    public AudioClip typeSound;

    // Shopkeeper expressions
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
    private Dictionary<string, Sprite> expressionMap;
    public bool dialogueDone = false; 
    private string[,] pairs = new string[,]{}; 
    private string[,] dialoguePairs = { 
        { "Hey junker.", "shp_normal" },
        { "Haven't seen you in a while.", "shp_normal" },
        { "You're lucky I managed to catch your ship before it ran out of fuel.", "shp_normal" },
        { "If it weren't for me, you'd be dead.", "shp_unamused" },
        { "Don't think I'm going soft on you.", "shp_normal" },
        { "You still owe me a lot of scrap, so go out there and fish for some.", "shp_unamused" },
        { "This time, DON'T forget to document the planetary bodies you see too.", "shp_angry" },
        { "I'll be around. And don't go too far again.", "shp_unamused" },
        { "I really don't need any more cases of my little junkers going missing.", "shp_unamused" }
    }; 
    private string[,] unamusedCheckInPairs = { 
        { "You're back? Did you forget what I said?", "shp_unamused"}, 
        { "Go and collect junk, dingus.", "shp_unamused"}, 
    };
    private string[,] dontBotherPairs = { 
        { "[...I should probably leave him alone before he gets any more angry.]", "player_think"},  
    };
    private string[,] upgradePairs = { 
        { "Hey there.", "stk_smile" },
        { "I'm a friend of your unamused buddy here. Name's Statik. I.T. guy for most of the creatures this quadrant of the universe.", "stk_smile" },
        { "Usually, I don't get to meet other Mamari. I.F. rules and all, but let's just say, my totally rad skills merit special consideration. ", "stk_smile" },
        { "And I missed your old buddy here. ", "stk_smile" },
        { "Hurry up already before I throw your ass back to space.", "shp_normal" },
        { "Ah... true bromance! How long have we known each other? And yet in all that time you never called me... I'm starting to think that you don't miss me. ", "stk_smile" },
        { "We could have bonded over our shared trauma of women or something.", "stk_smile" },
        { "Hmm... Perhaps you were too busy fraternizing with that human pilot of yours?", "stk_smile" },
        { "What's his name again? Or was it a her? Or they? Speaking of which, where are they? You didn't eat them did you? ", "stk_smile" },
        { "I'm gonna kill you. ", "shp_sulking" },
        { "That is unadvisable. I am a very valuable asset to the I.F." , "stk_smile" },
        { "Well, no matter. I'm almost done with the updates to your communications. ", "stk_smile" },
        { "Thank jods. I don't think I can stand another second of your incessant babbling.", "shp_unamused" },
        { "Strange, usually networking bugs aren't this elaborate. Whatever, it's probably just some troll with too much time. ", "stk_smile" },
        { "I also did a full-slate clean on your ship and minor upgrades so you should be able to sell more kinds of goods.", "stk_smile" },
        { "Yup, that's pretty much it...", "stk_smile" },
        { "Alright, I'll be leaving now.", "stk_smile" },
        { "Bye babe!!", "stk_smile" },
        { ".....", "shp_unamused" },
        { "Heheheh... Also...", "stk_smile" },
        { "Next time, maybe keep the '''cute pics :)''' folder of you and your pilot friend off public access.", "stk_smile" },
    }; 

    private string[,] cerealPairs = { 
        { "You're back? You know, I've been thinking. ", "shp_normal" }, 
        { "Just wait one second. Let me speak to your soul. ", "shp_normal" }, 
        { "There's two types of cereal in this world. ", "shp_normal" }, 
        { "Your fun cereal, and your casual cereal. None of that bullshit-ass cheerios with no sugar. ", "shp_normal" }, 
        { "What are those crazy shits? Raisan bran? Don't speak to me. ", "shp_normal" }, 
        { "Don't talk to me about some Raisan bran. ", "shp_normal" }, 
        { "Wheaties? Stop it. You pick that up in the aisle I'm... ", "shp_normal" }, 
        { "liable to punch you in the face.", "shp_normal" }, 
        { "At the beginning, the crunch is one of a kind. ", "shp_smile" }, 
        { "I eat that one crunch, just to get the feel of the shit, and I just", "shp_normal" }, 
        { "Just let that shit sit. Come back after a minute, 60/40 rule. ", "shp_normal" }, 
        { "I like 60% of my shit soggy, and the other 40% crunch. ", "shp_normal" }, 
        { "Ya feel?", "shp_smile" }, 
        { "[It appears that shopkeeper is not in his right mind. Perhaps those edibles were, indeed, shit.]", "player_think" }, 
    };

    private int count = 0;
    private bool isTyping = false;
    private bool nextLoadExist = true;
    public float baseDelay = 1.5f;
    public float charDelayFactor = 0.05f;

    void Start()
    {
        // Initialize dictionary mapping string names to sprites
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
        if (audioSource != null)
        {
            audioSource.loop = true; // Enable looping
            audioSource.volume = 0.4f; 
        }
        // Set initial pairs
        pairs = dialoguePairs;  
    }

    public void startText()
    {  
        if (dialogueDone) return;    
        StartCoroutine(DelayBeforeStartText());
    }

    IEnumerator DelayBeforeStartText()
    { 
        yield return new WaitForSeconds(0.1f); // Small delay before starting
        StartCoroutine(AutoLoadText());  // Start the actual text loading
    }

    IEnumerator AutoLoadText()
    {
        // Display all of the text 
        while (nextLoadExist)
        { 

            string text = pairs[count, 0];
            string expressionKey = pairs[count, 1];

            if (!shopKeepingCanvas.activeSelf){
                shopKeepingCanvas.SetActive(true);  
            }

            // Type text and play sound
            yield return StartCoroutine(TypeText(text, expressionKey)); 
            yield return new WaitForSeconds(baseDelay); 
            count++; 
            if (count >= pairs.GetLength(0))
            {
                nextLoadExist = false;
                break;
            }
        } 
        if (!nextLoadExist){ 
            // Checks if there's another conversation
            if (pairs == dialoguePairs)
            {
                pairs = unamusedCheckInPairs; 
            }
            else if (pairs == unamusedCheckInPairs)
            {
                pairs = dontBotherPairs; 
            }
            else
            {
                pairs = dontBotherPairs; 
            }

            // End text
            nextLoadExist = true;
            count = 0;   

            ShopkeeperText.text = "";
            ShopkeeperImage.sprite = player_think;
            
            shopKeepingCanvas.SetActive(false); 
            GameObject.Find("--MainPlayer--").GetComponent<PlayerMovement>().movementEnabled = true;
        }  
    }

    IEnumerator TypeText(string sentence, string expressionKey)
    {
        isTyping = true; 

        // Start looping audio when typing begins
        if (audioSource != null && typeSound != null)
        {
            audioSource.clip = typeSound;
            if (expressionKey.Substring(0, 3) == "stk"){
                audioSource.pitch = 0.75f; 
            }
            else if (expressionKey.Substring(0, 3) == "pla"){
                audioSource.pitch = 0f; 
            }
            else{
                audioSource.pitch = 1f; 
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.05f); // Give time to play
        }

        // Change expression if it exists in dictionary 
        if (expressionMap.TryGetValue(expressionKey, out Sprite newSprite))
        {
            ShopkeeperImage.sprite = newSprite;
        } 

        ShopkeeperText.text = "";
        foreach (char letter in sentence)
        {
            ShopkeeperText.text += letter;
            yield return new WaitForSeconds(charDelayFactor);
        } 
        isTyping = false;

        // Stop audio when typing finishes
        if (audioSource != null && audioSource.isPlaying)
        {
            StartCoroutine(FadeOutAudio(0.2f)); 
        }
    }

    IEnumerator FadeOutAudio(float fadeDuration) {
        float startVolume = audioSource.volume; 
        // Gradually decrease the volume
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        } 
        // Ensure the volume is set to 0 after fading out
        audioSource.volume = 0; 
        // Optionally stop the audio source after fading out
        audioSource.Stop();
        audioSource.volume = 1f; 
    }

}
