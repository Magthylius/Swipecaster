using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GaussCaliber : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    private static List<float> BaseMultiplier => PiercingProjectile.GetDefaultDiminishingMultiplier;
    private static List<float> AdditiveMultiplier => CalculateAdditiveMultiplier();
    private static readonly List<float> TargetMultiplier = new List<float>(4) { 2.0f, 1.5f, 0.7f, 0.5f };
    private static readonly Piercing PiercingProjectile = new Piercing();

    public override string Description
        => $"Changes piercing multiplier to "
         + $"[{TargetMAt(0)}x, {TargetMAt(1)}x, {TargetMAt(2)}x, {TargetMAt(3)}x].";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(Create.A_Status.PierceDamageUp(effectTurns, AdditiveMultiplier));
        ResetChargeAndEffectDuration();
    }

    private static List<float> CalculateAdditiveMultiplier()
    {
        var list = new List<float>(4);
        var targetM = TargetMultiplier;
        var baseM = BaseMultiplier;
        for(int i = 0; i < 4; i++)
        {
            float difference = 0.0f;
            if (targetM[i] > baseM[i]) difference = targetM[i] - baseM[i];
            list.Add(difference);
        }
        return list;
    }

    private static string TargetMAt(int i)
    {
        float result = 0.0f;
        if (i >= 0 && i < 4) result = TargetMultiplier[i];
        return OneDecimal(result);
    }

    public GaussCaliber(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = false;
        _unit = unit;
        EffectDuration0();
    }
    public GaussCaliber(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
