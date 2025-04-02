using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class GoalManager : MonoBehaviour
{
    public TextMeshProUGUI goalText; 
    public int goalStar = 0; 
    public int goalStarCount  = 1; 
    public int goalPlanet = 0; 
    public int goalPlanetCount = 1; 
    public int goalJunk = 0;
    public int goalJunkCount = 1; 
    // Start is called before the first frame update
    void Start()
    {
        calcGoalText(); 
    }
    public void calcGoalText(){
        string text = ""; 
        if (goalStarCount > 0){
            if (goalStar >= goalStarCount)
                text += "<s>";  
            text += "● Take Photo of " + goalStarCount + " Star\n"; 
            if (goalStar >= goalStarCount)
                text += "</s>";  
        }
        if (goalPlanetCount > 0){
            if (goalPlanet >= goalPlanetCount)
                text += "<s>";  
            text += "● Take Photo of " + goalPlanetCount + " Planet\n"; 
            if (goalPlanet >= goalPlanetCount)
                text += "</s>";  
        }
        if (goalJunkCount > 0){
            if (goalJunk >= goalJunkCount)
                text += "<s>";  
            text += "● Hook " + goalJunkCount + " Junk\n"; 
            if (goalJunk >= goalJunkCount)
                text += "</s>";  
        }
        goalText.text = text; 
    }
}
