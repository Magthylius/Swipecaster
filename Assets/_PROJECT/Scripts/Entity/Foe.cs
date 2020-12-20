using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Entity
{
    public override void TakeDamage(Entity damager, int damageAmount) { }
    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes) { }
    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => null;
}
