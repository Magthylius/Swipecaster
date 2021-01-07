using System;
using System.Collections.Generic;
using UnityEngine;

public class Solitarist : Stoic
{
    #region Public Override Methods

    public override void RecieveHealing(Unit healer, int healAmount) { }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeTurnBeginEvent(PassiveHeal);

        SetArchMinor(ArchTypeMinor.Solitarist);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeTurnBeginEvent(PassiveHeal);
    }

    #endregion

    #region Private Methods

    private void PassiveHeal() => AddCurrentHealth(Mathf.Abs(currentPassiveHealAmount));
    
    #endregion
}