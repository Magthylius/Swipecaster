using System.Collections.Generic;

public class Overhead : Projectile
{
    public override void AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
    }

    public override TargetInfo GetTargets(Unit focus, List<Unit> allEntities)
    {
        //if (!allEntities.Contains(focus)) return TargetInfo.Null;

        var collateral = new List<Unit>();
        var grazed = new List<Unit>();

        return new TargetInfo(focus, collateral, grazed);
    }

    public Overhead() => _projectileDamageMultiplier = 1.0f;
    public Overhead(float damageMultiplier) : base(damageMultiplier) { }
}