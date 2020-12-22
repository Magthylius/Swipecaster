using System;
using UnityEngine;

public class ScaledPoisonStatus : StatusEffect
{
    private float _maxHealthPercent;
    public float MaxHealthPercent => Mathf.Abs(_maxHealthPercent);

    public override void DoImmediateEffect(Entity target) { }
    public override void DoPostEffect(Entity target)
    {
        if (ShouldClear()) return;
        int damage = -Mathf.Abs(Round(target.GetCurrentHealth * MaxHealthPercent));
        target.AddHealth(damage);
        DeductRemainingTurns();
    }

    public ScaledPoisonStatus() : base() => _maxHealthPercent = 0;
    public ScaledPoisonStatus(int turns, float maxHealthPercent) : base(turns) => _maxHealthPercent = maxHealthPercent;
}