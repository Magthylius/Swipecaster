using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Banners/Gacha Banner")]
public class GachaBanner : ScriptableObject
{
    [System.Serializable]
    public struct ModifiedPull
    {
        public UnitObject unit;
        public float weight;

        public ModifiedPull(UnitObject _unit, float _weight)
        {
            unit = _unit;
            weight = _weight;
        }
    }

    [Header("Information")]
    public string BannerName;
    [TextArea] public string BannerDescription;

    [Header("Rarity Weights")]
    public float oneStarWeight;
    public float twoStarWeight;
    public float threeStarWeight;
    public float fourStarWeight;
    public float fiveStarWeight;

    [Header("Allowed Drops")]
    public List<ModifiedPull> oneStarCasters;
    public List<ModifiedPull> twoStarCasters;
    public List<ModifiedPull> threeStarCasters;
    public List<ModifiedPull> fourStarCasters;
    public List<ModifiedPull> fiveStarCasters;

    [Header("Pull Statistics")]
    float oneStarChance;
    float twoStarChance;
    float threeStarChance;
    float fourStarChance;
    float fiveStarChance;

    string casterLocation = "ScriptableObjects/Casters";

    public void UpdateChances()
    {
        float totalWeight = oneStarWeight + twoStarWeight + threeStarWeight + fourStarWeight + fiveStarWeight;
        oneStarChance = oneStarWeight / totalWeight;
        twoStarChance = twoStarWeight / totalWeight;
        threeStarChance = threeStarWeight / totalWeight;
        fourStarChance = fourStarWeight / totalWeight;
        fiveStarChance = fiveStarWeight / totalWeight;
    }

    public string GetAllChances()
    {
        UpdateChances();
        return "1: " + oneStarChance + " | 2: " + twoStarChance + " | 3: " + threeStarChance + " | 4: " + fourStarChance + " | 5: " + fiveStarChance;
    }

    public UnitObject PullCaster()
    {
        UpdateChances();
        float rarityPullPoint = Random.Range(0f, 1f);

        float oneStarPoint = oneStarChance;
        float twoStarPoint = oneStarPoint + twoStarChance;
        float threeStarPoint = twoStarPoint + threeStarChance;
        float fourStarPoint = threeStarPoint + fourStarChance;
        float fiveStarPoint = fourStarPoint + fiveStarChance;

        List<ModifiedPull> pullList;
        if (rarityPullPoint < oneStarPoint) pullList = oneStarCasters;
        else if (rarityPullPoint < twoStarPoint) pullList = twoStarCasters;
        else if (rarityPullPoint < threeStarPoint) pullList = threeStarCasters;
        else if (rarityPullPoint < fourStarPoint) pullList = fourStarCasters;
        else pullList = fiveStarCasters;

        float totalWeight = 0f;
        foreach (ModifiedPull pull in pullList) totalWeight += pull.weight;

        float casterPullPoint = Random.Range(0f, 1f);
        float totalChance = 0f;
        UnitObject pulledUnit = null;
        for (int i = 0; i < pullList.Count; i++)
        {
            totalChance += pullList[i].weight / totalWeight;

            if (casterPullPoint <= totalChance || i >= pullList.Count)
            {
                if (i >= pullList.Count) Debug.LogError("Pull list out of index!");
                pulledUnit = pullList[i].unit;
                break;
            }
        }

        return pulledUnit;
    }
}
