using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 014")]
public class Unit_014 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.GeminiPetrification(unit);
}
