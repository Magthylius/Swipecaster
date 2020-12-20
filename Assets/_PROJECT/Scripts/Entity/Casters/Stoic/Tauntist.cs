using System;
using System.Collections.Generic;
using UnityEngine;

public class Tauntist : Unit
{
    //! this has higher 'priority' than usual units

    #region Public Override Methods

    public override void TakeDamage(Entity damager, int damageAmount) => base.TakeDamage(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => base.DoDamage(focusTarget, affectedTargets, runes);
    public override int CalculateDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => base.CalculateDamage(focusTarget, affectedTargets, runes);
    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion
}