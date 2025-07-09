using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingThroughSpace : MonoBehaviour
{
    public bool isFloating = false;
    public float startX;
    public float endX;
    public float ZRotationSpeed;
    public float floatSpeed;

    void Update()
    {
         if (isFloating)
        {
            Vector3 pos = transform.position;
            pos.x += floatSpeed * Time.deltaTime; 
            // Apply position update
            transform.position = pos; 
            // Stop floating if we've reached endX
            if (pos.x >= endX)
            {
                isFloating = false;
            } 
            // Apply rotation
            transform.Rotate(0, 0, ZRotationSpeed * Time.deltaTime);
        }
    }
    public void startFloat()
    {
        isFloating = true; 
    }
}
