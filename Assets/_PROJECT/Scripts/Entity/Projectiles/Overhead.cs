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
        => new TargetInfo(info.Focus, new List<Unit>(), new List<Unit>(), info.Allies, info.Foes);
    protected override List<Unit> GetCollateralFoes(TargetInfo info) => new List<Unit>();

    public Overhead() => _projectileDamageMultiplier = 1.0f;
    public Overhead(float damageMultiplier) : base(damageMultiplier) { }
    public Overhead(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
    }
}