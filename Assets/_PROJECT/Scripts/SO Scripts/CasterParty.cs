using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Menu/Caster Party")]
public class CasterParty : ScriptableObject
{
    [Header("General")]
    public List<UnitObject> activeUnits;
}
