using Obtain;
using System.Collections.Generic;
using System.Linq;

public class Splash : Projectile
{
    private float _splashDamagePercent;
    public void SetSplashDamagePercent(float percent) => _splashDamagePercent = percent;

    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
        info.Collateral.ForEach(i => i.TakeHit(damager, Round(subtotalDamage * _splashDamagePercent)));

        List<Unit> units = new List<Unit>(info.Collateral) { info.Focus };
        return units.Sum(unit => unit != null ? unit.GetTotalDamageInTurn : 0);
    }
    protected override List<Unit> GetCollateralFoes(TargetInfo info)
    {
        var collateral = new List<Unit>();
        int focusIndex = info.AllFoeEntities.IndexOf(info.Focus);
        if (info.AllFoeEntities.ValidIndex(focusIndex - 1)) collateral.Add(info.AllFoeEntities[focusIndex - 1]);
        if (info.AllFoeEntities.ValidIndex(focusIndex + 1)) collateral.Add(info.AllFoeEntities[focusIndex + 1]);
        return collateral;
    }

    public Splash()
    {
        _projectileDamageMultiplier = 1.0f;
        _splashDamagePercent = 0.3f;
    }
    public Splash(float damageMultiplier, float splashDamagePercent) : base(damageMultiplier)
    {
        _splashDamagePercent = splashDamagePercent;
    }
    public Splash(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
        _splashDamagePercent = 0.3f;
    }
}