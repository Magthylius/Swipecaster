using System.Collections.Generic;

public class SplashOverhead : Projectile
{
    private float _splashDamagePercent;
    public void SetSplashDamagePercent(float percent) => _splashDamagePercent = percent;

    public override void AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));

        //! Collateral
        info.Collateral.ForEach(i => i.TakeHit(damager, Round(subtotalDamage * _splashDamagePercent)));
    }

    public override TargetInfo GetTargets(Unit focus, List<Unit> allEntities)
    {
        if (!allEntities.Contains(focus)) return TargetInfo.Null;

        var collateral = new List<Unit>();
        var grazed = new List<Unit>();
        int focusIndex = allEntities.IndexOf(focus);

        if (focusIndex - 1 >= 0) collateral.Add(allEntities[focusIndex - 1]);
        if (focusIndex + 1 < allEntities.Count) collateral.Add(allEntities[focusIndex + 1]);

        return new TargetInfo(focus, collateral, grazed);
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