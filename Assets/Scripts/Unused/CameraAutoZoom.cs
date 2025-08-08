using UnityEngine;

public class CameraAutoZoom : MonoBehaviour
{ 
    public float minZ = -4f; 
    public float maxZ = -9.5f; 
    public float targetZ;
    public float vertSpeed; 
    public float originalFov = 60f; 
    public float targetFOV = 30f; // The desired FOV after zoom
    public float zoomSpeed = 2f; // Speed of the zooming effect
    public float idleTimeThreshold = 10f; // Time before zooming starts

    private Vector3 lastPosition;
    private float idleTimer = 0f;
    public Camera cam;
    private bool isZooming = false;

    void Start()
    {
        lastPosition = transform.position;  
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right  

        float moveZ = Input.GetAxis("Vertical");
        // Only update targetZ when pressing keys
        if (moveZ > 0)
        {
            targetZ = minZ;
        }
        else if (moveZ < 0)
        {
            targetZ = maxZ;
        } 
        else{
            targetZ = transform.localPosition.z; 
        }
        // Smooth movement
        Vector3 currentPos = transform.localPosition;
        float newZ = Mathf.Lerp(currentPos.z, targetZ, Time.deltaTime * vertSpeed);
        transform.localPosition = new Vector3(currentPos.x, currentPos.y, newZ);

        // Cam
        if (moveX < 0)
        {
            // Rotate camera left over time
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, 355f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime); 
        }
        else if (moveX > 0)
        {
            // Rotate camera right over time
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, 5f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
        else if (moveX == 0)
        {
            // Reset camera to the center
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }   
        if (moveX != 0){
            idleTimer = 0f; 
            isZooming = false; 
            cam.fieldOfView = originalFov; 
        }  
        // Check if the camera has moved
        if (transform.position != lastPosition)
        {
            idleTimer = 0f; // Reset timer if movement is detected
            lastPosition = transform.position;
            isZooming = false; // Reset zooming state if moved
        }
        else
        {
            idleTimer += Time.deltaTime; // Increment timer if stationary
        }

        // If idle for 10 seconds, start zooming
        if (idleTimer >= idleTimeThreshold)
        {
            isZooming = true;
        }

        // Smoothly zoom to the target FOV
        if (isZooming)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        }  
    }
}
