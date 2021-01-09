using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    protected float _projectileDamageMultiplier = 1.0f;
    private Unit _unit;

    #region Abstract Methods

    public abstract int AssignTargetDamage(Unit damager, TargetInfo info, int damage);
    protected abstract List<Unit> GetCollateralFoes(TargetInfo info);

    #endregion

    #region Public Virtual Methods

    public virtual TargetInfo GetTargets(TargetInfo info)
        => new TargetInfo(info.Focus, GetCollateralFoes(info), GetGrazedUnits(info), info.Allies, info.Foes);
    public virtual List<float> GetDefaultDiminishingMultiplier => null;
    public virtual List<float> GetCurrentDiminishingMultiplier => null;
    public virtual void SetDiminishingMultiplier(List<float> multiplier) { }
    public virtual void ResetDiminishingMultiplier() { }

    #endregion

    #region Public Methods

    public void SetProjectileDamageMultiplier(float multiplier) => _projectileDamageMultiplier = multiplier;
    public float GetProjectileDamageMultiplier => _projectileDamageMultiplier;
    public Unit GetUnit => _unit;
    public void SetUnit(Unit unit) => _unit = unit;

    #endregion

    #region Protected Virtual Methods

    protected virtual List<Unit> GetGrazedAllies(TargetInfo info)
    {
        if (GetUnit == null) return null;
        var grazed = new List<Unit>();
        int thisIndex = info.Allies.IndexOf(GetUnit);

        for (int i = thisIndex; i >= 0; i--)
        {
            if (i == thisIndex) continue;
            grazed.Add(info.Allies[i]);
        }
        return grazed;
    }
    protected virtual List<Unit> GetGrazedFoes(TargetInfo info)
    {
        var grazed = new List<Unit>();
        int focusIndex = info.Foes.IndexOf(info.Focus);
        for (int i = focusIndex; i >= 0; i--)
        {
            if (info.Foes[i] == info.Focus) continue;
            grazed.Add(info.Foes[i]);
        }
        return grazed;
    }

    #endregion

    #region Protected Methods

    protected int Round(float number) => Mathf.RoundToInt(number);
    protected bool IndexWithinBounds(int index, List<Unit> list) => index >= 0 && index < list.Count;
    protected List<Unit> GetGrazedUnits(TargetInfo info) => GetGrazedAllies(info).Join(GetGrazedFoes(info));

    #endregion

    public Projectile() => _projectileDamageMultiplier = 1.0f;
    public Projectile(float damageMultiplier) => _projectileDamageMultiplier = Mathf.Abs(damageMultiplier);
}

public static class Extension
{
    public static List<T> Join<T>(this List<T> first, List<T> second)
    {
        if (first == null && second == null) return new List<T>();
        if (first == null) return second;
        if (second == null) return first;

        return first.Concat(second).ToList();
    }
}