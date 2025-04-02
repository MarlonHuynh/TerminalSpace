using UnityEngine;
using TMPro; 

public class PlayerMovement3d : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxSpeed = 10f; // New max speed variable

    private Rigidbody rb;
    private bool isGrounded;
    public GameObject terminalHitbox;
    private bool isTouchingTerminal = false;

    public TextMeshProUGUI navText;   
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        // Create a movement vector
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);

        // Limit the player's speed to the maximum speed
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Ignore vertical velocity
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed; // Clamp to max speed
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }

        if (moveX < 0)
        {
            // Rotate camera left over time
            Vector3 targetRotation = new Vector3(cameraTransform.eulerAngles.x, 350f, cameraTransform.eulerAngles.z);
            cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles, targetRotation, 5f * Time.deltaTime);
        }
        else if (moveX > 0)
        {
            // Rotate camera right over time
            Vector3 targetRotation = new Vector3(cameraTransform.eulerAngles.x, 10f, cameraTransform.eulerAngles.z);
            cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles, targetRotation, 5f * Time.deltaTime);
        }
        else if (moveX == 0)
        {
            // Reset camera to the center
            Vector3 targetRotation = new Vector3(cameraTransform.eulerAngles.x, 0f, cameraTransform.eulerAngles.z);
            cameraTransform.eulerAngles = Vector3.Lerp(cameraTransform.eulerAngles, targetRotation, 5f * Time.deltaTime);
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
    }
}
