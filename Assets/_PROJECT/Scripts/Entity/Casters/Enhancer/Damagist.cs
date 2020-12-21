using System;
using System.Collections.Generic;
using UnityEngine;

public class Damagist : Unit
{
    [SerializeField, Tooltip("Should be more than 1.0f")] private float damageBonusPercent;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) => base.DoAction(targetInfo, runes);
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        return Round(base.CalculateDamage(targetInfo, runes) * damageBonusPercent);
    }
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion
}