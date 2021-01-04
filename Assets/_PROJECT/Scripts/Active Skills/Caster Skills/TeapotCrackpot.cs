using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeapotCrackpot : CasterSkill
{
    public override string Description
        => "Heals team with 10% of current ATK, adds deflection by 15%";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {

        ResetSkillCharge();
    }

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {


        return TargetInfo.Null;
    }

    public TeapotCrackpot(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _unit = unit;
        EffectDuration0();
    }
}
