using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningDew : CasterSkill
{
    public override string Description
        => "Heals team with 50% of current DMG Dealt.";

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
        => new TargetInfo(focusTarget, null, null, allCasters, allFoes);

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Foes == null || targetInfo.Foes.Count == 0) return;

        ResetSkillCharge();
    }

    public MorningDew(Unit unit)
    {
        _skillDamageMultiplier = 1.0f;
        _startEffectDuration = 0;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }
}
