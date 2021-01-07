using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Summon
{
    private Unit _latestDamager = null;

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);

        if (_latestDamager == damager) return;
        _latestDamager = damager;
        damager.TakeHit(damager, Round(damageAmount * currentReboundPercent));
    }

    protected override void EndTurnMethods()
    {
        base.EndTurnMethods();
        _latestDamager = null;
    }

    protected override void Awake()
    {
        base.Awake();

        SetIsPlayer(true);
    }
}
