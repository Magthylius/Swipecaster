using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrypticMark : CasterSkill
{
    [SerializeField] private int effectTurns = 1;
    [SerializeField] private float damageTakenMultiplier = 1.0f;
    [SerializeField] private float defenceDownPercent = 0.5f;
    private StatusEffect StatusToRebound => Create.A_Status.DefenceDown(effectTurns, defenceDownPercent);

    public override string Description
        => $"Increase DMG taken by {RoundToPercent(damageTakenMultiplier)}%. "
         + $"Enemies that deal DMG to this caster -{RoundToPercent(defenceDownPercent)}% DEF.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(Create.A_Status.DamageTakenUp(effectTurns, damageTakenMultiplier));
        GetUnit.AddStatusEffect(Create.A_Status.ReboundingStatus(effectTurns, StatusToRebound));
        ResetChargeAndEffectDuration();
    }

    public CrypticMark(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = false;
        _unit = unit;
        EffectDuration0();
    }
    public CrypticMark(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
