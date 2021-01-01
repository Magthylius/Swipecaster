using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyInventoryBehaviour : MonoBehaviour
{


    public Image portrait;
    public TextMeshProUGUI casterName;
    string id;

    UnitObject assignedUnit;

    public void Init(UnitObject unit)
    {
        portrait.sprite = unit.PortraitArt;
        casterName.text = unit.CharacterName;
        id = unit.ID;
        assignedUnit = unit;
    }

    public string GetId() => id;

}
