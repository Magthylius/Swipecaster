using System;
using System.Collections.Generic;

public class CrowFlies : Projectile
{
    public override void AssignTargetDamage(Entity damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, damage));

        //! Damage
        info.Focus.TakeHit(damager, damage);
    }

    public override TargetInfo GetTargets(Entity focus, List<Entity> allEntities)
    {
        if (!allEntities.Contains(focus)) return TargetInfo.Null;
        
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

    public CrowFlies() { }
}