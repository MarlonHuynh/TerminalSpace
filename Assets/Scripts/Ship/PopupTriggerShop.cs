using UnityEngine;
using TMPro; 

public class PopupTriggerShop : MonoBehaviour
{
    public PlayerMovement playerMovement; 
    public Shop shop; 
    public int itemNumTriggered; 
    public TextMeshProUGUI tmpro; 

    void Start(){
        if (tmpro == null){
            tmpro = GetComponent<TextMeshProUGUI>(); 
        }
    } 

    void OnTriggerEnter(Collider other) // Make opaque
    {  
        if (other.gameObject.CompareTag("Player") && tmpro != null)
        {
            Debug.Log("Triggered item#" + itemNumTriggered); 
            tmpro.color = new Color(tmpro.color.r, tmpro.color.g, tmpro.color.b, 1f); 
            shop.changeCurrentItem(itemNumTriggered); 
        }
    } 
}
