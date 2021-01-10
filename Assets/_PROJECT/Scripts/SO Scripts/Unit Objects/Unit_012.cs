using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 012")]
public class Unit_012 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.WitheringMark(unit);
}
