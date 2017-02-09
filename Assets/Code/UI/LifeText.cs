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
    private Outline outline;

    public Color Green;
    public Color Yellow;
    public Color Red;
        
    private void UpdateUI()
    {
        if (text == null )
        {
            text = GetComponent<Text>();
            outline = GetComponent<Outline>();
        }

        if (Lifes > 5)
            outline.effectColor = Green;
        else if (Lifes > 3)
            outline.effectColor = Yellow;
        else
            outline.effectColor = Red;

        text.text = Lifes.ToString();
    }
}
