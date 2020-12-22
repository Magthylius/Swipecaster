using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    protected int _remainingTurns;
    public int RemainingTurns => _remainingTurns;

    public abstract void DoPreEffect(Entity target);
    public abstract void DoEffectOnAction(Entity target);
    public abstract void DoPostEffect(Entity target);
    public virtual void DeductRemainingTurns()
    {
        if (ShouldClear()) return;

        _remainingTurns--;
    }
    public bool ShouldClear() => _remainingTurns <= 0;

    protected int Round(float number) => Mathf.RoundToInt(number);

    public StatusEffect() => _remainingTurns = 0;
    public StatusEffect(int turns) => _remainingTurns = turns;
}