using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopTextTrigger : MonoBehaviour
{
    public GameObject player; 
    public GameObject textObjectToTrigger;  
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player){
            textObjectToTrigger.SetActive(true); 
        }
    }
     void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player){
            textObjectToTrigger.SetActive(false); 
        }
    }
}
