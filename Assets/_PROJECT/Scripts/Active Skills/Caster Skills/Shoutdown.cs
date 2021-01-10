using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shoutdown : CasterSkill
{
    [SerializeField] private int _effectTurns = 1;
    [SerializeField] private float _damageMultiplier = 0.25f;

    public override string Description 
        => $"Increases all caster DMG by {RoundToPercent(_damageMultiplier)}%";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Allies == null || targetInfo.Allies.Count == 0) return;

        targetInfo.Allies.ForEach(caster => caster.AddStatusEffect(Create.A_Status.AttackUp(_effectTurns, _damageMultiplier)));
        ResetSkillCharge();
    }

    public Shoutdown(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = false;
        _unit = unit;
        EffectDuration0();
    }
    public Shoutdown(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
