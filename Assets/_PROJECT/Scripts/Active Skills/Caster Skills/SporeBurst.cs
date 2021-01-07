using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SporeBurst : CasterSkill
{
    [SerializeField] private int _effectTurns = 2;
    [SerializeField] private float _damageMultiplier = 0.2f;

    public override string Description
        => $"Deals +{RoundToPercent(_damageMultiplier)}% damage and converts passed attacks to splash.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Allies == null || targetInfo.Allies.Count == 0) return;

        GetUnit.AddStatusEffect(Create.A_Status.AttackUp(_effectTurns, _damageMultiplier));
        GetUnit.SubscribeGrazeEvent(ConvertPassingProjectile);
        ResetSkillCharge();
    }
    protected override void OnEffectDurationComplete() => GetUnit.UnsubscribeGrazeEvent(ConvertPassingProjectile);

    private void ConvertPassingProjectile(Unit attacker, int potentialDamage) => attacker.SubstituteProjectile(new Splash());

    public SporeBurst(Unit unit)
    {
        _startEffectDuration = 2;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _ignoreDuration = false;
        _unit = unit;
        EffectDuration0();
    }
    public SporeBurst(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
