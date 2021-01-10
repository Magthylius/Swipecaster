using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class StatusEffect
{
    protected int _remainingTurns;
    protected float _baseResistanceProbability;
    protected bool _permanent;
    protected Unit _unit;
    protected Action _selfDestructEvent;

    #region Property Methods

    public virtual string StatusName => string.Empty;
    public int RemainingTurns => _remainingTurns;
    public float BaseResistanceProbability => _baseResistanceProbability;
    public bool EffectIsPermanent => _permanent;

    #endregion

    #region Abstract Methods

    public abstract void DoImmediateEffect(TargetInfo info);
    public abstract void UpdateStatus();
    public abstract void DoEffectOnAction(TargetInfo info, int totalDamage);
    public abstract void DoOnHitEffect(Unit damager, TargetInfo info, int totalDamage);
    public abstract void DoPostEffect();
    protected abstract int GetCountOfType(List<StatusEffect> statusList);
    protected abstract void Deinitialise();

    #endregion

    #region Virtual Methods

    public virtual float GetStatusDamageOutModifier() => 0.0f;
    public virtual float GetStatusDamageInModifier() => 0.0f;
    public virtual void DeductRemainingTurns()
    {
        if (ShouldClear() || EffectIsPermanent) return;
        _remainingTurns--;
    }

    #endregion

    #region Public Methods

    public static StatusEffect Null => new NullStatus();
    public static List<StatusEffect> NullList => new List<StatusEffect>(1) { Null };

    public bool ShouldClear() => _remainingTurns <= 0 && !EffectIsPermanent;
    public bool ProbabilityHit(List<StatusEffect> statuses) => Random.Range(0.0f, 1.0f - float.Epsilon) < CalculateResistance(GetCountOfType(statuses));

    public Unit GetUnit => _unit;
    public void SetUnit(Unit unit) => _unit = unit;

    public void SubscribeSelfDestructEvent(Action method) => _selfDestructEvent += method;
    public void UnsubscribeSelfDestructEvent(Action method) => _selfDestructEvent -= method;
    public void InvokeSelfDestructEvent() => _selfDestructEvent?.Invoke();

    #endregion

    #region Protected Methods

    protected int Round(float number) => Mathf.RoundToInt(number);
    protected static List<Unit> GetUnitsFromTransformChild0(List<Transform> unitTransforms)
    {
        var list = new List<Unit>();
        foreach (var u in unitTransforms)
        {
            if (u.childCount == 0)
            {
                list.Add(null);
                continue;
            }
            list.Add(u.GetChild(0).GetComponent<Unit>());
        }
        return list;
    }
    protected static bool UnitFound(Unit unit) => unit != null;
    protected static bool WithinRange<T>(int index, List<T> list) => index >= 0 && index < list.Count;

    #endregion

    #region Private Methods

    private float CalculateResistance(int effectCount) => Mathf.Clamp01(_baseResistanceProbability * effectCount);

    #endregion

    public StatusEffect()
    {
        _remainingTurns = 0;
        _baseResistanceProbability = 0.0f;
        _permanent = false;
        SetUnit(null);
        SubscribeSelfDestructEvent(Deinitialise);
    }
    public StatusEffect(int turns, float baseResistance, bool isPermanent)
    {
        _remainingTurns = turns;
        _baseResistanceProbability = baseResistance;
        _permanent = isPermanent;
        SetUnit(null);
        SubscribeSelfDestructEvent(Deinitialise);
    }
}