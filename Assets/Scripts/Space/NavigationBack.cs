using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationBack : MonoBehaviour
{ 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            GameObject.Find("GameManager").GetComponent<GameManager>().goShip(); 
        }
    }
}
