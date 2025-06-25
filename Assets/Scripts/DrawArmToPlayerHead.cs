using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArmToPlayerHead : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject shopkeeper; 
    public Transform playerHead; 

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.4f;
        lineRenderer.endWidth = 0.4f; 
        lineRenderer.useWorldSpace = true;  // Ensure it's using world space
        lineRenderer.SetPosition(0, shopkeeper.transform.position);  // Set the initial position in world space
    }

    void Update()
    {
        // Update the positions in world space
        lineRenderer.SetPosition(0, shopkeeper.transform.position);
        lineRenderer.SetPosition(1, playerHead.position);  // Use world space position of playerHead
    }
}
