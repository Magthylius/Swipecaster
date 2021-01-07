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

    public override TargetInfo GetTargets(TargetInfo info)
    {
        var collateral = new List<Unit>();
        var grazed = new List<Unit>();
        int focusIndex = info.Foes.IndexOf(info.Focus);

        for (int i = focusIndex; i >= 0; i--)
        {
            if (info.Foes[i] == info.Focus) continue;

            grazed.Add(info.Foes[i]);
        }

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.Foes);
    }

    public CrowFlies() => _projectileDamageMultiplier = 1.0f;
    public CrowFlies(float damageMultiplier) : base(damageMultiplier) { }
}