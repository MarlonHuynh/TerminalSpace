/*
    Makes the fuel tank object move up and down according to the amount of fuel     
*/

using UnityEngine;

public class FuelTankMovement : MonoBehaviour
{
    public float amplitude = 0.5f;  
    public float frequency = 1f;    
    private Vector3 currentPos;
    public float lowestPos = -10f;  
    [Range(0f, 100f)]
    public float value = 100f;  

    void Start()
    {
        currentPos = transform.position;
    }

    void Update()
    {
        Vector3 tempPos = currentPos; 
        // Base position
        float t = Mathf.Clamp01(value / 100f);
        float baseHeight = Mathf.Lerp(currentPos.y + lowestPos, currentPos.y, t); 
         // Add bobbing on top of base height
        float bobbingOffset = Mathf.Sin(Time.time * frequency) * amplitude; 
        tempPos.y = baseHeight + bobbingOffset; 
        transform.position = tempPos;
    }

    public void setFuel(float f){
        value = f; 
    }
}
