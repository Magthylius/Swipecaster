using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyConfigurationBehavior : MonoBehaviour
{
    CasterParty configurationParty;

    public UIFlexibleGrid grid;
    public List<Image> portraitList;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetConfigParty(CasterParty party)
    {
        configurationParty = party;
        grid.CalculateLayoutInputHorizontal();
    }

    public void UpdatePortraits()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < configurationParty.activeUnits.Count) portraitList[i].sprite = configurationParty.activeUnits[i].PortraitArt;
            else portraitList[i].sprite = null;
        }
    }
}
