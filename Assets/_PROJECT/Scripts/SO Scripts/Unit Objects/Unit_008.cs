using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 008")]
public class Unit_008 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.Wickfire(unit);
}
