using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 017")]
public class Unit_017 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.LuringDesire(unit);
}
