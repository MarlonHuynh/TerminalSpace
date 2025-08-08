// Attach to TMProUGUI 

using UnityEngine;
using TMPro; 

public class PopupTrigger : MonoBehaviour
{    
    public TextMeshProUGUI tmp; 

    void Start(){
        if (tmp == null){
            tmp = GetComponent<TextMeshProUGUI>(); 
        }
    }

    void OnTriggerEnter(Collider other) // Make opaque
    {  
        if (other.gameObject.CompareTag("Player") && tmp != null)
        { 
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1f); 
        }
    }

    void OnTriggerExit(Collider other) // Make transparent
    {
        if (other.gameObject.CompareTag("Player") && tmp != null)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0f); 
        }
    }
}
