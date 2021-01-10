using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 015")]
public class Unit_015 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.SandCorrosion(unit);
}
