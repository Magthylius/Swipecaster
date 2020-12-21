using System;
using System.Collections.Generic;

public class Overhead : Projectile
{
    public override void AssignTargetDamage(Entity damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        //! Damage
        info.Focus.TakeHit(damager, damage);
    }

    public override TargetInfo GetTargets(Entity focus, List<Entity> allEntities)
    {
        if (!allEntities.Contains(focus)) return TargetInfo.Null;

        var collateral = new List<Entity>();
        var grazed = new List<Entity>();

        return new TargetInfo(focus, collateral, grazed);
    }

    public Overhead() { }
}