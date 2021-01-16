using UnityEngine;

[System.Serializable]
public class SpontaneousFire : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    private StatusEffect StatusToApply => Create.A_Status.Aflame(effectTurns);

    public override string Description
        => $"All enemies gain a {effectTurns} turn AFLAME status.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Foes.Count == 0) return;
        targetInfo.Foes.ForEach(foe => foe.AddStatusEffect(StatusToApply));
        ResetChargeAndEffectDuration();
    }

    public SpontaneousFire(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = false;
        _unit = unit;
        EffectDuration0();
    }
    public SpontaneousFire(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
