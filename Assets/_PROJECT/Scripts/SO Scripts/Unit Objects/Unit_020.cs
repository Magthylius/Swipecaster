using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 020")]
public class Unit_020 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.SecondScar(unit);
}