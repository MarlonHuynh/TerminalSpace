using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveableWall : MonoBehaviour
{
    public float moveHeight = 10f; // Height to move
    public float moveSpeed = 2f;  // Speed at which the wall moves

    private Vector3 startPosition; // Initial position
    private Vector3 targetPosition; // Maximum upward position
    private Vector3 currentTarget; // Current movement target
    private bool isMoving = false; // Is the wall currently moving?

    void Start()
    { 
        startPosition = transform.position; 
        targetPosition = startPosition + Vector3.up * moveHeight;
        currentTarget = startPosition; // Initially at the start position
    }

    void Update()
    {
        if (isMoving)
        {
            // Move toward the current target at a constant speed
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);

            // Stop movement when the target is reached
            if (transform.position == currentTarget)
            {
                isMoving = false; // Stop movement
            }
        }
    }

    public void WallUp()
    {
        if (!isMoving || currentTarget != targetPosition)
        {
            currentTarget = targetPosition; // Set target to max position
            isMoving = true; // Start movement
        }
    }

    public void WallDown()
    {
        if (!isMoving || currentTarget != startPosition)
        {
            currentTarget = startPosition; // Set target to start position
            isMoving = true; // Start movement
        }
    }
}
