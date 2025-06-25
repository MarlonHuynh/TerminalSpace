using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public bool shipNearShop = true;
    [Header("Movement Settings")]
    public bool jumpEnabled = true;
    public float jumpForce = 100f; 
    public float moveSpeed = 5f;
    public float maxSpeed = 10f; // New max speed variable  
    public GameObject shopHitbox;
    public GameObject shopCamera;
    public GameObject shipCamera;
    private Transform shipCameraTransform;
    private Rigidbody rb;
    public AudioSource walkAudio;
    //
    [Header("Hitboxes")]
    public RemoveableWall leftRemoveableWall;
    private GameObject gameManager;
    public GameObject terminalHitbox;
    public GameObject storageHitbox;
    public GameObject astronomiconHitbox;
    public GameObject solarshieldHitbox;
    public GameObject shopkeeperDialogueHitbox;
    public GameObject fuelHitbox;
    public TextMeshProUGUI navText;
    public TextMeshProUGUI storText;
    public TextMeshProUGUI astroText;
    public TextMeshProUGUI dockText;
    public TextMeshProUGUI solarshieldText;
    public TextMeshProUGUI fuelText;
    private bool isTouchingTerminal = false;
    private bool isTouchingShop = false;
    private bool isTouchingAstronomicon = false;
    private bool isTouchingStorage = false;
    private bool inShop = false;
    private bool isTouchingSolarshield = false;
    private bool isTouchingShopkeeperDialogueHitbox = false;
    private bool solarshieldDown = true;

    [Header("Items")]
    public bool carryingItem = false;
    public bool ownedItem = false;
    public ShopkeeperDialogue shopkeeperDialogue;
    public Shop shop;
    public GameObject headItem;
    public Material headMaterial;
    public GameObject item1Hitbox;
    private bool isTouchingItem1 = false;
    public GameObject item2Hitbox;
    private bool isTouchingItem2 = false;
    public GameObject item3Hitbox;
    private bool isTouchingItem3 = false;

    [Header("Sprites")]
    public MeshRenderer playerSpriteMeshRenderer;
    public Transform playerSpriteTransform;
    public Transform playerItemTransform;
    public Material playerF;
    public Material playerB;
    public Material playerB2;
    public Material playerL;
    public Material playerR;
    [Header("Other")]
    public bool movementEnabled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        shipCameraTransform = shipCamera.transform;
        gameManager = GameObject.Find("GameManager");
        Color clear = headItem.GetComponent<MeshRenderer>().material.color;
        clear.a = 0f;
        headItem.GetComponent<MeshRenderer>().material.color = clear;

    }

    void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        if (movementEnabled)
        {
            // Get input for movement
            float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
            float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

            Vector3 move;

            if (!inShop)
            {
                move = transform.right * moveX + transform.forward * moveZ;
            }
            else
            {
                move = transform.right * (-moveZ) + transform.forward * moveX;
            }

            // Set movement velocity (preserve Y velocity)
            Vector3 newVelocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
            rb.velocity = newVelocity;

            // Clamp horizontal speed
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }

            // Update sprite facing
            changeMaterialToCorrectFace(moveZ, moveX);

            // Sounds
            if ((moveX != 0 || moveZ != 0) && !walkAudio.isPlaying)
            {
                walkAudio.Play();
            }
            else if (moveX == 0 && moveZ == 0 && walkAudio.isPlaying)
            {
                walkAudio.Stop();
            }
            if (Input.GetKeyDown(KeyCode.B) && jumpEnabled)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
            }

            // SPACE Interactivity
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpEnabled)
                {
                    GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                if (isTouchingTerminal) // Go to top view
                    {
                        playerSpriteMeshRenderer.material = playerB2;
                        gameManager.GetComponent<GameManager>().goTopView();
                    }
                    else if (isTouchingSolarshield) // Close Shield
                    {
                        if (solarshieldDown)
                        {
                            GameObject.Find("FrontOutWall").GetComponent<RemoveableWall>().WallUp();
                            solarshieldDown = false;
                        }
                        else if (!solarshieldDown)
                        {
                            GameObject.Find("FrontOutWall").GetComponent<RemoveableWall>().WallDown();
                            solarshieldDown = true;
                        }
                    }
                    else if (isTouchingAstronomicon)
                    { // Open astronomicon
                        playerSpriteMeshRenderer.material = playerB2;
                        gameManager.GetComponent<GameManager>().goAstro();
                    }
                    else if (isTouchingStorage)
                    { // Open storage 
                        playerSpriteMeshRenderer.material = playerB2;
                        gameManager.GetComponent<GameManager>().goStorage();
                    }
                    else if (inShop == false && isTouchingShop && shipNearShop)
                    { // Change cam and open wall
                        playerSpriteTransform.rotation = Quaternion.Euler(0, -90f, 0); // Turn player sprite right 90 deg
                        playerItemTransform.rotation = Quaternion.Euler(0, -90f, 0);
                        shopCamera.SetActive(true);
                        shipCamera.SetActive(false);
                        inShop = true;
                        leftRemoveableWall.WallUp();
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
                        Debug.Log("Space on Item 1");
                        shop.performAction(0);
                    }
                    else if (inShop == true && isTouchingItem2)
                    { // Buy item 2
                        Debug.Log("Space on Item 2");
                        shop.performAction(1);
                    }
                    else if (inShop == true && isTouchingItem3)
                    { // Buy item 3
                        Debug.Log("Space on Item 3");
                        shop.performAction(2);
                    }
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the TerminalHitbox
        if (other.gameObject == terminalHitbox)
        {
            isTouchingTerminal = true;
            Color c = new Color(navText.color.r, navText.color.g, navText.color.b, 1f);
            navText.color = c;
        }
        else if (other.gameObject == storageHitbox)
        {
            Color c = new Color(storText.color.r, storText.color.g, storText.color.b, 1f);
            storText.color = c;
            isTouchingStorage = true;
        }
        else if (other.gameObject == astronomiconHitbox)
        {
            isTouchingAstronomicon = true;
            Color c = new Color(astroText.color.r, astroText.color.g, astroText.color.b, 1f);
            astroText.color = c;
        }
        else if (other.gameObject == solarshieldHitbox)
        {
            Color c = new Color(solarshieldText.color.r, solarshieldText.color.g, solarshieldText.color.b, 1f);
            solarshieldText.color = c;
            isTouchingSolarshield = true;
        }
        else if (other.gameObject == shopHitbox)
        {
            isTouchingShop = true;
            Color c = new Color(dockText.color.r, dockText.color.g, dockText.color.b, 1f);
            if (shipNearShop)
            {
                dockText.text = "Starship Port\n(Docked)";
            }
            else
            {
                dockText.text = "Starship Port\n(No Ship In Range)";
            }
            dockText.color = c;
        }
        else if (other.gameObject == fuelHitbox)
        {
            Color c = new Color(fuelText.color.r, fuelText.color.g, fuelText.color.b, 1f);
            fuelText.color = c;
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
            isTouchingTerminal = false;
            Color c = new Color(navText.color.r, navText.color.g, navText.color.b, 0f);
            navText.color = c;
        }
        else if (other.gameObject == storageHitbox)
        {
            Color c = new Color(storText.color.r, storText.color.g, storText.color.b, 0f);
            storText.color = c;
            isTouchingStorage = false;
        }
        else if (other.gameObject == astronomiconHitbox)
        {
            isTouchingAstronomicon = false;
            Color c = new Color(astroText.color.r, astroText.color.g, astroText.color.b, 0f);
            astroText.color = c;
        }
        else if (other.gameObject == solarshieldHitbox)
        {
            isTouchingSolarshield = false;
            Color c = new Color(solarshieldText.color.r, solarshieldText.color.g, solarshieldText.color.b, 0f);
            solarshieldText.color = c;
        }
        else if (other.gameObject == shopHitbox)
        {
            if (carryingItem == true && ownedItem == false
            && shopkeeperDialogue.warningThief == false)
            {
                shopkeeperDialogue.warningThief = true;
                shopkeeperDialogue.thiefReturnedItem = false;
                shopkeeperDialogue.startText();
            }
            else if ((carryingItem == true && ownedItem == true) || (carryingItem == false && ownedItem == false))
            {
                isTouchingShop = false;
                inShop = false;
                shopCamera.SetActive(false);
                shipCamera.SetActive(true);
                leftRemoveableWall.WallDown();
                Color c = new Color(dockText.color.r, dockText.color.g, dockText.color.b, 0f);
                dockText.color = c;
                playerSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
                playerItemTransform.rotation = Quaternion.Euler(0, -90f, 0);
            }
        }
        else if (other.gameObject == fuelHitbox)
        {
            Color c = new Color(fuelText.color.r, fuelText.color.g, fuelText.color.b, 0f);
            fuelText.color = c;
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

    void changeMaterialToCorrectFace(float moveX, float moveZ)
    {
        if (moveX != 0 || moveZ != 0)
        {
            // Face correct direction
            if (moveX > 0)
            {
                playerSpriteMeshRenderer.material = playerB;
            }
            else if (moveX < 0)
            {
                playerSpriteMeshRenderer.material = playerF;
            }
            if (moveZ > 0)
            {
                playerSpriteMeshRenderer.material = playerR;
            }
            else if (moveZ < 0)
            {
                playerSpriteMeshRenderer.material = playerL;
            }
            // Bounce the sprite for walking
            float rotationZ = Mathf.Sin(Time.time * 15f) * 4f;
            playerSpriteTransform.rotation = Quaternion.Euler(playerSpriteTransform.eulerAngles.x, playerSpriteTransform.eulerAngles.y, rotationZ);
            playerItemTransform.rotation = Quaternion.Euler(playerSpriteTransform.eulerAngles.x, playerSpriteTransform.eulerAngles.y, rotationZ);

        }
        else
        {
            playerSpriteTransform.rotation = Quaternion.Euler(playerSpriteTransform.eulerAngles.x, playerSpriteTransform.eulerAngles.y, playerSpriteTransform.rotation.z);
            playerItemTransform.rotation = Quaternion.Euler(playerSpriteTransform.eulerAngles.x, playerSpriteTransform.eulerAngles.y, playerSpriteTransform.rotation.z);
        }

    }
    // Stops any walking movement audio before switching 'scenes'
    void OnDisable() {
    if (walkAudio != null && walkAudio.isPlaying) {
        walkAudio.Stop();
    }
}
} 

