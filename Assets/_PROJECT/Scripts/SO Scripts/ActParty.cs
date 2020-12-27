using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Menu/Active Party")]
public class ActParty : ScriptableObject
{
    [Header("General")]
    public List<MenuUnit> activeUnits;
}
