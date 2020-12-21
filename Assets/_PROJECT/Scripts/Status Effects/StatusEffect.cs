using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    private int _remainingTurns;
    public int RemainingTurns => _remainingTurns;

    public abstract void DoImmediateEffect(Entity target);
    public abstract void DoPostEffect(Entity target);
    public virtual void DeductRemainingTurns()
    {
        if (_remainingTurns <= 0) return;

        _remainingTurns--;
    }
    public bool ShouldClear() => _remainingTurns <= 0;

    protected int Round(float number) => Mathf.RoundToInt(number);

    public StatusEffect(int turns) => _remainingTurns = turns;
}