using System.Collections.Generic; 
using UnityEngine; 
using TMPro;
using System;

public class LevelLoader : MonoBehaviour
{
    [Header("Assignables")]
    public GoalManager goalManager;
    public InteractionPlayer playerInteraction; 
    public ShopkeeperDialogue shopkeeperDialogue; 
    public GameObject camera3d; 
    public TerminalCommand commandTerminal;
    public GameObject starPrefab;
    public GameObject blackholePrefab; 
    public GameObject planetPrefab;
    public GameObject junkPrefab;
    public GameObject shopkeeperShipPrefab;
    public TextMeshProUGUI titleText;
    [Header("Debug")]
    public int loadedLevel;
    public List<GameObject> allBodies = new List<GameObject>();
    public List<GameObject> uncompletedBodies = new List<GameObject>();
    public int currentLevel; // Keep track of current level 
    // PRIVATE
    private int currentIncompleteLevel = 1;
    private const int LONE88_ID0 = 0;
    private const int PROXIMA16_ID1 = 1;
    private const int LUXE10_ID2 = 2; 
    private const int RIDGE65_ID3 = 3;
    private const int MAGNETIC73_ID4 = 4;
    public List<DialogueData> SystemDialogues;
    
    /*
    
    Statuses
    "Inherent" - Unlocked immediately after starting game 
    "Incomplete" - Unlocked but incomplete junk. Player needs to complete to unlock next level 
    "OldComplete" - Marked after the player successfully complete objective and logged it IN THE PAST
    "NewComplete" - Marked after the player successfully complete objective and needs to log it NOW 
    "X" - Not unlocked yet 

    */
    public string[,] levelCompletions = {
        {"Lone-88", "Inherent"}, // 0
        {"Proxima-16", "Incomplete"}, // 1
        {"Luxe-10", "X"}, // 2
        {"Ridge-65", "X"}, // 3
        {"Magnetic-73", "X"} // 4
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
        currentLevel = level; 
        destroyAllBodies(); // Destroy all bodies 
        camera3d.transform.position = new Vector3(0, camera3d.transform.position.y, -30); // Reset Camera position
        camera3d.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset Camera rotation 

        if (level < 0 || level > levelCompletions.Length)
        {
            Debug.Log("Error: Invalid level number to load.");
            return;
        }
        else if (level == LONE88_ID0)
        {
            // Instantiate Star 
            GameObject star = createStar(starPrefab, 500, Color.white);
            // Instantiate ShopkeeperShip 
            GameObject shop = Instantiate(shopkeeperShipPrefab, new Vector3(-5, 0, -30), Quaternion.identity);
            shop.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            allBodies.Add(shop);
            shop.GetComponent<FaceCamera>().setCamera(camera3d.GetComponent<Camera>());
            goalManager.resetGoals();
            // Assign INITIAL Dialogue
            shopkeeperDialogue.currentDialogue = SystemDialogues[level];
            // State in range is true (isTrigger hasn't triggered yet when loaded)
            playerInteraction.shipNearShop = true; 
        }
        else if (level == PROXIMA16_ID1) // Proxima-16
        {
            // Instantiate Star 
            GameObject star = createStar(starPrefab, 400, Color.white);
            // Instantiate Planet(s) 
            // "Lush"
            GameObject planet = createPlanet(planetPrefab, 75, 0, star, 120, Color.white, false);
            // Instantiate Junk only if the level is Incomplete
            if (levelCompletions[level, 1] == "Incomplete")
            {
                createJunk(junkPrefab, "Old Satellite", 100f, 0.5f, planet, 10, 90);
            }
        }
            else if (level == LUXE10_ID2) // Luxe-10
            {
                // Instantiate Star 
                GameObject star = createStar(starPrefab, 500, Color.red);
                // Instantiate Planet(s) 
                // "Dust", Gas Giant
                GameObject planet1 = createPlanet(planetPrefab, 50, 80, star, 80, Color.white, false);
                GameObject planet2 = createPlanet(planetPrefab, 120, 120, star, 250, Color.white, false);
                // Instantiate Junk 
                if (levelCompletions[level, 1] == "Incomplete")
                {
                    createJunk(junkPrefab, "Palladium Asteroid", 100f, 0.5f, star, 20, 90);
                    createJunk(junkPrefab, "Sealed Metal Box", 100f, 0.5f, planet2, 10, 90);
                }
            }
            else if (level == RIDGE65_ID3) // Ridge 
            {
                // Instantiate Star 
                GameObject star = createStar(starPrefab, 500, Color.red);
                // Instantiate Planet(s) 
                // Rocky, Sand, Gas 
                GameObject planet1 = createPlanet(planetPrefab, 50, 260, star, 120, Color.white, false);
                GameObject planet2 = createPlanet(planetPrefab, 80, 60, star, 120, Color.white, false);
                GameObject planet3 = createPlanet(planetPrefab, 120, 130, star, 200, Color.white, false);
                GameObject planet4 = createPlanet(planetPrefab, 150, 150, star, 250, Color.white, true);
                // Instantiate Junk 
                if (levelCompletions[level, 1] == "Incomplete")
                {
                    createJunk(junkPrefab, "Small Palladium Asteroid", 100f, 0.5f, planet1, 10, 90);
                    createJunk(junkPrefab, "Spaceship Crash Parts", 100f, 0.5f, planet3, 15, 90);
                }
            }
            else if (level == MAGNETIC73_ID4) // Magnetic 
            {
                // Instantiate Star 
                GameObject star = createStar(blackholePrefab, 200, Color.black);
                // Instantiate Planet(s) 
                // Ice planets, Gas giant
                GameObject planet1 = createPlanet(planetPrefab, 50, 80, star, 120, Color.white, false);
                GameObject planet2 = createPlanet(planetPrefab, 70, 120, star, 100, Color.white, false);
                GameObject planet3 = createPlanet(planetPrefab, 90, 180, star, 120, Color.white, false);
                GameObject planet4 = createPlanet(planetPrefab, 120, 230, star, 120, Color.white, false);
                GameObject planet5 = createPlanet(planetPrefab, 130, 10, star, 200, Color.white, false);
                createJunk(junkPrefab, "Degenerate Hyperdrive", 100f, 0.5f, star, 10, 90);
            }

        titleText.text = levelCompletions[level, 0];
        uncompletedBodies = allBodies; 
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
        if (levelCompletions[currentIncompleteLevel, 1] == "NewComplete")
        {
            levelCompletions[currentIncompleteLevel, 1] = "OldComplete";
        }
        // Unlock next level
        int nextLevel = currentIncompleteLevel + 1;
        if (nextLevel < levelCompletions.GetLength(0)) // GetLength(0) = number of rows
        {
            levelCompletions[nextLevel, 1] = "Incomplete";
        }

        // Update Starting Dialogue 
        shopkeeperDialogue.currentDialogue = SystemDialogues[currentIncompleteLevel]; 

        calculateValidLevels();
    }
    // Destroys all body gameobjects in a level 
    public void destroyAllBodies()
    {
        Debug.Log("Destroying all"); 
        foreach (GameObject body in allBodies)
        {
            if (body != null)
                Destroy(body);
        }
        allBodies.Clear(); // Optional: clear the list after deleting
        uncompletedBodies.Clear(); 
    }
    public GameObject createStar(GameObject starPrefab, float scale, Color color)
    {
        GameObject star = Instantiate(starPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        star.transform.localScale = new Vector3(scale, scale, scale);
        allBodies.Add(star);
        goalManager.setStarGoal(goalManager.getStarGoal() + 1); 
        // Change Color of Star
        Transform starMeshTransform = star.transform.Find("StarMesh");
        if (starMeshTransform != null)
        {
            MeshRenderer meshRenderer = starMeshTransform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            { 
                meshRenderer.material.color = color;  
            }
        }
        // Assign camera component if Black Hole
        if (starPrefab == blackholePrefab)
        {
            Transform eventHorizon = star.transform.Find("EventHorizon");
            if (eventHorizon != null)
            {
                eventHorizon.GetComponent<FaceCamera>().setCamera(camera3d.GetComponent<Camera>());
            }
        }
        return star; 
    }
    public GameObject createPlanet(GameObject planetPrefab, int distance, float angle, GameObject starToOrbit, float scale, Color color, bool ringed)
    {
        GameObject planet = Instantiate(planetPrefab, new Vector3(distance, 0, 0), Quaternion.identity);
        planet.transform.localScale = new Vector3(scale, scale, scale);
        planet.GetComponent<CircularPath>().setCenterTarget(starToOrbit, distance, angle);
        if (!ringed)
        {
            Transform ringTransform = planetPrefab.transform.Find("Ring");
            Debug.Log("Finding ring.."); 
            GameObject ring = ringTransform != null ? ringTransform.gameObject : null;
            if (ring != null)
            {
                Debug.Log("Found ring."); 
                ring.SetActive(false); 
            }
        }
        allBodies.Add(planet);
        goalManager.setPlanetGoal(goalManager.getPlanetGoal() + 1); 
        return planet; 
    }
    public void createJunk(GameObject junkPrefab, string junkName, float junkValue, float scale, GameObject objectToOrbit, int orbitDist, float orbitAngle)
    {
        GameObject junk = Instantiate(junkPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        junk.transform.localScale = new Vector3(scale, scale, scale);
        junk.GetComponent<CircularPath>().setCenterTarget(objectToOrbit, orbitDist, orbitAngle);
        junk.GetComponent<BodyStatus>().junkName = junkName;
        junk.GetComponent<BodyStatus>().junkValue = junkValue;
        goalManager.setJunkGoal(goalManager.getJunkGoal() + 1); 
        junk.GetComponent<FaceCamera>().setCamera(camera3d.GetComponent<Camera>()); 
        allBodies.Add(junk);
    }
}
