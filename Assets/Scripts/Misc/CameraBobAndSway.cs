using UnityEngine;

public class CameraBobAndSway : MonoBehaviour
{
    public float horizontalSpeed = 0.5f;     // Side to side speed
    public float horizontalAmplitude = 0.5f; // Side to side distance

    public float verticalSpeed = 1.0f;       // Bobbing speed
    public float verticalAmplitude = 0.2f;   // Bobbing height

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float time = Time.time;

        float x = Mathf.Sin(time * horizontalSpeed) * horizontalAmplitude;
        float y = Mathf.Sin(time * verticalSpeed) * verticalAmplitude;

        transform.position = startPos + new Vector3(x, y, 0f);
    }
}
