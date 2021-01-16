using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLetterboxFades : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public bool inverted = false;

    RectTransform self;
    float fadeHeight;

    void Start()
    {
        self = GetComponent<RectTransform>();

        fadeHeight = Screen.height - canvasScaler.referenceResolution.y;

        print(fadeHeight);
        print(self.offsetMax);
        print(self.offsetMin);

        self.sizeDelta = new Vector2(self.sizeDelta.x, fadeHeight * 0.5f);

        if (inverted)
        {
            self.offsetMin -= new Vector2(0, fadeHeight * 0.5f);
            self.offsetMax -= new Vector2(0, fadeHeight * 0.5f);
        }
    }
    
    void Update()
    {
        
    }
}
