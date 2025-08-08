using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;             // Offset from the player
    public float xLimitL = 185f;
    public float xLimitR = 195f;
    public float zLimit = -3.5f;
    public float smoothSpeed = 5f;     // Speed of the lerp (tweakable)

    private void LateUpdate()
    {
        Vector3 targetPos = player.position + offset;

        // Lock Z if below limit
        if (player.position.z <= zLimit)
        {
            targetPos.z = zLimit + offset.z;
        }

        // Lock X if out of bounds
        if (player.position.x < xLimitL)
        {
            targetPos.x = xLimitL + offset.x;
        }
        else if (player.position.x > xLimitR)
        {
            targetPos.x = xLimitR + offset.x;
        }

        // Smoothly interpolate to the target position
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }
}
