using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyGroupBehavior : MonoBehaviour
{
    public CasterParty party;
    public List<Image> portraitList;

    void Start()
    {
        UpdateImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAll()
    {
        UpdateImages();
    }

    public void UpdateImages()
    {
        for (int i = 0; i < party.activeUnits.Count; i++)
        {
            portraitList[i].sprite = party.activeUnits[i].PortraitArt;
        }
    }

    public void SetParty(CasterParty casterParty)
    {
        party = casterParty;
    }
}
