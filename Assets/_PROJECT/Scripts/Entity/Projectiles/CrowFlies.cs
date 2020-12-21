using System.Collections.Generic;
using UnityEngine;

public class CrowFlies : Projectile
{
    public override void AssignTargetDamage(Entity damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _damageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
    }

    public override TargetInfo GetTargets(Entity focus, List<Entity> allEntities)
    {
        var collateral = new List<Entity>();
        var grazed = new List<Entity>();
        int focusIndex = allEntities.IndexOf(focus);

        for (int i = focusIndex; i >= 0; i--)
        {
            if (allEntities[i] == focus) continue;

            grazed.Add(allEntities[i]);
        }

        return new TargetInfo(focus, collateral, grazed);
    }

    public CrowFlies() => _damageMultiplier = 1.0f;
    public CrowFlies(float damageMultiplier) : base(damageMultiplier) { }
}