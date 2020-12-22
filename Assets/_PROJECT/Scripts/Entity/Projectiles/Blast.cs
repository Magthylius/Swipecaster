using System.Collections.Generic;

public class Blast : Projectile
{
    public override void AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
        info.Collateral.ForEach(i => i.TakeHit(damager, Round(subtotalDamage)));
    }

    public override TargetInfo GetTargets(Unit focus, List<Unit> allEntities)
    {
        if (!allEntities.Contains(focus)) return TargetInfo.Null;

        var collateral = new List<Unit>();
        var grazed = new List<Unit>();

        for (int i = 0; i < allEntities.Count; i++)
        {
            if (allEntities[i] == focus) continue;

            collateral.Add(allEntities[i]);
        }

        return new TargetInfo(focus, collateral, grazed);
    }

    public Blast() => _projectileDamageMultiplier = 1.0f;
    public Blast(float damageMultiplier) : base(damageMultiplier) { }
}