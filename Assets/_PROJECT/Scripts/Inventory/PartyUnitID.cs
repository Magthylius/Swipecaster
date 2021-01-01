using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PartyUnitID : MonoBehaviour
{
    string id;

    public string GetId() => id;

    public void SetID(UnitObject unit) => id = unit.ID;

}
