using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveableWall : MonoBehaviour
{
    public float moveHeight = 10f; // Height to move
    public float moveSpeed = 2f;  // Speed at which the wall moves
    public AudioSource audioSource;
    public AudioClip openingSFX; 
    public AudioClip closingSFX;

    private Vector3 startPosition;  
    private Vector3 endPosition;  
    private Vector3 targetPosition; 
    [Header("Debug")]
    public bool isMovingUp = false;  
    public bool isMovingDown = false;  

    void Start()
    { 
        startPosition = transform.position; 
        endPosition = startPosition + Vector3.up * moveHeight;
        targetPosition = endPosition;  
    }

    void Update()
    {
        if (isMovingUp || isMovingDown)
        {
            // Move toward the current target at a constant speed
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop movement when the target is reached
            if (transform.position == targetPosition)
            {
                isMovingUp = false; 
                isMovingDown = false; 
            }
        }
    }

    public void WallUp()
    { 
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)// Only play sound when door is at the start position and Wall Up is called
            {
                audioSource.clip = openingSFX;
                audioSource.time = 0;
                audioSource.Play(); 
            }
            targetPosition = endPosition; // Set target to max position
            isMovingUp = true; // Start movement  
    }

    public void WallDown()
    { 
            if ((Vector3.Distance(transform.position, endPosition) < 0.01f) || isMovingUp) // Only play sound when door is at the end position and Wall Down is called
            {
                audioSource.clip = closingSFX;
                audioSource.time = 0;
                audioSource.Play(); 
            }
            targetPosition = startPosition; // Set target to min position
            isMovingUp = false; 
            isMovingDown = true; // Start movement  
    }
}
