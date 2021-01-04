using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CasterSkill : ActiveSkill
{
    public CasterSkill() : base() { }
    public CasterSkill(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
    {
        _startEffectDuration = startEffectDuration;
        _maxSkillCharge = maxSkillCharge;
        _chargeGainPerTurn = 1;
        _ignoreDuration = ignoreDuration;
        _unit = unit;
        EffectDuration0();
    }

    public override void TurnStartCall() { }
    protected override void OnEffectDurationComplete() { }
}
