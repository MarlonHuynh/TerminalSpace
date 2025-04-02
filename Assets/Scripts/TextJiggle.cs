using UnityEngine;
using TMPro;

public class JiggleText : MonoBehaviour
{
    public TMP_Text textMeshPro; // Assign your TextMeshPro component in the Inspector
    public float amplitude = 1f; // How much each character moves
    public float frequency = 1f; // How quickly each character moves

    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }

        if (textMeshPro == null)
        {
            Debug.LogError("No TextMeshPro component found!");
            enabled = false;
            return;
        }

        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;

        // Store the original vertices
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            originalVertices[i] = new Vector3[textInfo.meshInfo[i].vertices.Length];
        }
    }

    void Update()
    {
        if (textMeshPro == null) return;

        // Update the mesh and text info
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;

        // Loop through each character
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            // Get the vertices of the character
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Store original vertices
            if (originalVertices[materialIndex].Length > vertexIndex + 3)
            {
                originalVertices[materialIndex][vertexIndex + 0] = vertices[vertexIndex + 0];
                originalVertices[materialIndex][vertexIndex + 1] = vertices[vertexIndex + 1];
                originalVertices[materialIndex][vertexIndex + 2] = vertices[vertexIndex + 2];
                originalVertices[materialIndex][vertexIndex + 3] = vertices[vertexIndex + 3];
            }

            // Calculate jiggle offset
            float offsetX = Mathf.Sin(Time.time * frequency + i) * amplitude;
            float offsetY = Mathf.Cos(Time.time * frequency + i) * amplitude;
            Vector3 offset = new Vector3(offsetX, offsetY, 0);

            // Apply the jiggle to each vertex of the character
            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        // Update the mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
