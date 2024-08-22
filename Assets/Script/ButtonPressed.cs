using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPressed : MonoBehaviour
{
    //number of times button is pressed
    public static int n;
    public void button_pressed()
    {
        Debug.Log("Button Pressed " + n);
        n++;
        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();
        Debug.Log(txt.text);
        if (txt.text == "Hide Pointer")
        {
            txt.text = "Show Pointer";
        }
        else
        {
            txt.text = "Hide Pointer";
        }
    }
}
