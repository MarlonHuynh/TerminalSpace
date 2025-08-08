using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // The camera the sprite should face
    public Camera targetCamera;
    public bool lockToY; 

    public void setCamera(Camera c)
    {
        targetCamera = c;
    }

    private void LateUpdate()
    {
        if (targetCamera != null)
        {
            Vector3 cameraDirection = targetCamera.transform.position - transform.position;

            if (lockToY)
            {
                // Flatten the direction on the Y-axis (ignore vertical difference)
                cameraDirection.y = 0f;

                // Prevent LookRotation error if direction becomes zero
                if (cameraDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(-cameraDirection);
                }
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(-cameraDirection, Vector3.up);
            }
        }
    }

}
