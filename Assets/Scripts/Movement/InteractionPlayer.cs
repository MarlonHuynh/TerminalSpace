using System.Collections;
using UnityEngine;
using TMPro; 

public class InteractionPlayer : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement movementPlayer; 
    public GameObject shopHitbox;
    public Camera shopCamera;
    public Camera shipCamera;
    private Transform shipCameraTransform;
    private Rigidbody rb;
    public AudioSource walkAudio;
    public Light spotlight;
    public Light shiplight;
    public MiscSoundSFX miscSoundSFX; 
    //
    [Header("Hitboxes")]
    public RemoveableWall leftRemoveableWall; 
    public GameObject terminalHitbox;
    public GameObject storageHitbox;
    public GameObject astronomiconHitbox;
    public GameObject solarshieldHitbox;
    public GameObject shopkeeperDialogueHitbox;
    public GameObject fuelHitbox;  
    public TextMeshProUGUI dockingPopup; 

    private bool isTouchingNavTerminal = false;
    private bool isTouchingShop = false;
    private bool isTouchingAstronomicon = false;
    private bool isTouchingStorage = false;
    public bool inShop = false;
    private bool isTouchingSolarshield = false;
    private bool isTouchingShopkeeperDialogueHitbox = false;
    private bool solarshieldDown = true;

    [Header("Items")]
    public bool carryingItem = false;
    public bool ownedItem = false;
    public ShopkeeperDialogue shopkeeperDialogue;
    public Shop shop; 
    public GameObject item1Hitbox;
    private bool isTouchingItem1 = false;
    public GameObject item2Hitbox;
    private bool isTouchingItem2 = false;
    public GameObject item3Hitbox;
    private bool isTouchingItem3 = false;

    [Header("Sprites")]
    public MeshRenderer playerSpriteMeshRenderer;
    public Transform playerSpriteTransform; 
    public Material playerB2; 
    [Header("Other")]
    //public bool movementEnabled = false;
    public bool shipNearShop = true;
    private bool firstTimeExitShop = false; 


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        shipCameraTransform = shipCamera.transform; 

    }

    void Update()
    { 
        if (movementPlayer.movementEnabled == true)
        { 
            // SPACE Interactivity
            if (Input.GetKeyDown(KeyCode.Space))
            { 
                if (isTouchingNavTerminal) // Go to top view
                {
                    miscSoundSFX.playAnalogBeep(); 
                    playerSpriteMeshRenderer.material = playerB2;
                    gameManager.GetComponent<GameManager>().goTopView();
                }
                else if (isTouchingSolarshield) // Close Shield
                {
                    miscSoundSFX.playAnalogBeep(); 
                    if (solarshieldDown)
                    {
                        Debug.Log("U!");
                        GameObject.Find("FrontOutWall").GetComponent<RemoveableWall>().WallUp();
                        solarshieldDown = false;
                    }
                    else if (!solarshieldDown)
                    {
                        Debug.Log("D!");
                        GameObject.Find("FrontOutWall").GetComponent<RemoveableWall>().WallDown();
                        solarshieldDown = true;
                    }
                }
                else if (isTouchingAstronomicon)
                { // Open astronomicon
                    miscSoundSFX.playAnalogBeep(); 
                    playerSpriteMeshRenderer.material = playerB2;
                    gameManager.GetComponent<GameManager>().goAstro();
                }
                else if (isTouchingStorage)
                { // Open storage 
                    miscSoundSFX.playAnalogBeep(); 
                    playerSpriteMeshRenderer.material = playerB2;
                    gameManager.GetComponent<GameManager>().goStorage();
                }
                else if (inShop == false && isTouchingShop && shipNearShop)
                { // Change cam and open wall 
                    playerSpriteTransform.rotation = Quaternion.Euler(0, -90f, 0); // Turn player sprite right 90 deg 
                    shopCamera.gameObject.SetActive(true);
                    shipCamera.gameObject.SetActive(false);
                    inShop = true;
                    leftRemoveableWall.WallUp();
                    dockingPopup.color = new Color(dockingPopup.color.r, dockingPopup.color.g, dockingPopup.color.b, 0f); 
                }
                else if (inShop == true && isTouchingShopkeeperDialogueHitbox)
                { // Talk to shopkeeper
                    if (shopkeeperDialogue.dialogueDone == false)
                    {
                        shopkeeperDialogue.startText();
                    }
                }
                else if (inShop == true && isTouchingItem1)
                { // Buy item 1 
                    shop.takeCurrentItem();
                }
                else if (inShop == true && isTouchingItem2)
                { // Buy item 2
                    shop.takeCurrentItem();
                }
                else if (inShop == true && isTouchingItem3)
                { // Buy item 3
                    shop.takeCurrentItem();
                }
            } 
        }

    } 
    void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject == terminalHitbox)
        {
            isTouchingNavTerminal = true; 
        }
        else if (other.gameObject == storageHitbox)
        { 
            isTouchingStorage = true;
        }
        else if (other.gameObject == astronomiconHitbox)
        {
            isTouchingAstronomicon = true; 
        }
        else if (other.gameObject == solarshieldHitbox)
        { 
            isTouchingSolarshield = true;
        }
        else if (other.gameObject == shopHitbox)
        {
            isTouchingShop = true; 
            if (shipNearShop)
            {
                dockingPopup.text = "Starship Port\n(Docked)";
            }
            else
            {
                dockingPopup.text = "Starship Port\n(No Ship In Range)";
            } 
        }
        else if (other.gameObject == fuelHitbox)
        { 

        }
        else if (other.gameObject == shopkeeperDialogueHitbox)
        {
            isTouchingShopkeeperDialogueHitbox = true;
        } 
        else if (other.gameObject == item1Hitbox)
        {
            isTouchingItem1 = true;
        }
        else if (other.gameObject == item2Hitbox)
        {
            isTouchingItem2 = true;
        }
        else if (other.gameObject == item3Hitbox)
        {
            isTouchingItem3 = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Reset isTouchingTerminal when leaving the hitbox
        if (other.gameObject == terminalHitbox)
        {
            isTouchingNavTerminal = false; 
        }
        else if (other.gameObject == storageHitbox)
        { 
            isTouchingStorage = false;
        }
        else if (other.gameObject == astronomiconHitbox)
        {
            isTouchingAstronomicon = false; 
        }
        else if (other.gameObject == solarshieldHitbox)
        {
            isTouchingSolarshield = false; 
        }
        else if (other.gameObject == shopHitbox)
        {
            isTouchingShop = false;
            inShop = false;
            shopCamera.gameObject.SetActive(false);
            shipCamera.gameObject.SetActive(true);
            leftRemoveableWall.WallDown(); 
            playerSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
            if (firstTimeExitShop == false && shopkeeperDialogue.firstTalkedYet == true)
            {
                firstTimeExitShop = true; 
                StartCoroutine(StartCoLightOn());
            }
        }
        else if (other.gameObject == fuelHitbox)
        { 

        } 
        else if (other.gameObject == shopkeeperDialogueHitbox)
        {
            isTouchingShopkeeperDialogueHitbox = false;
        }
        else if (other.gameObject == item1Hitbox)
        {
            isTouchingItem1 = false;
        }
        else if (other.gameObject == item2Hitbox)
        {
            isTouchingItem2 = false;
        }
        else if (other.gameObject == item3Hitbox)
        {
            isTouchingItem3 = false;
        }
    } 

    IEnumerator StartCoLightOn()
    {
        spotlight.intensity = 0f;
        yield return new WaitForSeconds(0.1f);
        spotlight.intensity = 75f;
        yield return new WaitForSeconds(0.1f);
        spotlight.intensity = 0f;
        yield return new WaitForSeconds(1f);
        shiplight.intensity = 1000f;
        yield return new WaitForSeconds(0.1f);
        shiplight.intensity = 0f;
        yield return new WaitForSeconds(0.1f);
        shiplight.intensity = 1000f;
        yield return new WaitForSeconds(0.1f);
        shiplight.intensity = 0f; 
        yield return new WaitForSeconds(0.1f);
        shiplight.intensity = 1000f; 

    }

     
    // Stops any walking movement audio before switching 'scenes'
    void OnDisable() {
        if (walkAudio != null && walkAudio.isPlaying) {
            walkAudio.Stop();
        }
    }
} 

