using System.Collections.Generic;
using System.Linq;

public class Blast : Projectile
{
    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
        info.Collateral.ForEach(i => i.TakeHit(damager, Round(subtotalDamage)));

        List<Unit> units = new List<Unit>(info.Collateral) { info.Focus };
        return units.Sum(unit => unit != null ? unit.GetTotalDamageInTurn : 0);
    }

    public override TargetInfo GetTargets(TargetInfo info)
    {
        var collateral = new List<Unit>();
        var grazed = new List<Unit>();

        for (int i = 0; i < info.Foes.Count; i++)
        {
            if (info.Foes[i] == info.Focus) continue;

            collateral.Add(info.Foes[i]);
        }

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.Foes);
    }

    public Blast() => _projectileDamageMultiplier = 1.0f;
    public Blast(float damageMultiplier) : base(damageMultiplier) { }
}