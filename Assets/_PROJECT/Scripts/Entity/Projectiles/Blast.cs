using System.Collections.Generic;
using System.Linq;

public class Blast : Projectile
{
    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        int count = 1 + info.Collateral.Count;
        int subtotalDamage = Round(damage * _projectileDamageMultiplier / count);

        //! Damage
        info.Focus.TakeHit(damager, subtotalDamage);
        info.Collateral.ForEach(i => i.TakeHit(damager, subtotalDamage));

        List<Unit> units = new List<Unit>(info.Collateral) { info.Focus };
        return units.Sum(unit => unit != null ? unit.GetTotalDamageInTurn : 0);
    }
    protected override List<Unit> GetCollateralFoes(TargetInfo info)
    {
        var collateral = new List<Unit>();
        for (int i = 0; i < info.Foes.Count; i++)
        {
            if (info.Foes[i] == info.Focus) continue;

            collateral.Add(info.Foes[i]);
        }
        return collateral;
    }

    public Blast() => _projectileDamageMultiplier = 1.0f;
    public Blast(float damageMultiplier) : base(damageMultiplier) { }
    public Blast(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
    }
}