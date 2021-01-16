using System.Linq;
using UnityEngine;

[System.Serializable]
public class GeminiPetrification : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    [SerializeField] private int priorityUpAmount = 3;
    [SerializeField] private float atkDownMultiplier = 0.6f;
    [SerializeField] private float baseDefenceMultiplier = 0.6f;
    [SerializeField] private float extraDefenceMultiplier = 0.3f;
    [SerializeField] private string casterName = "Gargoyle";

    private StatusEffect AttackDownStatus => Create.A_Status.AttackDown(effectTurns, atkDownMultiplier);
    private StatusEffect BaseDefenceUpStatus => Create.A_Status.DefenceUp(effectTurns, baseDefenceMultiplier);
    private StatusEffect ExtraDefenceUpStatus => Create.A_Status.DefenceUp(effectTurns, extraDefenceMultiplier);
    private StatusEffect PriorityUpStatus => Create.A_Status.PriorityUp(effectTurns, priorityUpAmount);

    public override string Description
        => $"ATK -{RoundToPercent(atkDownMultiplier)}%. DEF +{RoundToPercent(baseDefenceMultiplier)}%. "
         + $"Priority +{priorityUpAmount}. "
         + $"If {casterName} is in the team, {casterName}'s DEF +{RoundToPercent(extraDefenceMultiplier)}%";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        var unit = GetUnitByCasterName(targetInfo);
        HandleSelfStatusEffects();
        HandleOtherStatusEffects(unit);
        ResetChargeAndEffectDuration();
    }

    private void HandleOtherStatusEffects(Unit unit)
    {
        if (UnitFound(unit)) unit.AddStatusEffect(ExtraDefenceUpStatus);
    }

    private void HandleSelfStatusEffects()
    {
        GetUnit.AddStatusEffect(AttackDownStatus);
        GetUnit.AddStatusEffect(BaseDefenceUpStatus);
        GetUnit.AddStatusEffect(PriorityUpStatus);
    }

    private Unit GetUnitByCasterName(TargetInfo targetInfo) => targetInfo.Allies.Where(ally => MatchesCasterName(ally)).FirstOrDefault();
    private bool MatchesCasterName(Unit ally) => ally.GetEntityName == casterName;

    public GeminiPetrification(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 4;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public GeminiPetrification(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}