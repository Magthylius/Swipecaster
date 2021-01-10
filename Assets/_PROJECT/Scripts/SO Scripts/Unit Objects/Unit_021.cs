using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 021")]
public class Unit_021 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.BlackSun(unit);
}