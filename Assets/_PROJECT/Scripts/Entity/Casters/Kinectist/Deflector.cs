using System.Collections.Generic;
using UnityEngine;

public class Deflector : Kinectist
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Deflector);
        SubscribeGrazeEvent(Deflect);
        SubscribeSelfTurnEndEvent(ResetTrigger);
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
        if (!ProbabilityHit || TriggerOnce) return;
        TriggerOnce = true;
        damager.SetAttackStatus(AttackStatus.Deflected);
    }

    #endregion
}