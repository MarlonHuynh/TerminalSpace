using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;  // Assign your custom cursor texture in the Inspector
    public Vector2 hotspot = Vector2.zero; // Set the hotspot (pivot point) of the cursor

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
