using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 005")]
public class Unit_005 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => base.GetUnitActiveSkill(unit);
}