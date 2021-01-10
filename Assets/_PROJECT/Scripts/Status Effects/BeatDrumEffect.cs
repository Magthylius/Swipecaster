using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatDrumEffect : StatusTemplate<BeatDrumEffect>
{
    #region Variables and Properties

    private BattlestageManager _battleStage;
    public override string StatusName => "Beat Drum Effect";
    private int effectTurns = 2;
    private StatusEffect StatusToApply => Create.A_Status.Weakness(effectTurns);

    #endregion

    #region Override Methods

    public override void DoImmediateEffect(TargetInfo info) => Unit.SubscribeAllTurnEndEvent(DoActionAndApplyEffects);
    protected override void Deinitialise()
    {
        Unit.UnsubscribeAllTurnEndEvent(DoActionAndApplyEffects);
        GetUnit.Suicide();
        base.Deinitialise();
    }

    #endregion

    #region Private Methods

    private void DoActionAndApplyEffects()
    {
        var enemyPos = _battleStage.enemyPositions.ToList();
        var enemyEntityPos = _battleStage.enemyEntityPositions.ToList();
        int damage = GetUnit.CalculateDamage(TargetInfo.Null, RuneCollection.Null);
        HandleTargets(enemyPos, enemyEntityPos, damage);
    }

    private void HandleTargets(List<Transform> enemyPositions, List<Transform> enemyEntityPositions, int damage)
    {
        int entityIndex = GetUnitsFromTransformChild0(enemyEntityPositions).IndexOf(GetUnit);
        List<Unit> targets = GetUnitsAdjacentToEntity(entityIndex, enemyPositions);
        targets.ForEach(unit => unit.TakeHit(GetUnit, damage));
        targets.ForEach(unit => unit.AddStatusEffect(StatusToApply));
    }
    private static List<Unit> GetUnitsAdjacentToEntity(int entityIndex, List<Transform> unitObjects)
    {
        var list = new List<Unit>();
        var units = GetUnitsFromTransformChild0(unitObjects);
        if (WithinRange(entityIndex - 1, units)) list.Add(units.ElementAtOrDefault(entityIndex - 1));
        if (WithinRange(entityIndex, units)) list.Add(units.ElementAtOrDefault(entityIndex));
        return list.Where(unit => UnitFound(unit)).ToList();
    }

    #endregion

    public BeatDrumEffect() : base() => _battleStage = null;
    public BeatDrumEffect(int turns, float baseResistance, bool isPermanent, BattlestageManager battleStage) : base(turns, baseResistance, isPermanent)
    {
        _battleStage = battleStage;
    }
}