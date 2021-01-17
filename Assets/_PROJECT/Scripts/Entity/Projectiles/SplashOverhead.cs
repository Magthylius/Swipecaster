﻿using Obtain;
using System.Collections.Generic;
using System.Linq;

public class SplashOverhead : Projectile
{
    private float _splashDamagePercent;
    public void SetSplashDamagePercent(float percent) => _splashDamagePercent = percent;

    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));

        //! Collateral
        info.Collateral.ForEach(i => i.TakeHit(damager, Round(subtotalDamage * _splashDamagePercent)));

        List<Unit> units = new List<Unit>(info.Collateral) { info.Focus };
        return units.Sum(unit => unit != null ? unit.GetTotalDamageInTurn : 0);
    }

    public override TargetInfo GetTargets(TargetInfo info)
        => new TargetInfo(info.Focus, GetCollateralFoes(info), new List<Unit>(), info.Allies, info.Foes);
    protected override List<Unit> GetCollateralFoes(TargetInfo info)
    {
        var collateral = new List<Unit>();
        int focusIndex = info.Foes.IndexOf(info.Focus);
        if (info.AllFoeEntities.ValidIndex(focusIndex - 1)) collateral.Add(info.AllFoeEntities[focusIndex - 1]);
        if (info.AllFoeEntities.ValidIndex(focusIndex + 1)) collateral.Add(info.AllFoeEntities[focusIndex + 1]);
        return collateral;
    }

    public SplashOverhead()
    {
        _projectileDamageMultiplier = 1.0f;
        _splashDamagePercent = 0.3f;
    }
    public SplashOverhead(float damageMultiplier, float splashDamagePercent) : base(damageMultiplier)
    {
        _splashDamagePercent = splashDamagePercent;
    }
    public SplashOverhead(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
        _splashDamagePercent = 0.3f;
    }
}