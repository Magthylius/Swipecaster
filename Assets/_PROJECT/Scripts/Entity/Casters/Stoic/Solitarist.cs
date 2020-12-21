using System;
using System.Collections.Generic;
using UnityEngine;

public class Solitarist : Unit
{
    [SerializeField] private int passiveHealAmount = 100;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) => base.DoAction(targetInfo, runes);
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeTurnBeginEvent(PassiveHeal);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeTurnBeginEvent(PassiveHeal);
    }

    #endregion

    #region Private Methods

    private void PassiveHeal() => AddHealth(Mathf.Abs(passiveHealAmount));
    
    #endregion
}