using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 019")]
public class Unit_019 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.RollingThunder(unit);
}
