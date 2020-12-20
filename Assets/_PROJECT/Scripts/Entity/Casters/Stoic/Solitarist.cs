using System;
using System.Collections.Generic;
using UnityEngine;

public class Solitarist : Unit
{
    [SerializeField] private int passiveHealAmount = 100;

    #region Public Override Methods

    public override void TakeDamage(Entity damager, int damageAmount) => base.TakeDamage(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => base.DoDamage(focusTarget, affectedTargets, runes);
    public override int CalculateDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => base.CalculateDamage(focusTarget, affectedTargets, runes);
    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SubscribeTurnBeginEvent(PassiveHeal);
    }

    #endregion

    #region Private Methods

    private void PassiveHeal() => AddHealth(Mathf.Abs(passiveHealAmount));
    private void OnDestroy() => UnsubscribeTurnBeginEvent(PassiveHeal);

    #endregion

}