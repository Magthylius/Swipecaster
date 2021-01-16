using System.Collections.Generic;
using UnityEngine;

public class Reflector : Kinectist
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Reflector);
        SubscribeGrazeEvent(Reflect);
        SubscribeSelfTurnEndEvent(ResetTrigger);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeGrazeEvent(Reflect);
        UnsubscribeSelfTurnEndEvent(ResetTrigger);
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
        if (ProbabilityHit && !TriggerOnce)
        {
            TriggerOnce = true;
            damager.SetAttackStatus(AttackStatus.Reflected);
            damager.TakeHit(this, Round(damageAmount * GetReboundPercent));
        }
    }

    #endregion
}