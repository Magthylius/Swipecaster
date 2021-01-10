using UnityEngine;

[CreateAssetMenu(menuName = "Unit/ID/Unit Object 011")]
public class Unit_011 : UnitObject
{
    public override ActiveSkill GetUnitActiveSkill(Unit unit) => Create.A_Skill.WaveringMist(unit);
}
