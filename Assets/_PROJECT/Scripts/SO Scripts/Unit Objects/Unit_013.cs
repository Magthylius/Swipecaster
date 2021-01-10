using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 013")]
public class Unit_013 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.TwinShell(unit);
}
