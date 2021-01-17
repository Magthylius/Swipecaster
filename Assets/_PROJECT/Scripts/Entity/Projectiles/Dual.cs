using ClampFunctions;
using Obtain;
using System.Collections.Generic;
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
        int focusIndex = info.AllFoeEntities.IndexOf(info.Focus);
        int thisIndex = info.AllAllyEntities.IndexOf(GetUnit);

        if (GetUnit != null)
        {
            for (int i = thisIndex; i >= 0; i--)
            {
                if (i == thisIndex) continue;
                grazed.Add(info.AllAllyEntities[i]);
            }
        }
        for (int i = focusIndex; i >= 0; i--)
        {
            if (info.AllFoeEntities[i] == info.Focus) continue;

            grazed.Add(info.AllFoeEntities[i]);
        }

        var temp = new List<Unit>(info.AllFoeEntities);
        temp.Remove(info.Focus);
        if(temp.Count.Not(0))
        {
            int randomIndex = Random.Range(0, temp.Count);
            collateral.Add(temp[randomIndex]);
            for (int i = randomIndex; i >= 0; i--)
            {
                if (i == randomIndex || grazed.Contains(temp[i])) continue;

                grazed.Add(temp[i]);
            }
        }

        return new TargetInfo(info.Focus, collateral, grazed, info.Allies, info.AllFoeEntities, info.AllAllyEntities, info.AllFoeEntities);
    }
    protected override List<Unit> GetCollateralFoes(TargetInfo info)
    {
        var collateral = new List<Unit>();
        var temp = new List<Unit>(info.AllFoeEntities);
        if(temp.Count.AtLeast(2))
        {
            temp.Remove(info.Focus);
            collateral.Add(temp.Random());
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