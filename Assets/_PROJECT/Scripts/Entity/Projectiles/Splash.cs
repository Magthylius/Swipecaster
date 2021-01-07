using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        if (IndexWithinBounds(focusIndex - 1, info.Foes)) collateral.Add(info.Foes[focusIndex - 1]);
        if (IndexWithinBounds(focusIndex + 1, info.Foes)) collateral.Add(info.Foes[focusIndex + 1]);

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.Foes);
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