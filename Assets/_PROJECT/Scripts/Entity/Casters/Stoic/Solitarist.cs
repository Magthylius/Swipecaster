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
        SetArchMinor(ArchTypeMinor.Solitarist);
        SubscribeSelfTurnBeginEvent(PassiveHeal);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeSelfTurnBeginEvent(PassiveHeal);
    }

    #endregion

    #region Private Methods

    private void PassiveHeal() => AddCurrentHealth(Mathf.Abs(GetPassiveHealAmount));
    
    #endregion
}