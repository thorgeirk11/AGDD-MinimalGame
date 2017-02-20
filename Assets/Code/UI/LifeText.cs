using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeText : MonoBehaviour
{
    public int Lifes
    {
        get { return lifes; }
        set
        {
            lifes = value;
            UpdateUI();
        }
    }

    private int lifes;
    private Text text;
    
        
    private void UpdateUI()
    {
        if (text == null )
        {
            text = GetComponent<Text>();
        }

        text.text = Lifes.ToString();
    }
}
