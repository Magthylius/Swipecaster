using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 007")]
public class Unit_007 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => base.GetUnitActiveSkill(unit);
}
