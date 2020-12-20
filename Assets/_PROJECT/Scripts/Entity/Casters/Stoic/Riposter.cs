using System;
using System.Collections.Generic;
using UnityEngine;

public class Riposter : Unit
{
    private Entity _recentDamager = null;
    [SerializeField] private float reflectionPercent = 0.2f;

    #region Public Override Methods

    public override void TakeDamage(Entity damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);
        
        if (!CanReflect(damager)) return;
        PrimeRecentDamager(damager);
        damager.TakeDamage(this, Round(damageAmount * reflectionPercent));
    }
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => base.DoDamage(focusTarget, affectedTargets, runes);
    public override int CalculateDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => base.CalculateDamage(focusTarget, affectedTargets, runes);
    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeTurnEndEvent(ResetRecentDamager);
    }

    #endregion

    #region Private Methods

    private bool CanReflect(Entity damager) => _recentDamager != damager;
    private void PrimeRecentDamager(Entity damager) => _recentDamager = damager;
    private void ResetRecentDamager() => _recentDamager = null;
    private void OnDestroy() => UnsubscribeTurnEndEvent(ResetRecentDamager);

    #endregion

}