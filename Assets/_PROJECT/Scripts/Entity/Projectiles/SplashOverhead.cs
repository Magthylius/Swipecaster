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
    {
        var collateral = new List<Unit>();
        var grazed = new List<Unit>();
        int focusIndex = info.Foes.IndexOf(info.Focus);

        if (focusIndex - 1 >= 0) collateral.Add(info.Foes[focusIndex - 1]);
        if (focusIndex + 1 < info.Foes.Count) collateral.Add(info.Foes[focusIndex + 1]);

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.Foes);
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
}