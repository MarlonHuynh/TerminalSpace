using System.Collections;
using UnityEngine;
using TMPro; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxSpeed = 10f; // New max speed variable  
    public GameObject shopHitbox;  
    public GameObject shopCamera; 
    public GameObject shipCamera; 
    private Transform shipCameraTransform; 
    private Rigidbody rb;  
    public GameObject terminalHitbox; 
    public GameObject storageHitbox; 
    public GameObject astronomiconHitbox; 
    public GameObject solarshieldHitbox; 
    public GameObject shopkeeperDialogueHitbox;  
    private GameObject gameManager; 
    public TextMeshProUGUI navText;   
    public TextMeshProUGUI storText;   
    public TextMeshProUGUI astroText;   
    public TextMeshProUGUI dockText;   
    public TextMeshProUGUI solarshieldText;   
    private bool isTouchingTerminal = false;
    private bool isTouchingShop = false; 
    private bool isTouchingAstronomicon = false; 
    private bool isTouchingStorage = false; 
    private bool inShop = false; 
    private bool isTouchingSolarshield = false; 
    private bool isTouchingShopkeeperDialogueHitbox = false; 
    private bool solarshieldDown = true; 
    public MeshRenderer playerSpriteMeshRenderer; 
    public Transform playerSpriteTransform; 
    public Material playerF; 
    public Material playerB; 
    public Material playerB2; 
    public Material playerL; 
    public Material playerR;  
    public bool movementEnabled = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        shipCameraTransform = shipCamera.transform; 
        gameManager = GameObject.Find("GameManager"); 
    }

    void Update()
    {
        MovePlayer();
    } 
    void MovePlayer()
    {
        if (movementEnabled){
            // Get input for movement
            float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
            float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

            Vector3 horizontalVelocity = new Vector3(); 
            // Create and apply a movement vector, then clamp speed
            if (!inShop){
                Vector3 move = transform.right * moveX + transform.forward * moveZ; 
                rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed); 
                horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Ignore vertical velocity  
                checkFace(moveZ, moveX); 

            }
            else if (inShop){
                Vector3 move = transform.right * (-moveZ) + transform.forward * moveX; 
                rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed); 
                horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);   
                checkFace(moveZ, moveX); 
            }  
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed; // Clamp to max speed
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            } 

            // Interactivity
            if (Input.GetKeyDown(KeyCode.Space)){
                if (isTouchingTerminal)
                {  
                    playerSpriteMeshRenderer.material = playerB2;  
                    gameManager.GetComponent<GameManager>().goTopView(); 
                }
                if (isTouchingSolarshield)
                {
                    if (solarshieldDown){
                        GameObject.Find("FrontOutWall").GetComponent<RemoveableWall>().WallUp(); 
                        solarshieldDown = false; 
                    } 
                    else if (!solarshieldDown){
                        GameObject.Find("FrontOutWall").GetComponent<RemoveableWall>().WallDown(); 
                        solarshieldDown = true; 
                    } 
                }
                if (isTouchingShop){ 
                    playerSpriteTransform.rotation = Quaternion.Euler(0, -90f, 0);
                    shopCamera.SetActive(true); 
                    shipCamera.SetActive(false);  
                    inShop = true; 
                    GameObject.Find("LeftOutWall").GetComponent<RemoveableWall>().WallUp();  
                }
                if (isTouchingAstronomicon){ 
                    playerSpriteMeshRenderer.material = playerB2;  
                    gameManager.GetComponent<GameManager>().goAstro(); 
                }
                if (isTouchingShopkeeperDialogueHitbox){ 
                    if (gameManager.GetComponent<ShopkeeperDialogue>().dialogueDone == false){ 
                        movementEnabled = false; 
                        gameManager.GetComponent<ShopkeeperDialogue>().startText(); 
                    }
                }
                if (isTouchingStorage){  
                    movementEnabled = false; 
                    gameManager.GetComponent<GameManager>().goStorage();  
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
            Color c = new Color(navText.color.r,navText.color.g,navText.color.b, 1f);
            navText.color = c; 
        }
        else if (other.gameObject == storageHitbox)
        { 
            Color c = new Color(storText.color.r,storText.color.g,storText.color.b, 1f);
            storText.color = c; 
            isTouchingStorage = true; 
        }
        else if (other.gameObject == astronomiconHitbox)
        { 
            isTouchingAstronomicon = true; 
            Color c = new Color(astroText.color.r,astroText.color.g,astroText.color.b, 1f);
            astroText.color = c; 
        }
        else if (other.gameObject == solarshieldHitbox)
        { 
            Color c = new Color(solarshieldText.color.r,solarshieldText.color.g,solarshieldText.color.b, 1f);
            solarshieldText.color = c; 
            isTouchingSolarshield = true; 
        }
        else if (other.gameObject == shopHitbox)
        {
            isTouchingShop = true; 
            Color c = new Color(dockText.color.r,dockText.color.g,dockText.color.b, 1f);
            dockText.color = c; 
        } 
        else if (other.gameObject == shopkeeperDialogueHitbox)
        { 
            isTouchingShopkeeperDialogueHitbox = true;  
        }  
    }

    void OnTriggerExit(Collider other)
    {
        // Reset isTouchingTerminal when leaving the hitbox
        if (other.gameObject == terminalHitbox)
        {
            isTouchingTerminal = false;
            Color c = new Color(navText.color.r,navText.color.g,navText.color.b, 0f);
            navText.color = c; 
        }
        else if (other.gameObject == storageHitbox)
        { 
            Color c = new Color(storText.color.r,storText.color.g,storText.color.b, 0f);
            storText.color = c; 
            isTouchingStorage = false; 
        }
        else if (other.gameObject == astronomiconHitbox)
        { 
            isTouchingAstronomicon = false; 
            Color c = new Color(astroText.color.r,astroText.color.g,astroText.color.b, 0f);
            astroText.color = c; 
        }
        else if (other.gameObject == solarshieldHitbox)
        { 
            isTouchingSolarshield = false; 
            Color c = new Color(solarshieldText.color.r,solarshieldText.color.g,solarshieldText.color.b, 0f);
            solarshieldText.color = c;  
        }
        else if (other.gameObject == shopHitbox)
        {
            isTouchingShop = false; 
            inShop = false;    
            shopCamera.SetActive(false); 
            shipCamera.SetActive(true); 
            GameObject.Find("LeftOutWall").GetComponent<RemoveableWall>().WallDown(); 
            Color c = new Color(dockText.color.r,dockText.color.g,dockText.color.b, 0f);
            dockText.color = c; 
            playerSpriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (other.gameObject == shopkeeperDialogueHitbox)
        {
            isTouchingShopkeeperDialogueHitbox = false;  
        } 
    }

    void checkFace(float moveX, float moveZ){
        if (moveX != 0 || moveZ != 0){
            // Face correct direction
            if (moveX > 0){
                playerSpriteMeshRenderer.material = playerB; 
            }
            else if (moveX < 0){
                playerSpriteMeshRenderer.material = playerF;
            }
            if (moveZ > 0){
                playerSpriteMeshRenderer.material = playerR; 
            }
            else if (moveZ < 0){
                playerSpriteMeshRenderer.material = playerL;
            }
            // Bounce the sprite for walking
            float rotationZ = Mathf.Sin(Time.time * 15f) * 4f;
            playerSpriteTransform.rotation = Quaternion.Euler(playerSpriteTransform.eulerAngles.x, playerSpriteTransform.eulerAngles.y, rotationZ);
        }
        else{
            playerSpriteTransform.rotation = Quaternion.Euler(playerSpriteTransform.eulerAngles.x, playerSpriteTransform.eulerAngles.y, playerSpriteTransform.rotation.z);
        }

    }
} 
