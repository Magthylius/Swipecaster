using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 010")]
public class Unit_010 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => base.GetUnitActiveSkill(unit);
}
