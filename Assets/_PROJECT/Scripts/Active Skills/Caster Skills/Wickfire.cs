using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wickfire : CasterSkill
{
    public override string Description
        => "Increases all caster DMG by 25%";

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
        => new TargetInfo(focusTarget, null, null, allCasters, allFoes);

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Allies == null || targetInfo.Allies.Count == 0) return;

        targetInfo.Allies.ForEach(caster => caster.AddStatusEffect(Create.A_Status.AttackUp(_startEffectDuration, 0.25f)));
        ResetSkillCharge();
    }

    public Wickfire(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _ignoreDuration = false;
        _unit = unit;
        EffectDuration0();
    }
    public Wickfire(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
