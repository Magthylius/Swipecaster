using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 003")]
public class Unit_003 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => base.GetUnitActiveSkill(unit);
}
