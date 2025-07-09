using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class CameraMovement : MonoBehaviour
{
    [Header("GameObjects")]
    public Button printBtn;
    public Button hookBtn;
    public PlayerMovement playerMovement;
    public TextMeshProUGUI fuelText;
    public GameObject gameManager;
    [Header("Movement")]
    public float moveSpeed = 10f;     // Speed for forward/backward movement
    public float turnSpeed = 100f;   // Speed for turning

    public float fuel = 200f;
    public float fuelRate = 1f; // Amt of fuel consumed per second 
    public float maxFuel = 200f;
    private float fuelTimer = 0f;
    private Vector2 inputDirection = Vector2.zero;
    public GameObject fuelTextObj; 
    [Header("Debug")]
    public bool movementEnabled = true; 
    public GameObject currentBody;
    public GameObject currentJunk;
    public int planetState = 0; // 0 = No planetary bodies or junk detected. 
                                // 1 = Too far from planetary body!
                                // 2 = Good distance from planetary body! 
                                // 3 = "" (Warning or crash)
    public int junkState = 0; // 0 = No junk
                              // Junk in range

    void Start()
    {
        fuelText = fuelTextObj.GetComponent<TextMeshProUGUI>();  
    }
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        if (fuel > 0 && movementEnabled)
        {
            // Move forward or backward
            Vector3 forwardMovement = transform.forward * vertical * moveSpeed * Time.deltaTime;
            // Apply the movement
            transform.position += forwardMovement;
            // Rotate the camera left or right
            float rotation = horizontal * turnSpeed * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }
        // Normalized dir vect
        inputDirection = new Vector2(vertical, horizontal).normalized;
        if (inputDirection != Vector2.zero)
        {
            fuelTimer += Time.deltaTime; // Increment timer based on elapsed time 
            if (fuelTimer >= 1f) // Check if one second has passed
            {
                fuel -= fuelRate; // Reduce fuel
                fuelTimer = 0f; // Reset the timer
                fuelText.text = "Fuel: " + fuel;
            }
        }
        else
        {
            fuelTimer = 0f; // Reset the timer if no input
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            printBtn.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            hookBtn.onClick.Invoke();
        }

        // CurrentBody
        if (currentBody != null)
        {
            gameManager.GetComponent<ObjectOnScreenCheck>().setTarget(currentBody);
        }
        // CurrentJunk
        if (currentJunk != null)
        {
            gameManager.GetComponent<ObjectOnScreenCheck>().setTarget(currentJunk);
        }
    }

    void OnEnable() 
    {
        updateFuelText(); 
    }

    public void updateFuelText()
    {
        fuelText.text = "Fuel: " + fuel;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the TerminalHitbox
        if (other.CompareTag("FarHitbox"))
        {
            planetState = 1; // Far
            currentBody = other.transform.parent.gameObject;
        }
        else if (other.CompareTag("PerfectHitbox"))
        {
            planetState = 2; // Planet in range
            currentBody = other.transform.parent.gameObject;
        }
        else if (other.CompareTag("CloseHitbox"))
        {
            planetState = 3; // " "
            gameManager.GetComponent<PrintSnapManager>().warning = true;
            currentBody = other.transform.parent.gameObject;
        }
        else if (other.CompareTag("CrashHitbox"))
        {
            planetState = 3; // " "
            gameManager.GetComponent<GameManager>().triggerCrashCutscene();
            currentBody = other.transform.parent.gameObject;
        }
        else if (other.CompareTag("JunkHitbox"))
        {
            junkState = 1; // Junk in range
            currentJunk = other.transform.parent.gameObject;
        }
        else if (other.CompareTag("ShopHitbox"))
        {
            playerMovement.shipNearShop = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        // Check if the collider is the TerminalHitbox
        if (other.CompareTag("CloseHitbox"))
        {
            planetState = 2; // Perfect
            gameManager.GetComponent<PrintSnapManager>().warning = false;
        }
        else if (other.CompareTag("PerfectHitbox"))
        {
            planetState = 1; // Far 
        }
        else if (other.CompareTag("FarHitbox"))
        {
            planetState = 0; // No detect
            currentBody = null;
        }
        else if (other.CompareTag("JunkHitbox"))
        {
            junkState = 0; // No detect
            currentJunk = null;
        }
        else if (other.CompareTag("ShopHitbox"))
        {
            playerMovement.shipNearShop = false;
        }
    }

    public void AddFuel(float f)
    {
        fuel += f;
        if (fuel > maxFuel)
        {
            fuel = maxFuel;
        }
        fuelText.text = "Fuel: " + fuel;
    }
    public void takeHyperjumpFuel()
    {
        if (fuel > 0)
        {
            fuel -= 20;
            if (fuel < 0)
            {
                fuel = 0;
            }
        }
        else if (fuel == 0)
        {
            Debug.Log("Jump failed. Explodes Ship."); 
        }
    }
}
