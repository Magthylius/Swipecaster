using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 016")]
public class Unit_016 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.DrumBeater(unit);
}
