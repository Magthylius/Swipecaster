using System;
using System.Collections.Generic;
using UnityEngine;

public class Spreader : Unit
{
    [SerializeField] private float damageMultiplier;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) => base.DoAction(targetInfo, runes);
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        return Round(base.CalculateDamage(targetInfo, runes) * damageMultiplier);
    }
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetProjectile(new Blast());
    }

    #endregion
}