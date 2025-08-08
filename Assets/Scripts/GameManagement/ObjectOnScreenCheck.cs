using UnityEngine;

public class ObjectOnScreenCheck : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject targetObject; 
    public float leftTolerance = 0.25f; 
    public float rightTolerance = 0.25f; 

    public bool checkInView()
    {
        if (targetObject != null){
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(targetObject.transform.position); 
            bool isInView = screenPoint.z >= 0 && screenPoint.x >= (0 + leftTolerance) && screenPoint.x <= (Screen.width + rightTolerance);  
            return isInView; 
        }
        return false;
    }

    public void setTarget(GameObject o){
        targetObject = o; 
    }
}
