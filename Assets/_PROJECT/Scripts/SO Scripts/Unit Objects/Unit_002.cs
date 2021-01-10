using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 002")]
public class Unit_002 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.Shoutdown(unit);
}
