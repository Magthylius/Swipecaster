using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatDrumEffect : StatusTemplate<BeatDrumEffect>
{
    #region Variables and Properties

    private BattlestageManager _battleStage;
    private float _parentAtkPercent;
    private Unit _parentUnit;
    public float ParentAttackPercent => _parentAtkPercent;
    public Unit GetParentUnit => _parentUnit;
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
        int damage = CalculateDamage();
        HandleTargets(enemyPos, enemyEntityPos, damage);
    }

    private int CalculateDamage()
    {
        GetUnit.GetBaseSummon.MaxAttack = Round(GetParentUnit.GetCurrentAttack * ParentAttackPercent);
        GetUnit.UpdateStatusEffects();
        return GetUnit.CalculateDamage(TargetInfo.Null, RuneCollection.Null);
    }

    private void HandleTargets(List<Transform> enemyPos, List<Transform> enemyEntityPos, int damage)
    {
        int selfIndex = GetUnitsFromTransformChild0(enemyEntityPos).IndexOf(GetUnit);
        List<Unit> targets = GetAdjacentUnits(selfIndex, enemyPos);
        targets.ForEach(unit => unit.TakeHit(GetUnit, damage));
        targets.ForEach(unit => unit.AddStatusEffect(StatusToApply));
    }

    private static List<Unit> GetAdjacentUnits(int entityIndex, List<Transform> unitObjects)
    {
        var list = new List<Unit>();
        var units = GetUnitsFromTransformChild0(unitObjects);
        if (WithinRange(entityIndex - 1, units)) list.Add(units.ElementAtOrDefault(entityIndex - 1));
        if (WithinRange(entityIndex, units)) list.Add(units.ElementAtOrDefault(entityIndex));
        return list.Where(unit => UnitFound(unit)).ToList();
    }

    #endregion

    public BeatDrumEffect() : base()
    {
        _parentUnit = null;
    }
    public BeatDrumEffect(int turns, float baseResistance, bool isPermanent, float parentAtkPercent, Unit parentUnit, BattlestageManager battleStage) : base(turns, baseResistance, isPermanent)
    {
        _parentAtkPercent = Mathf.Abs(parentAtkPercent);
        _parentUnit = parentUnit;
        _battleStage = battleStage;
    }
}