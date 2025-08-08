using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float money = 50;  
    public CameraMovement cameraMovement;  
    [Header("Textures")]
    public Texture smallFuel, bigFuel, hookUpgrade, flowers;  
    [Header("World Items")] 
    public GameObject item1Texture;
    public GameObject item2Texture; 
    public GameObject item3Texture;
    private GameObject[] itemTextures = new GameObject[3]; 
    public TextMeshProUGUI itemPopup; 
    public TextMeshProUGUI creditPopup;
    private string[,] shopInventoryInfo = {
        {"50","Small Fuel Container\nCost: 50c\n Fills your ship's fuel by a small amount."},
        {"100","Large Fuel Container\nCost: 100c\n Fills your ship's fuel by a large amount."},
        {"300","Hook Upgrade\nCost: 300c\nLess damage to junk means you can sell at a better rate."},
    }; 
    private int currentItemNum;

    void Start()
    {
        item1Texture.GetComponent<MeshRenderer>().material.mainTexture = smallFuel;
        item2Texture.GetComponent<MeshRenderer>().material.mainTexture = bigFuel;
        item3Texture.GetComponent<MeshRenderer>().material.mainTexture = hookUpgrade;
        itemTextures = new GameObject[] { item1Texture, item2Texture, item3Texture };
        creditPopup.text = money + "C"; 
    }   
    public void takeCurrentItem()
    {
        if (int.Parse(shopInventoryInfo[currentItemNum, 0]) <= money)
        {
            money -= int.Parse(shopInventoryInfo[currentItemNum, 0]);
            // Apply effect of item
            if (itemTextures[currentItemNum].GetComponent<MeshRenderer>().material.mainTexture = smallFuel)
            {
                cameraMovement.AddFuel(25);
            }
            else if (itemTextures[currentItemNum].GetComponent<MeshRenderer>().material.mainTexture = bigFuel)
            {
                cameraMovement.AddFuel(100);
            }
            // Make Texture null
            itemTextures[currentItemNum].GetComponent<MeshRenderer>().material.mainTexture = null;
            Color col = itemTextures[currentItemNum].GetComponent<MeshRenderer>().material.color;
            col.a = 0;
            itemTextures[currentItemNum].GetComponent<MeshRenderer>().material.color = col;
            // Update Text
            creditPopup.text = money + "C"; 
        }
    }

    public void changeCurrentItem(int num)
    {
        currentItemNum = num;
        itemPopup.text = shopInventoryInfo[currentItemNum, 1]; 
    }
    
}
