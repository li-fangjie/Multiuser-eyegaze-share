using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGaze_Color : MonoBehaviour
{
    List<Color> colorList = new List<Color>()
        {
             Color.red,
             Color.green,
             Color.yellow,
             Color.magenta,
             new Color(255F, 0F, 255F),
             new Color(0F, 255F, 255F),
             new Color(255F, 255F, 0F),
             new Color(128F, 0F, 128F),
             new Color(128F, 0F, 0F)
        };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
