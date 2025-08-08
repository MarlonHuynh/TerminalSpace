using UnityEngine;

public class RotateContinuously : MonoBehaviour
{
    public float rotationSpeed = 10f;  

    void Update()
    { 
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
