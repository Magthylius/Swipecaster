using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wickfire : CasterSkill
{
    [SerializeField] private int effectTurns = 5;
    [SerializeField] private float damageMultiplier = 0.2f;

    public override string Description
        => $"Changes damage to distributed spread, +{RoundToPercent(damageMultiplier)}% damage.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Allies == null || targetInfo.Allies.Count == 0) return;

        GetUnit.AddStatusEffect(Create.A_Status.ProjectileLocker(effectTurns, new Blast()));
        GetUnit.AddStatusEffect(Create.A_Status.AttackUp(effectTurns, damageMultiplier));
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
