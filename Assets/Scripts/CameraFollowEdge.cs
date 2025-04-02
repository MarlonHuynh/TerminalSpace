using UnityEngine;

public class CameraFollowDirect : MonoBehaviour
{
    public Transform targetCamera; // The 3dCamera to follow

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // Directly follow the target camera's position
        transform.position = new Vector3(targetCamera.position.x, 75f, targetCamera.position.z);
    }
}
