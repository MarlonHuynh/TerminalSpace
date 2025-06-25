using UnityEngine;

public class CircularPath : MonoBehaviour
{
    public bool orbitCenter = false; 
    public GameObject centerTarget; 
    public Transform centerPoint; // The point around which the object will rotate
    public float radius = 50f; // The radius of the circular path
    public float speed = 1f; // The speed of the movement along the path
    public float startingAngle = 0f; // Initial angle in degrees
    public float rotationSpeed = 30f; // Rotation speed of the object itself

    private float angle; // Angle in radians

    public 

    void Start()
    {
        // Convert starting angle to radians
        angle = startingAngle * Mathf.Deg2Rad;
    }
    public void setCenterTarget(GameObject centerTarget_in, int distance)
    {
        centerTarget = centerTarget_in;
        centerPoint = centerTarget.GetComponent<Transform>();
        radius = distance; 
    } 
    public void setCenterTarget(int distance)
    {
        orbitCenter = true;
        radius = distance;
    }

    void Update()
    {
        if (centerTarget != null && orbitCenter == false) 
        {
            // Increase the angle over time
            angle += speed * Time.deltaTime; 
            // Calculate the new position
            float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
            float z = centerPoint.position.z + Mathf.Sin(angle) * radius; 
            // Update the position of the object
            transform.position = new Vector3(x, transform.position.y, z); 
            // Rotate the object itself
            if (rotationSpeed > 0)
            {
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
        }
        else if (orbitCenter == true)
        {
            // Increase the angle over time
            angle += speed * Time.deltaTime; 
            // Calculate the new position
            float x = 0 + Mathf.Cos(angle) * radius;
            float z = 0 + Mathf.Sin(angle) * radius; 
            // Update the position of the object
            transform.position = new Vector3(x, transform.position.y, z); 
            // Rotate the object itself
            if (rotationSpeed > 0)
            {
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
        }
    }
}
