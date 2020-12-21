using System.Collections.Generic;
using UnityEngine;

public class Deflector : Unit
{
    private bool triggerOnce = false;

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
        SubscribeGrazeEvent(Deflect);
        SubscribeTurnEndEvent(ResetTrigger);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnsubscribeGrazeEvent(Deflect);
        UnsubscribeTurnEndEvent(ResetTrigger);
    }

    protected override void TakeDamage(Entity damager, int damageAmount)
    {
        Deflect(damager, damageAmount);
        base.TakeDamage(damager, damageAmount);
    }

    #endregion

    #region Private Methods

    private void Deflect(Entity damager, int damageAmount)
    {
        if (!ProbabilityHit || triggerOnce) return;
        triggerOnce = true;
        damager.SetAttackStatus(AttackStatus.Deflected);
    }

    private void ResetTrigger() => triggerOnce = false;

    private bool ProbabilityHit => Random.Range(0.0f, 1.0f) <= probability;

    #endregion
}