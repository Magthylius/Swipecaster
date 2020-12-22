using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class StatusEffect
{
    protected string _statusName;
    protected int _remainingTurns;
    protected float _baseResistanceProbability;
    protected bool _permanent;

    #region Property Methods

    public virtual string StatusName => _statusName;
    public int RemainingTurns => _remainingTurns;
    public float BaseResistanceProbability => _baseResistanceProbability;
    public bool EffectIsPermanent => _permanent;

    #endregion

    #region Abstract Methods

    public abstract void DoPreEffect(Entity target);
    public abstract void DoEffectOnAction(Entity target);
    public abstract void DoPostEffect(Entity target);
    protected abstract int GetCountOfType(List<StatusEffect> statusList);

    #endregion

    #region Virtual Methods

    public virtual float GetStatusDamageModifier() => 0.0f;
    public virtual void DeductRemainingTurns()
    {
        if (ShouldClear() || EffectIsPermanent) return;

        _remainingTurns--;
    }

    #endregion

    #region Public Methods

    public bool ShouldClear() => _remainingTurns <= 0 && !EffectIsPermanent;
    public bool ProbabilityHit(List<StatusEffect> statuses) => Random.Range(0.0f, 1.0f) < CalculateResistance(GetCountOfType(statuses));

    #endregion

    #region Protected Methods

    protected int Round(float number) => Mathf.RoundToInt(number);

    #endregion

    #region Private Methods

    private float CalculateResistance(int effectCount) => Mathf.Clamp01(_baseResistanceProbability * effectCount);

    #endregion

    public StatusEffect()
    {
        _remainingTurns = 0;
        _statusName = string.Empty;
        _baseResistanceProbability = 0.0f;
        _permanent = false;
    }
    public StatusEffect(int turns, float baseResistance, bool isPermanent)
    {
        _remainingTurns = turns;
        _baseResistanceProbability = baseResistance;
        _permanent = isPermanent;
    }
}