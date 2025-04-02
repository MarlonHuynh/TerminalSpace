using TMPro;
using UnityEngine;

public class ChangeSortingOrder : MonoBehaviour
{ 
    public int sortOrder = 0; // Sorting order value

    void Start()
    {
        // Get the Renderer component of the TMP object
        Renderer renderer = GetComponent<Renderer>(); 
        renderer.sortingOrder = sortOrder; 
    }
}