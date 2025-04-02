using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // The camera the sprite should face
    public Camera targetCamera;

    private void Start()
    {
        // If no specific camera is assigned, default to the main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        // Make the sprite face the target camera
        if (targetCamera != null)
        {
            Vector3 cameraDirection = targetCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-cameraDirection, Vector3.up);
        }
    }
}
