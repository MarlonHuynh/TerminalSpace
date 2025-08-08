using UnityEngine;

public class FaceAxis : MonoBehaviour
{
    public int lookInt = 0; // 0 = forward, 1 = back, 2 = left, 3 = right
    private void LateUpdate()
    {
        // Set the rotation to face the +Z direction
        if (lookInt == 0){
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
        else if (lookInt == 1){
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        }
        else if (lookInt == 2){
            transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        }
        else if (lookInt == 3){
            transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        }
    }
}
