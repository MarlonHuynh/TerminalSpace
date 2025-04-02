using UnityEngine;

public class LowResCamera : MonoBehaviour
{
    public Camera targetCamera; // Assign the target camera in the Inspector
    public int width = 320; // Width for 180p resolution
    public int height = 180; // Height for 180p resolution

    private RenderTexture lowResTexture;

    void Start()
    {
        if (targetCamera == null)
        {
            Debug.LogError("No camera assigned to the LowResCamera script!");
            return;
        }

        // Create and assign a RenderTexture
        lowResTexture = new RenderTexture(width, height, 16);
        lowResTexture.filterMode = FilterMode.Point; // Pixelated look
        targetCamera.targetTexture = lowResTexture;

        Debug.Log("Render texture assigned successfully.");
    }

    void OnDisable()
    {
        // Clean up the RenderTexture when this script is disabled
        if (lowResTexture != null)
        {
            lowResTexture.Release();
        }
    }
}
