using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GachaBanner : ScriptableObject
{
    [Header("Information")]
    public string BannerName;
    [TextArea] public string BannerDescription;

    [Header("Pull Settings")]
    public float oneStarWeight;
    public float twoStarWeight;
    public float threeStarWeight;
    public float fourStarWeight;
    public float fiveStarWeight;

    [Header("Pull Statistics")]
    [ReadOnly] float oneStarChance;
    [ReadOnly] float twoStarChance;
    [ReadOnly] float threeStarChance;
    [ReadOnly] float fourStarChance;
    [ReadOnly] float fiveStarChance;

    public void UpdateChances()
    {
        float totalWeight = oneStarWeight + twoStarWeight + threeStarWeight + fourStarWeight + fiveStarWeight;
        oneStarChance = oneStarWeight / totalWeight;
        twoStarChance = twoStarWeight / totalWeight;
        threeStarChance = threeStarWeight / totalWeight;
        fourStarChance = fourStarWeight / totalWeight;
        fiveStarChance = fiveStarWeight / totalWeight;
    }
}
