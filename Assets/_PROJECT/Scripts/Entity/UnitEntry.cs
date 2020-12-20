using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntry : Entity
{
    public int UnitID => Convert.ToInt32(baseUnit.ID);

    public override void TakeDamage(Entity damager, int damageAmount) { }
    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) { }
    public override int CalculateDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) => -1;
    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => null;
}
