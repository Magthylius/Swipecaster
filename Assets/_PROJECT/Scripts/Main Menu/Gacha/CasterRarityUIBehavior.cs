using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasterRarityUIBehavior : MonoBehaviour
{
    public List<GameObject> rarityStars;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideAll()
    {
        foreach (GameObject img in rarityStars) img.SetActive(false);
    }

    public void Setup(int points)
    {
        HideAll();

        for (int i = 0; i < points; i++)
        {
            rarityStars[i].SetActive(true);
        }

    }
}
