using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalManager : MonoBehaviour
{
    [Header("Complete")]
    public bool completeSystem = false;
    [Header("Stats")]
    public TextMeshProUGUI goalText;
    public int currentStarCount = 0;
    public int starGoal = 1;
    public int currentPlanetCount = 0;
    public int planetGoal = 1;
    public int currentJunkCount = 0;
    public int junkGoal = 1;
    public void resetGoals()
    {
        starGoal = 0;
        planetGoal = 0;
        junkGoal = 0;
        calcGoalText(); 
    }
    public void setStarGoal(int i)
    {
        starGoal = i;
        calcGoalText(); 
    }
    public void setPlanetGoal(int i)
    {
        planetGoal = i;
        calcGoalText(); 
    }
    public void setJunkGoal(int i)
    {
        junkGoal = i;
        calcGoalText(); 
    }
    public void setGoals(int junk, int star, int planet)
    {
        junkGoal = junk;
        starGoal = star;
        planetGoal = planet;
        calcGoalText(); 
    }
    public int getStarGoal()
    {
        return starGoal; 
    }
    public int getJunkGoal()
    {
        return junkGoal; 
    }
    public int getPlanetGoal()
    {
        return planetGoal; 
    }
    public void addStarCount()
    {
        currentStarCount++;
    }
    public void addPlanetCount()
    {
        currentPlanetCount++;
    }
    public void addJunkCount()
    {
        currentJunkCount++;
    }
    //-----------------------
    void Start()
    {
        calcGoalText();
    }
    public void calcGoalText()
    {
        string text = "";
        if (starGoal > 0)
        {
            if (currentStarCount >= starGoal)
                text += "<s>";
            text += "● Take Photo of " + starGoal + " Star\n";
            if (currentStarCount >= starGoal)
                text += "</s>";
        }
        if (planetGoal > 0)
        {
            if (currentPlanetCount >= planetGoal)
                text += "<s>";
            text += "● Take Photo of " + planetGoal + " Planet\n";
            if (currentPlanetCount >= planetGoal)
                text += "</s>";
        }
        if (junkGoal > 0)
        {
            if (currentJunkCount >= junkGoal)
                text += "<s>";
            text += "● Hook " + junkGoal + " Junk\n";
            if (currentJunkCount >= junkGoal)
                text += "</s>";
        }
        if (starGoal == 0 && planetGoal == 0 && junkGoal == 0)
        {
            text += "● Navigate to a new system to update goals.\n";
        }
        goalText.text = text;
    }

    public bool isJunkComplete()
    {
        if (currentJunkCount >= junkGoal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isBodiesComplete()
    {
        if (currentStarCount >= starGoal && currentPlanetCount >= planetGoal)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
}
