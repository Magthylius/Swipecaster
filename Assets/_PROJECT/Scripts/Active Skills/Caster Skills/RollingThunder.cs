using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RollingThunder : CasterSkill
{
    [SerializeField] private int effectTurns = 1;
    [SerializeField] private int stackCount = 1;
    [SerializeField] private float attackPercent = 0.6f;
    private StatusEffect StatusToApply => Create.A_Status.Vulnerability(effectTurns);

    public override string Description
        => $"Drops bombs ({RoundToPercent(attackPercent)}% ATK) on ALL enemy positions. " +
           $"All affected enemies suffer {stackCount} stack(s) and {effectTurns} turns VULNERABLE status.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        int damage = CalculateDamage();
        var allFoes = GetAllFoeAlignedUnits(battleStage);
        HandleFoes(damage, allFoes);
        ResetSkillCharge();
    }

    private int CalculateDamage() => Round(GetUnit.CalculateDamage(TargetInfo.Null, RuneCollection.Null) * attackPercent);
    private static List<Unit> GetAllFoeAlignedUnits(BattlestageManager battleStage)
    {
        var allFoes = battleStage.GetEnemyTeamAsUnit();
        var allFoeEntities = battleStage.GetEnemyEntitiesAsUnit().Where(e => !e.GetIsPlayer).ToList();
        allFoes = allFoes.Join(allFoeEntities);
        return allFoes;
    }
    private void HandleFoes(int damage, List<Unit> allFoes)
    {
        allFoes.ForEach(foe => foe.TakeHit(GetUnit, damage));
        for (int i = 0; i < stackCount; i++) allFoes.ForEach(foe => foe.AddStatusEffect(StatusToApply));
    }

    public RollingThunder(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 4;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }

    public RollingThunder(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}