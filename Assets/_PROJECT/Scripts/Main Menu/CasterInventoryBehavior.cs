using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CasterInventoryBehavior : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI casterName;

    public void Init(UnitObject unit)
    {
        portrait.sprite = unit.PortraitArt;
        casterName.text = unit.CharacterName;
    }

    public void Init(Sprite portraitImg, string name)
    {
        portrait.sprite = portraitImg;
        casterName.text = name;
    }

    public void ChangePortrait(Sprite portraitImg)
    {
        portrait.sprite = portraitImg;
    }

    public void ChangeName(string name)
    {
        casterName.text = name;
    }
}
