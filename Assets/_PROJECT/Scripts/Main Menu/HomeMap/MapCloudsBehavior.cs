using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCloudsBehavior : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float verticalOffsetMax;
    public float maxTransparency = 0.3f;

    RectTransform self;
    Vector2 originalOFMin;
    Vector2 originalOFMax;
    Vector2 targetOFMin;
    Vector2 targetOFMax;

    Image img;
    Color originalColor;
    Color targetColor;

    void Start()
    {
        self = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        originalOFMax = self.offsetMax;
        originalOFMin = self.offsetMin;

        targetOFMax = new Vector2(originalOFMax.x, originalOFMax.y + verticalOffsetMax);
        targetOFMin = new Vector2(originalOFMin.x, originalOFMin.y + verticalOffsetMax);

        originalColor = img.color;
        targetColor = new Color(img.color.r, img.color.g, img.color.b, maxTransparency);
    }

    public void ParallaxView()
    {
        self.offsetMax = Vector2.Lerp(originalOFMax, targetOFMax, scrollRect.verticalNormalizedPosition);
        self.offsetMin = Vector2.Lerp(originalOFMin, targetOFMin, scrollRect.verticalNormalizedPosition);

        img.color = Color.Lerp(originalColor, targetColor, scrollRect.verticalNormalizedPosition);
    }

    Vector2 center => (self.offsetMax + self.offsetMin) * 0.5f;
}
