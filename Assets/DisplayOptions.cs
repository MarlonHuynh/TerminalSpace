using UnityEngine;

public class DisplayOptions : MonoBehaviour
{
    /*
    void displayOptionsText()
    { 
        string temp = "";
        for (int i = 0; i < optionItems.Length; i++)
        {
            if (i == optionsCount)
            {
                temp += "> ";
            }
            temp += optionItems[i];
            if (i == 0)
            {
                temp += volume + "/100";
            }
            temp += "\n";
        }
        pauseText.text = temp;
    }
    
    public void SetMasterVolume(float volume) // Vol from 0f to 1f 
    {
        // volume is expected in linear 0.0 to 1.0 range
        // Convert to decibel (logarithmic) scale:
        float dB;
        if (volume > 0)
            dB = Mathf.Log10(volume) * 20f;
        else
            dB = -80f; // silent

        masterMixer.SetFloat("MasterVolume", dB);
    }*/
}
