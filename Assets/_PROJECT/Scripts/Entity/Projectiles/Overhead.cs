using System.Collections.Generic;

public class Overhead : Projectile
{
    public override void AssignTargetDamage(Entity damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _damageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
    }

    public override TargetInfo GetTargets(Entity focus, List<Entity> allEntities)
    {
        if (!allEntities.Contains(focus)) return TargetInfo.Null;

        var collateral = new List<Entity>();
        var grazed = new List<Entity>();

        return new TargetInfo(focus, collateral, grazed);
    }

    public Overhead() => _damageMultiplier = 1.0f;
    public Overhead(float damageMultiplier) : base(damageMultiplier) { }
}