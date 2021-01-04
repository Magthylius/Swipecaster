using System.Collections.Generic;
using UnityEngine;

public class Splash : Projectile
{
    private float _splashDamagePercent;
    public void SetSplashDamagePercent(float percent) => _splashDamagePercent = percent;

    public override void AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
        info.Collateral.ForEach(i => i.TakeHit(damager, Round(subtotalDamage * _splashDamagePercent)));
    }

    public override TargetInfo GetTargets(Unit focus, List<Unit> allEntities)
    {
        var collateral = new List<Unit>();
        var grazed = new List<Unit>();
        int focusIndex = allEntities.IndexOf(focus);

        for (int i = focusIndex; i >= 0; i--)
        {
            if (allEntities[i] == focus) continue;

            grazed.Add(allEntities[i]);
        }

        if (IndexWithinBounds(focusIndex - 1, allEntities)) collateral.Add(allEntities[focusIndex - 1]);
        if (IndexWithinBounds(focusIndex + 1, allEntities)) collateral.Add(allEntities[focusIndex + 1]);

        return new TargetInfo(focus, collateral, grazed);
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

    private bool IndexWithinBounds(int index, List<Unit> list) => index >= 0 && index < list.Count;
}