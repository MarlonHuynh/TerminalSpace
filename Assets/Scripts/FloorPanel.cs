using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPanel : MonoBehaviour
{
    private bool opened; 
    public Transform floorPivotL;
    public float lerpSpeed = 2f;
    public float desiredRotationDegree; 
    private Quaternion targetRotation;

    void Start()
    {
        opened = false;   
    }
    public void open()
    {
        opened = true; 
        targetRotation = Quaternion.Euler(floorPivotL.eulerAngles.x, floorPivotL.eulerAngles.y, -desiredRotationDegree);
    }

    void Update()
    {
        if (opened == true)
        {
            floorPivotL.rotation = Quaternion.Lerp(floorPivotL.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        }
    }
}
