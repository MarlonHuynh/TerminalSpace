using UnityEngine;
using TMPro;

public class TextJitter : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float jitterAmount = 2f; // max pixels to move randomly
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = tmpText.rectTransform.anchoredPosition;
    }

    void Update()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-jitterAmount, jitterAmount),
            Random.Range(-jitterAmount, jitterAmount),
            0);

        tmpText.rectTransform.anchoredPosition = originalPosition + randomOffset;
    }
}
