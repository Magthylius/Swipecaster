using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CasterInventoryBehavior : MonoBehaviour
{
    InventoryManager inventoryManager;

    public Image portrait;
    public TextMeshProUGUI casterName;

    UnitObject assignedUnit;

    public void Init(UnitObject unit, InventoryManager invManager)
    {
        portrait.sprite = unit.PortraitArt;
        casterName.text = unit.CharacterName;
        assignedUnit = unit;

        inventoryManager = invManager;
    }

    public void ChangePortrait(Sprite portraitImg)
    {
        portrait.sprite = portraitImg;
    }

    public void ChangeName(string name)
    {
        casterName.text = name;
    }

    public void BTN_Focus()
    {
        inventoryManager.ActivateFocus(assignedUnit);
    }
}
