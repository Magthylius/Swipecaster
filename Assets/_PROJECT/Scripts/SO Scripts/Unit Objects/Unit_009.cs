using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 009")]
public class Unit_009 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.SpontaneousFire(unit);
}
