﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dual : Projectile
{
    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage));
        var col = info.Collateral.FirstOrDefault();
        if(col != null) col.TakeHit(damager, Round(subtotalDamage));

        List<Unit> units = new List<Unit>() { info.Focus, col };
        return units.Sum(unit => unit != null ? unit.GetTotalDamageInTurn : 0);
    }

    public override TargetInfo GetTargets(TargetInfo info)
    {
        var collateral = new List<Unit>();
        var grazed = new List<Unit>();
        int focusIndex = info.Foes.IndexOf(info.Focus);
        int thisIndex = info.Allies.IndexOf(GetUnit);

        if (GetUnit != null)
        {
            for (int i = thisIndex; i >= 0; i--)
            {
                if (i == thisIndex) continue;
                grazed.Add(info.Allies[i]);
            }
        }
        for (int i = focusIndex; i >= 0; i--)
        {
            if (info.Foes[i] == info.Focus) continue;

            grazed.Add(info.Foes[i]);
        }

        info.Foes.Remove(info.Focus);
        if(info.Foes.Count != 0)
        {
            int randomIndex = Random.Range(0, info.Foes.Count);
            collateral.Add(info.Foes[randomIndex]);
            for (int i = randomIndex; i >= 0; i--)
            {
                if (i == randomIndex || grazed.Contains(info.Foes[i])) continue;

                grazed.Add(info.Foes[i]);
            }
        }

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.Foes);
    }
    protected override List<Unit> GetCollateralFoes(TargetInfo info)
    {
        var collateral = new List<Unit>();
        var temp = new List<Unit>(info.Foes);
        if(info.Foes.Count >= 2)
        {
            temp.Remove(info.Focus);
            collateral.Add(info.Foes[Random.Range(0, info.Foes.Count)]);
            return collateral;
        }
        return new List<Unit>();
    }

    public Dual() => _projectileDamageMultiplier = 1.0f;
    public Dual(float damageMultiplier) : base(damageMultiplier) { }
    public Dual(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
    }
}