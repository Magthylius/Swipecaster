using System.Collections.Generic;
using UnityEngine;

public class CrowFlies : Projectile
{
    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));

        return info.Focus.GetTotalDamageInTurn;
    }
    protected override List<Unit> GetCollateralFoes(TargetInfo info) => new List<Unit>();

    public CrowFlies() => _projectileDamageMultiplier = 1.0f;
    public CrowFlies(float damageMultiplier) : base(damageMultiplier) { }
    public CrowFlies(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
    }
}