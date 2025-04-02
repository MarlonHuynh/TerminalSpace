using UnityEngine;
using System.Collections;
using System.Linq;

public class FrameByFrameAnimation : MonoBehaviour
{
    public bool startWakeupAnim = false; 
    public GameObject player; 
    public GameObject playerSpr; 
    public GameObject inputText; 
    public Texture2D[] frames; // Array to hold animation frames (textures)
    private int currentFrame = 0;
    
    public int speed = 1; // Speed of the animation
    public string folderName = "StartAnim"; // Folder name where the textures are located
    public bool repeatAnimation = false; // Whether the animation should repeat or not

    private float timeSinceLastFrame = 0f;
    public Material playerStartMaterial;

    void Start()
    { 
        playerSpr.SetActive(false);
        player.GetComponent<PlayerMovement>().movementEnabled = false;  

        // Load all textures from the specified folder in Resources
        frames = Resources.LoadAll<Texture2D>(folderName); 
        if (frames.Length == 0)
        {
            Debug.LogError($"No textures found in Resources/{folderName}");
            return;
        } 
        // Sort the textures by their name (assuming a naming convention like "frame1", "frame2", etc.)
        frames = frames.OrderBy(frame => ExtractFrameNumber(frame.name)).ToArray();
        
    } 

    public void startWakeUp(){
        startWakeupAnim = true; 
    }
    void Update()
    { 
        if (frames.Length > 0 && startWakeupAnim)
        {
            // Update the animation based on the speed
            timeSinceLastFrame += Time.deltaTime;

            if (timeSinceLastFrame >= 1f / speed)
            {
                // Change the texture of the PlayerStart material
                if (playerStartMaterial != null)
                {
                    playerStartMaterial.SetTexture("_BaseMap", frames[currentFrame]);

                }

                // Advance to the next frame
                currentFrame++;

                // Check if the animation should repeat
                if (currentFrame >= frames.Length)
                {
                    if (repeatAnimation)
                    {
                        currentFrame = 0; // Reset to the first frame if repeating
                    }
                    else
                    {
                        currentFrame = frames.Length - 1; // Stay at the last frame if not repeating
                    }
                }

                timeSinceLastFrame = 0f; // Reset the timer
            }
            // Done
            if ( currentFrame == frames.Length - 1){
                player.GetComponent<PlayerMovement>().movementEnabled = true; 
                playerSpr.SetActive(true); 
                playerStartMaterial.SetTexture("_BaseMap", frames[0]);
                gameObject.SetActive(false);
                inputText.GetComponent<TextFade>().fadeIn();
            }
        }
    }

    // Method to extract the numerical part from the texture name (e.g., "frame1" -> 1)
    int ExtractFrameNumber(string name)
    {
        string numberPart = new string(name.Where(char.IsDigit).ToArray());
        return int.Parse(numberPart); // Convert to an integer for proper sorting
    }
}
