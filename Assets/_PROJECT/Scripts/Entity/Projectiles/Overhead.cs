using System.Collections.Generic;

public class Overhead : Projectile
{
    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));

        return info.Focus.GetTotalDamageInTurn;
    }

    public override TargetInfo GetTargets(TargetInfo info)
    {
        var collateral = new List<Unit>();
        var grazed = new List<Unit>();

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.Foes);
    }

    public Overhead() => _projectileDamageMultiplier = 1.0f;
    public Overhead(float damageMultiplier) : base(damageMultiplier) { }
}