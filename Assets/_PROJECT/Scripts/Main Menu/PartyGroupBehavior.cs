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

    void Start()
    {
        UpdateImages();

        partyName.text = party.partyName;
        squadNumber.text = (transform.GetSiblingIndex() + 1).ToString();
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
