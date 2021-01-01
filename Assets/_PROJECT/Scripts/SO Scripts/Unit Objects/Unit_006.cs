using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 006")]
public class Unit_006 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => new MothLamp(1.0f, 0, 4, 1, unit);
}