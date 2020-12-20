using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : Unit
{
    #region Public Override Methods

    public override void TakeDamage(Entity damager, int damageAmount)
    {

    }

    public override void RecieveHealing(Entity healer, int healAmount)
    {

    }

    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes)
    {

    }

    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities)
    {
        return null;
    }

    #endregion



}