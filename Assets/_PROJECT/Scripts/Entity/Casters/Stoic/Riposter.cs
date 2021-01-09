using System;
using System.Collections.Generic;
using UnityEngine;

public class Riposter : Stoic
{
    private Unit _recentDamager = null;

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeSelfTurnEndEvent(ResetRecentDamager);

        SetArchMinor(ArchTypeMinor.Riposter);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeSelfTurnEndEvent(ResetRecentDamager);
    }

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);
        ReboundDamage(damager, damageAmount);
    }

    #endregion

    #region Private Methods

    private void ReboundDamage(Unit damager, int totalDamage)
    {
        if (!CanRebound(damager)) return;
        PrimeRecentDamager(damager);
        damager.TakeHit(this, Round(totalDamage * currentReboundPercent));
    }
    private bool CanRebound(Unit damager) => _recentDamager != damager && damager != null;
    private void PrimeRecentDamager(Unit damager) => _recentDamager = damager;
    private void ResetRecentDamager() => _recentDamager = null;

    #endregion

}