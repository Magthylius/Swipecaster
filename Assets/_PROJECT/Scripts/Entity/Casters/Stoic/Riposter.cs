using System;
using System.Collections.Generic;
using UnityEngine;

public class Riposter : Unit
{
    private Entity _recentDamager = null;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) => base.DoAction(targetInfo, runes);
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeTurnEndEvent(ResetRecentDamager);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeTurnEndEvent(ResetRecentDamager);
    }

    protected override void TakeDamage(Entity damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);
        ReboundDamage(damager, damageAmount);
    }

    #endregion

    #region Private Methods

    private void ReboundDamage(Entity damager, int totalDamage)
    {
        if (!CanRebound(damager)) return;
        PrimeRecentDamager(damager);
        damager.TakeHit(this, Round(totalDamage * reboundPercent));
    }
    private bool CanRebound(Entity damager) => _recentDamager != damager;
    private void PrimeRecentDamager(Entity damager) => _recentDamager = damager;
    private void ResetRecentDamager() => _recentDamager = null;

    #endregion

}