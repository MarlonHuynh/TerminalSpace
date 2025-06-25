using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float money = 50; 
    public ShopkeeperDialogue shopkeeperDialogue; 
    public PlayerMovement playerMovement;
    public CameraMovement cameraMovement; 
    [Header("Current")]
    public GameObject headItem; 
    public int currentItemInt;  
    public Texture currentTexture; 
    public string currentName;
    public int currentPrice; 
    [Header("Textures")]
    public Texture smallFuel, bigFuel, hookUpgrade, flowers;  
    [Header("World Items")] 
    public GameObject item1;
    public GameObject item2; 
    public GameObject item3;
    public TextMeshProUGUI item1Text; 
    public TextMeshProUGUI item2Text; 
    public TextMeshProUGUI item3Text; 
    public TextMeshProUGUI moneyText;  
    private int[] prices = new int[3]; 

    void Start(){
        moneyText.text = "Credits: " + money; 
        //
        item1Text.text = "Small Fuel [50C]\nFills your ship's fuel by a small amount."; 
        prices[0] = 50;  
        item2Text.text = "Large Fuel [100C]\nFills your ship's fuel by a large amount. ";
        prices[1] = 100;  
        item3Text.text = "Hook Upgrade [300C]\nAllows you to sell junk at a better rate."; 
        prices[2] = 300; 
        //
        item1.GetComponent<MeshRenderer>().material.mainTexture = smallFuel; 
        item2.GetComponent<MeshRenderer>().material.mainTexture = bigFuel;
        item3.GetComponent<MeshRenderer>().material.mainTexture = hookUpgrade;
    } 
    public void performAction(int index)
    {  
        if (index == 0){
            takeItem(item1, item1Text, 0); 
        }
        else if (index == 1)
        {
            takeItem(item2, item2Text, 1); 
        }
        else if (index == 2)
        {
            takeItem(item3, item3Text, 2); 
        }
    }

    public void takeItem(GameObject item, TextMeshProUGUI itemText, int i)
    {
        if (prices[i] <= money)
        {
            money -= prices[i];
            // Apply effect of item
            if (item.GetComponent<MeshRenderer>().material.mainTexture = smallFuel)
            {
                cameraMovement.AddFuel(25);
            }
            else if (item.GetComponent<MeshRenderer>().material.mainTexture = bigFuel)
            {
                cameraMovement.AddFuel(100);
            }
            // Make Texture null
            item.GetComponent<MeshRenderer>().material.mainTexture = null;
            Color col = item.GetComponent<MeshRenderer>().material.color;
            col.a = 0;
            item.GetComponent<MeshRenderer>().material.color = col;
            // Change text 
            itemText.text = "Purchased!"; 
            // Update Money text 
            moneyText.text = "Credits: " + money;   
        }
    }
    
}
