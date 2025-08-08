// Attach to TMProUGUI 

using UnityEngine;
using TMPro;

public class PopupTriggerShopBG : MonoBehaviour
{
    public GameObject obj;
    public TextMeshProUGUI tmp; 

    void OnTriggerEnter(Collider other) // Make opaque
    {
        if (other.gameObject.CompareTag("Player") && obj != null)
        {
            obj.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) // Make transparent
    {
        if (other.gameObject.CompareTag("Player") && obj != null)
        {
            tmp.text = ""; 
            obj.SetActive(false);
        }
    }
}