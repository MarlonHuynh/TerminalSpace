using System.Collections.Generic; 
using UnityEngine; 
using TMPro; 

public class LevelLoader : MonoBehaviour
{
    public GameObject camera3d; 
    public GoalManager goalManager;
    public CommandTerminal commandTerminal; 
    public GameObject starPrefab;
    public GameObject planetPrefab;
    public GameObject junkPrefab;
    public GameObject shopkeeperShipPrefab;
    public TextMeshProUGUI titleText; 
    Texture2D starTexture;
    Texture2D planet1Texture;
    Texture2D planet2Texture;
    Texture2D planet3Texture;
    Texture2D planet4Texture;
    Texture2D planet5Texture;
    Texture2D planet6Texture;
    Texture2D ringTexture;
    private string currentStar;
    private string currentPlanet;
    private int loadedLevel; 
    private List<GameObject> allBodies = new List<GameObject>();
    private int currentIncompleteLevel = 1; 
    public string[,] levelCompletions = {
        {"Lone-88", "Inherent"}, // 0
        {"Proxima-16", "Incomplete"}, // 1
        {"Luxe-10", "X"},
        {"Ridge-65", "X"}
    };
    public List<string[]> validLevels = new List<string[]>(); 
    
    // Load initial level 
    void Start()
    {
        loadedLevel = 0;
        loadLevel(loadedLevel);
        calculateValidLevels(); 
    }
    // Method to load a level 
    public void loadLevel(int level)
    {
        destroyAllBodies(); // Destroy all bodies
        camera3d.transform.position = new Vector3(0, camera3d.transform.position.y, -30); // Reset Camera position
        camera3d.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset Camera rotation 

        if (level < 0 || level > levelCompletions.Length)
        {
            Debug.Log("Error: Inavlid level number to load.");
            return;
        }
        if (level == 0)
        {
            // Instantiate Star 
            GameObject star = Instantiate(starPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            star.transform.localScale = new Vector3(300, 300, 300);
            allBodies.Add(star);
            // Instantiate ShopkeeperShip 
            GameObject shop = Instantiate(shopkeeperShipPrefab, new Vector3(-5, 0, -30), Quaternion.identity);
            shop.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            allBodies.Add(shop);
            // Add Goals (none)
            goalManager.setGoals(0, 0, 0);
            titleText.text = levelCompletions[level, 0];
        }
        if (level == 1)
        {
            // Instantiate Star 
            GameObject star = Instantiate(starPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            star.transform.localScale = new Vector3(300, 300, 300);
            allBodies.Add(star);
            // Instantiate Planet(s) 
            GameObject planet = Instantiate(planetPrefab, new Vector3(100, 0, 0), Quaternion.identity);
            planet.transform.localScale = new Vector3(100, 100, 100);
            planet.GetComponent<CircularPath>().setCenterTarget(star, 80);
            allBodies.Add(planet);
            // Instantiate Junk 
            GameObject junk = Instantiate(junkPrefab, new Vector3(100, 0, 0), Quaternion.identity);
            junk.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            junk.GetComponent<CircularPath>().setCenterTarget(planet, 10);
            junk.GetComponent<BodyStatus>().junkName = "Old Satellite";
            junk.GetComponent<BodyStatus>().junkValue = 100f;
            allBodies.Add(junk);
            // Set Goals
            goalManager.setGoals(1, 1, 1);
            titleText.text = levelCompletions[level, 0];
        }
    }
    // Method to recalculate validLevels (Levels have the secondary characteristics of Inherent, Incomplete, or Complete and not X which are not unlocked yet)
    public void calculateValidLevels()
    {
        validLevels.Clear();
        for (int i = 0; i < levelCompletions.GetLength(0); i++)
        {
            if (levelCompletions[i, 1] != "X")
            {
                validLevels.Add(new string[] { levelCompletions[i, 0], levelCompletions[i, 1] });
            }
        }
    }
   // Mark the current incomplete level as complete and unlock the next level
    public void markCompleteAndAddNewLevel()
    {
        // Mark as Complete
        if (levelCompletions[currentIncompleteLevel, 1] == "Incomplete")
        {
            levelCompletions[currentIncompleteLevel, 1] = "Complete";
        }
        // Unlock next level
        int nextLevel = currentIncompleteLevel + 1;
        if (nextLevel < levelCompletions.GetLength(0)) // GetLength(0) = number of rows
        {
            levelCompletions[nextLevel, 1] = "Incomplete";
        }
        calculateValidLevels(); 
    } 
    // Destroys all body gameobjects in a level 
    public void destroyAllBodies()
    {
        foreach (GameObject body in allBodies)
        {
            if (body != null)
                Destroy(body);
        }
        allBodies.Clear(); // Optional: clear the list after deleting
    }  
}
