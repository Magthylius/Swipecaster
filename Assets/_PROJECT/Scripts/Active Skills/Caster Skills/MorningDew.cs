using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MorningDew : CasterSkill
{
    [SerializeField] private int _effectTurns = 1;
    [SerializeField] private float _damageToHealPercent = 0.5f;

    public override string Description
        => $"Heals team with {RoundToPercent(_damageToHealPercent)}% of current DMG Dealt.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Foes == null || targetInfo.Foes.Count == 0) return;

        _unit.AddStatusEffect(Create.A_Status.AttackToPartyHeal(_effectTurns, _damageToHealPercent));
        ResetSkillCharge();
    }

    public MorningDew(Unit unit)
    {
        _skillDamageMultiplier = 1.0f;
        _startEffectDuration = 1;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = false;
        _unit = unit;
        EffectDuration0();
    }

    public MorningDew(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
