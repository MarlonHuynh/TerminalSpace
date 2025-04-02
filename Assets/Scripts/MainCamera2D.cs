using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera2D : MonoBehaviour
{
    private float moveX, moveZ; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get input for movement
        moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down 
 
        // Cam
        if (moveX < 0)
        {
            // Rotate camera 10 degrees left
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, -10f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
        }
        else if (moveX > 0)
        {
            // Rotate camera 10 degrees right
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, 10f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
        }
        else if (moveX == 0)
        {
            // Reset camera to 0 degrees
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
        } 
    }
}
