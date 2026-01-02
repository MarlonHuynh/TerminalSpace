using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoalManager : MonoBehaviour
{
    [Header("Complete")]
    public LevelLoader levelLoader; 
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
        string toptext = "";
        string bottext = ""; 
        
        // If the level was previously completed
        if (levelLoader.levelCompletions[levelLoader.currentLevel, 1] == "OldComplete")
        {
            toptext = "System already logged.\n";
        }
        // The starting "inherent" level 
        else if (starGoal == 0 && planetGoal == 0 && junkGoal == 0)
        {
            toptext = "Use Command Terminal to navigate to new system.\n";
        }
        // If the player newly completes an Incomplete level 
        else if ((levelLoader.levelCompletions[levelLoader.currentLevel, 1] == "Incomplete")
        && (currentStarCount >= starGoal && currentPlanetCount >= planetGoal && currentJunkCount >= junkGoal))
        {
            levelLoader.levelCompletions[levelLoader.currentLevel, 1] = "NewComplete";
            toptext = "System completed. Please use Command Terminal to log.\n";
        }
        else
        {
            if (starGoal > 0)
            {
                if (currentStarCount >= starGoal)
                    bottext += "<s>";
                bottext += "● Take Photo of " + starGoal + " Star\n";
                if (currentStarCount >= starGoal)
                    bottext += "</s>";
            }
            if (planetGoal > 0)
            {
                if (currentPlanetCount >= planetGoal)
                    bottext += "<s>";
                bottext += "● Take Photo of " + planetGoal + " Planet\n";
                if (currentPlanetCount >= planetGoal)
                    bottext += "</s>";
            }
            if (junkGoal > 0)
            {
                if (currentJunkCount >= junkGoal)
                    bottext += "<s>";
                bottext += "● Hook " + junkGoal + " Junk\n";
                if (currentJunkCount >= junkGoal)
                    bottext += "</s>";
            }
        }
        goalText.text = toptext + bottext;
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
