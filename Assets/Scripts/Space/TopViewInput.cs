using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewInput : MonoBehaviour
{
    public GameObject gameManager;
    public MiscSoundSFX miscSoundSFX; 
    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.E))
        { 
            gameManager.GetComponent<GameManager>().goShip();
        }
    }
}
