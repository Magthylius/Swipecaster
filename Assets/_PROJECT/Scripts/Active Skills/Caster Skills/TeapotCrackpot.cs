using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeapotCrackpot : CasterSkill
{
    [SerializeField] private int _effectTurns = 3;
    [SerializeField] private float _damageToHealPercent = 0.1f;
    [SerializeField] private float _reboundPercent = 0.15f;

    public override string Description
        => $"Heals team with {RoundToPercent(_damageToHealPercent)}% of current ATK, "
         + $"adds deflection by {RoundToPercent(_reboundPercent)}%.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(Create.A_Status.AttackToPartyHeal(_effectTurns, _damageToHealPercent));
        GetUnit.AddStatusEffect(Create.A_Status.ReboundDamageUp(_effectTurns, _reboundPercent));
        ResetChargeAndEffectDuration();
    }

    public TeapotCrackpot(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _unit = unit;
        EffectDuration0();
    }
    public TeapotCrackpot(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
