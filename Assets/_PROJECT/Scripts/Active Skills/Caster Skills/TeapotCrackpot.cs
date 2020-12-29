using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeapotCrackpot : CasterSkill
{
    public override string Description
        => "Heals team with 10% of current ATK, adds deflection by 15%";

    public override void TriggerSkill(TargetInfo targetInfo, StageInfo stageInfo)
    {
        
    }

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {


        return TargetInfo.Null;
    }

    public TeapotCrackpot() : base() { }
    public TeapotCrackpot(float damageMultiplier, int effectDuration, int maxSkillCharge, int chargeGainPerTurn, Unit unit)
         : base(damageMultiplier, effectDuration, maxSkillCharge, chargeGainPerTurn, unit)
    { }
}
