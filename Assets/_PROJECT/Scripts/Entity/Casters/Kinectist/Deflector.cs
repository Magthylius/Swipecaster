using System.Collections.Generic;
using UnityEngine;

public class Deflector : Kinectist
{
    private bool triggerOnce = false;

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeGrazeEvent(Deflect);
        SubscribeSelfTurnEndEvent(ResetTrigger);

        SetArchMinor(ArchTypeMinor.Deflector);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeGrazeEvent(Deflect);
        UnsubscribeSelfTurnEndEvent(ResetTrigger);
    }

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        Deflect(damager, damageAmount);
        base.TakeDamage(damager, damageAmount);
    }

    #endregion

    #region Private Methods

    private void Deflect(Unit damager, int damageAmount)
    {
        if (!ProbabilityHit || triggerOnce) return;
        triggerOnce = true;
        damager.SetAttackStatus(AttackStatus.Deflected);
    }

    private void ResetTrigger() => triggerOnce = false;

    #endregion
}