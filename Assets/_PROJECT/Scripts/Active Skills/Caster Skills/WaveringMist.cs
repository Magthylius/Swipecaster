using System.Linq;
using UnityEngine;

[System.Serializable]
public class WaveringMist : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    [SerializeField] private float percentUp = 0.1f;
    private StatusEffect StatusToApply => Create.A_Status.ReboundRateUpKinectist(effectTurns, percentUp);

    public override string Description
        => $"Increases all Kinectist passive skill chances by {RoundToPercent(percentUp)}%.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        targetInfo.Allies.
            Where(ally => IsKinectist(ally)).ToList().
            ForEach(ally => ally.AddStatusEffect(StatusToApply));
        ResetChargeAndEffectDuration();
    }

    private bool IsKinectist(Unit unit) => unit.GetBaseUnit.ArchTypeMajor == ArchTypeMajor.Kinectist;

    public WaveringMist(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public WaveringMist(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}