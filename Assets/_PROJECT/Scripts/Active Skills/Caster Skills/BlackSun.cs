using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BlackSun : CasterSkill
{
    [SerializeField] private float damageMultiplier = 5.0f;

    public override string Description
        => $"Deals {RoundToPercent(damageMultiplier)}% DMG to all enemies overhead.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        int damage = Round(GetUnit.CalculateDamage(TargetInfo.Null, RuneCollection.Null) * damageMultiplier);
        var allFoes = GetAllFoeAlignedUnits(battleStage);
        allFoes.ForEach(foe => foe.TakeHit(GetUnit, damage));
        ResetChargeAndEffectDuration();
    }

    private static List<Unit> GetAllFoeAlignedUnits(BattlestageManager battleStage)
    {
        var allFoes = battleStage.GetEnemyTeamAsUnit();
        var allFoeEntities = battleStage.GetEnemyEntitiesAsUnit().Where(e => !e.GetIsPlayer).ToList();
        allFoes = allFoes.Join(allFoeEntities);
        return allFoes;
    }

    public BlackSun(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public BlackSun(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}