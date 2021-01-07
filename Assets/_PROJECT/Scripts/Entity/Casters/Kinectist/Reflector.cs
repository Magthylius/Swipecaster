using System.Collections.Generic;
using UnityEngine;

public class Reflector : Kinectist
{
    private bool triggerOnce = false;

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeGrazeEvent(Reflect);
        SubscribeTurnEndEvent(ResetTrigger);

        SetArchMinor(ArchTypeMinor.Reflector);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeGrazeEvent(Reflect);
        UnsubscribeTurnEndEvent(ResetTrigger);
    }

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        Reflect(damager, damageAmount);
        base.TakeDamage(damager, damageAmount);
    }

    #endregion

    #region Private Methods

    private void Reflect(Unit damager, int damageAmount)
    {
        if (!ProbabilityHit || triggerOnce) return;
        triggerOnce = true;
        damager.SetAttackStatus(AttackStatus.Reflected);
        damager.TakeHit(this, Round(damageAmount * currentReboundPercent));
    }

    private void ResetTrigger() => triggerOnce = false;

    #endregion
}