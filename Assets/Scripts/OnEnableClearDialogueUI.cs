using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class OnEnableClearDialogueUI : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Image img; 
    void OnEnable()
    {
        tmp.text = ""; 
    }
}
