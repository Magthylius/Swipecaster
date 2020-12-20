using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    protected override void Awake()
    {
        base.Awake();
        _currentHealth = _totalHealth;
    }

    public override void TakeDamage(Entity damager, int damageAmount) => AddHealth(-damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => AddHealth(healAmount);

    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes)
    {
        int totalDamage = 0;


        focusTarget.TakeDamage(this, totalDamage);
    }

    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities)
    {
        return null;
    }

    

    
}
