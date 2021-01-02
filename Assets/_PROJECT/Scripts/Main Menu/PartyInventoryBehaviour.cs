using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyInventoryBehaviour : MonoBehaviour
{
    PartyConfigurationBehavior partyConfigurationBehavior;
    
    public Image portrait;
    public TextMeshProUGUI casterName;
    string id;

    UnitObject assignedUnit;

    void Start()
    {
        partyConfigurationBehavior = PartyConfigurationBehavior.instance;
    }

    public void Init(UnitObject unit)
    {
        portrait.sprite = unit.PortraitArt;
        casterName.text = unit.CharacterName;
        id = unit.ID;
        assignedUnit = unit;
    }

    public void SelfAdd()
    {
        partyConfigurationBehavior.Add(assignedUnit);
    }

    public void SelfRemove(Button button)
    {
        
        if (!button.IsInteractable())
        {
            partyConfigurationBehavior.RemoveDirectly(assignedUnit);
            button.interactable = true;
        }
        else if (button.IsInteractable())
        {
            if (partyConfigurationBehavior.GetCurPartyList().Count >= 4)
            {
                print("Party Full!");
                return;
            }
            button.interactable = false;
        }
    }

    #region Accessors

    public string GetId() => id;

    #endregion


}
