using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaussCaliber : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    private static List<float> BaseMultiplier => PiercingProjectile.GetDefaultDiminishingMultiplier;
    private static readonly List<float> AdditiveMultiplier = CalculateAdditiveMultiplier();
    private static readonly List<float> TargetMultiplier = new List<float>(4) { 2.0f, 1.5f, 0.7f, 0.5f };
    private static readonly Piercing PiercingProjectile = new Piercing();

    public override string Description
        => $"Changes piercing multiplier to"
         + $"[{TotalMultiplierAt(0)}x, {TotalMultiplierAt(1)}x, {TotalMultiplierAt(2)}x, {TotalMultiplierAt(3)}x].";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(Create.A_Status.PierceDamageUp(effectTurns, AdditiveMultiplier));
        ResetSkillCharge();
    }

    private static float TotalMultiplierAt(int index)
    {
        if (index < 0 || index >= 4) return 0.0f;
        return BaseMultiplier[index] + AdditiveMultiplier[index];
    }

    private static List<float> CalculateAdditiveMultiplier()
    {
        var list = new List<float>(4);
        for(int i = 0; i < 4; i++)
        {
            float difference = 0.0f;
            if (TargetMultiplier[i] > BaseMultiplier[i]) difference = TargetMultiplier[i] - BaseMultiplier[i];
            list.Add(difference);
        }
        return list;
    }

    public GaussCaliber(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = false;
        _unit = unit;
        EffectDuration0();
    }
    public GaussCaliber(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
