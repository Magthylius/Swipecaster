using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 004")]
public class Unit_004 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.MorningDew(unit);
}
