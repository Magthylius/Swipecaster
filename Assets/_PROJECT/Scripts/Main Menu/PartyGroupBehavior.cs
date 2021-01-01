using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyGroupBehavior : MonoBehaviour
{
    public CasterParty party;
    public List<Image> portraitList;

    public TextMeshProUGUI partyName;
    public TextMeshProUGUI squadNumber;

    List<UnitObject> castersList;

    void Start()
    {
        UpdateImages();
        UpdatePartyName();
        UpdateCasters();
        squadNumber.text = (transform.GetSiblingIndex() + 1).ToString();
    }

    public void UpdateAll()
    {
        UpdateImages();
        UpdatePartyName();
        UpdateCasters();
    }

    public void UpdateCasters()
    {
        castersList = new List<UnitObject>();
        castersList = party.activeUnits;
    }
    
    public void UpdateImages()
    {
        for (int i = 0; i < party.activeUnits.Count; i++)
        {
            portraitList[i].sprite = party.activeUnits[i].PortraitArt;
        }
    }

    public void UpdatePartyName()
    {
        partyName.text = party.partyName;
    }

    public void SetParty(CasterParty casterParty)
    {
        party = casterParty;
    }
}
