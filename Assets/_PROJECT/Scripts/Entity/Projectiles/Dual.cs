using System.Collections.Generic;
using UnityEngine;

public class Dual : Projectile
{
    public override void AssignTargetDamage(Entity damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _damageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
        info.Collateral[0].TakeHit(damager, Round(subtotalDamage));
    }

    public override TargetInfo GetTargets(Entity focus, List<Entity> allEntities)
    {
        var collateral = new List<Entity>();
        var grazed = new List<Entity>();
        int focusIndex = allEntities.IndexOf((global::Entity)focus);

        for (int i = focusIndex; i >= 0; i--)
        {
            if (allEntities[i] == focus) continue;

            grazed.Add(allEntities[i]);
        }

        allEntities.Remove(focus);
        int randomIndex = Random.Range(0, allEntities.Count);
        collateral.Add(allEntities[randomIndex]);
        for(int i = randomIndex; i >= 0; i--)
        {
            if (i == randomIndex || grazed.Contains(allEntities[i])) continue;

            grazed.Add(allEntities[i]);
        }

        return new TargetInfo(focus, collateral, grazed);
    }

    public Dual() => _damageMultiplier = 1.0f;
    public Dual(float damageMultiplier) : base(damageMultiplier) { }
}