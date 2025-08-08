using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public GameObject player;

    private Vector3 initialForward;
    private Vector3 initialPosition;

    public float bobSpeed = 1f;    // Speed of bobbing (cycles per second)
    public float bobHeight = 0.25f; // How far up/down it bobs
 
    void Start()
    {
        // Store the initial forward direction (on XZ plane)
        initialForward = transform.forward;
        initialForward.y = 0;
        initialForward.Normalize();

        // Store the original starting position
        initialPosition = transform.position; 
    }

    void LateUpdate()
    {
        Vector3 toPlayer = player.transform.position - transform.position;

        // Flatten direction to XZ plane
        toPlayer.y = 0;

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            toPlayer.Normalize();

            float angleFromStart = Vector3.SignedAngle(initialForward, toPlayer, Vector3.up); 

            // Optional rotation if behind
            if ((angleFromStart > 150 && angleFromStart < 179) || (angleFromStart > -179 && angleFromStart < -160))
            {
                Quaternion targetRotation = Quaternion.LookRotation(toPlayer);
                transform.rotation = targetRotation;
            }
        }

        // Bobbing motion using sine wave
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed * 2f * Mathf.PI) * bobHeight;
        Vector3 bobbedPosition = new Vector3(transform.position.x, newY, transform.position.z);
        transform.position = bobbedPosition;
    }
}
