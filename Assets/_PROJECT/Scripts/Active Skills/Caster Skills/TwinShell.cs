using System.Linq;
using UnityEngine;

[System.Serializable]
public class TwinShell : CasterSkill
{
    [SerializeField] private int effectTurns = 1;
    [SerializeField] private float baseDamageMultiplier = 0.15f;
    [SerializeField] private float extraDamageMultiplier = 0.1f;
    [SerializeField] private string casterName = "Golem";

    private StatusEffect AttackStatus(float percent) => Create.A_Status.AttackUp(effectTurns, percent);
    private StatusEffect ProjectileStatus => Create.A_Status.ProjectileLocker(effectTurns, new Dual(GetUnit));

    public override string Description
        => $"ATK +{RoundToPercent(baseDamageMultiplier)}%. Hits an extra target when attacking. "
         + $"If {casterName} is in the team, further ATK +{RoundToPercent(extraDamageMultiplier)}%";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        var unit = GetUnitByCasterName(targetInfo);
        float totalMultiplier = CalculateTotalMultiplier(unit);
        HandleStatusEffects(totalMultiplier);
        ResetSkillCharge();
    }

    private void HandleStatusEffects(float totalMultiplier)
    {
        GetUnit.AddStatusEffect(AttackStatus(totalMultiplier));
        GetUnit.AddStatusEffect(ProjectileStatus);
    }
    private float CalculateTotalMultiplier(Unit unit) => baseDamageMultiplier + (UnitFound(unit) ? extraDamageMultiplier : 0.0f);
    private Unit GetUnitByCasterName(TargetInfo targetInfo) => targetInfo.Allies.Where(ally => MatchesCasterName(ally)).FirstOrDefault();
    private bool MatchesCasterName(Unit ally) => ally.GetBaseUnit.CharacterName == casterName;
    private static bool UnitFound(Unit unit) => unit != null;

    public TwinShell(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }

    public TwinShell(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
