using System.Collections;
using System.Collections.Generic;  
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements; 

public class NearestBodyFinder : MonoBehaviour
{
    [Header("Debug / Stats")]
    public GameObject nearestBody; 
    public float updateInterval = 0.5f; 
    private float targetAngleZ = 0f;
    public float rotationSmoothSpeed = 5f; 

    [Header("Required")]
    public Transform playerTransform;  
    public LevelLoader levelLoader;
    public UnityEngine.UI.Image pointer; 
    private List<GameObject> completedBodies = new List<GameObject>(); // ref
    private bool canCour = true;

    void Start()
    {
        completedBodies = levelLoader.allBodies; 
        canCour = true; 
    }
    void OnEnable()
    {
        completedBodies = levelLoader.allBodies;
        canCour = true; 
    }
    void Update()
    {
        if (canCour)
        {
            canCour = false;
            StartCoroutine(UpdateNearestBodyRoutine()); 
        } 
        // Get current z rotation
        float currentZ = pointer.rectTransform.localEulerAngles.z; 
        // Smoothly interpolate angle using Mathf.LerpAngle (handles wraparound 0-360)
        float smoothZ = Mathf.LerpAngle(currentZ, targetAngleZ, Time.deltaTime * rotationSmoothSpeed);
        pointer.rectTransform.localEulerAngles = new Vector3(0, 0, smoothZ);
    }
    IEnumerator UpdateNearestBodyRoutine()
    {
        nearestBody = FindNearestBodyStatus();
        if (nearestBody != null)
        {
            // Calculates the direction and rotate the pointer appropriately
            Vector3 direction3D = nearestBody.transform.position - playerTransform.position;
            Vector2 direction = new Vector2(direction3D.x, direction3D.z); // Use X and Z! 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetAngleZ = angle - 90f;
        }
        yield return new WaitForSeconds(updateInterval); 
        canCour = true; 
    }
    GameObject FindNearestBodyStatus()
    { 
        // Vars
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;  
        Vector2 playerPos2D = new Vector2(playerTransform.position.x, playerTransform.position.z); // USE X, Z PLANE!! 
        // Keeps track of any removals needed
        List<GameObject> tempRemovalList = new List<GameObject>();

        foreach (GameObject body in completedBodies)
        {
            if (body == null) { // Skip null (safety), also for destroyed Junks...
                Debug.Log("Found null body");
                tempRemovalList.Add(body);
                continue;
            }
            // Checks if already obtained, if so destroy from list (synced)
            if (body.GetComponent<BodyStatus>() != null)
            {
                if (body.GetComponent<BodyStatus>().obtained == true)
                {
                    Debug.Log("Body is already obtained! Deleting from list");
                    tempRemovalList.Add(body);
                    continue;
                }
            }
            // Calculate dist to target
            Vector2 bodyPos2D = new Vector2(body.transform.position.x, body.transform.position.z);
            Vector2 directionToTarget = bodyPos2D - playerPos2D;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            // Find closest
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = body;
            } 
        }  

         // Remove bodies after the loop
        foreach (var b in tempRemovalList)
        {
            completedBodies.Remove(b);
        }

        return closest;
    }

}
