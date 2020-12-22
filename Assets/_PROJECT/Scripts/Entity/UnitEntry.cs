using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntry : Entity
{
    public int UnitID => Convert.ToInt32(BaseUnit.ID);

    public override void TakeHit(Entity damager, int damageAmount) { }
    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) { }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => -1;
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => TargetInfo.Null;
}
