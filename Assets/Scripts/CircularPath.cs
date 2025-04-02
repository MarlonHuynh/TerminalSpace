using UnityEngine;

public class CircularPath : MonoBehaviour
{
    public Transform centerPoint; // The point around which the object will rotate
    public float radius = 50f; // The radius of the circular path
    public float speed = 1f; // The speed of the movement along the path
    public float startingAngle = 0f; // Initial angle in degrees
    public float rotationSpeed = 30f; // Rotation speed of the object itself

    private float angle; // Angle in radians

    void Start()
    {
        // Convert starting angle to radians
        angle = startingAngle * Mathf.Deg2Rad;
    }

    void Update()
    {
        if (centerPoint != null)
        {
            // Increase the angle over time
            angle += speed * Time.deltaTime;

            // Calculate the new position
            float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
            float z = centerPoint.position.z + Mathf.Sin(angle) * radius;

            // Update the position of the object
            transform.position = new Vector3(x, transform.position.y, z);

            // Rotate the object itself
            if (rotationSpeed > 0){
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }   
        }
    }
}
