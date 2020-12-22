using System.Collections.Generic;
using UnityEngine;

public class CrowFlies : Projectile
{
    public override void AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
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

        return new TargetInfo(focus, collateral, grazed);
    }

    public CrowFlies() => _projectileDamageMultiplier = 1.0f;
    public CrowFlies(float damageMultiplier) : base(damageMultiplier) { }
}