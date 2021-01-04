using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeBurst : CasterSkill
{
    public override string Description
        => "Deals +20% damage and converts passed attacks to splash.";

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
        => new TargetInfo(focusTarget, null, null, allCasters, allFoes);

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Casters == null || targetInfo.Casters.Count == 0) return;

        GetUnit.AddStatusEffect(Create.A_Status.AttackUp(_startEffectDuration, 0.2f));
        GetUnit.SetProjectile(new Splash());
        ResetSkillCharge();
    }

    protected override void OnEffectDurationComplete()
    {
        GetUnit.SetProjectile(new CrowFlies());
    }

    public SporeBurst(Unit unit)
    {
        _startEffectDuration = 2;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _ignoreDuration = false;
        _unit = unit;
        EffectDuration0();
    }
}
