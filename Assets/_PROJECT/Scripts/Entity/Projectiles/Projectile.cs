using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    protected float _projectileDamageMultiplier = 1.0f;

    public abstract TargetInfo GetTargets(TargetInfo info);
    public abstract int AssignTargetDamage(Unit damager, TargetInfo info, int damage);

    protected int Round(float number) => Mathf.RoundToInt(number);
    
    public void SetProjectileDamageMultiplier(float multiplier) => _projectileDamageMultiplier = multiplier;
    public float GetProjectileDamageMultiplier => _projectileDamageMultiplier;

    public virtual List<float> GetDefaultDiminishingMultiplier => null;
    public virtual List<float> GetCurrentDiminishingMultiplier => null;
    public virtual void SetDiminishingMultiplier(List<float> multiplier) { }
    public virtual void ResetDiminishingMultiplier() { }

    public Projectile() => _projectileDamageMultiplier = 1.0f;
    public Projectile(float damageMultiplier) => _projectileDamageMultiplier = Mathf.Abs(damageMultiplier);
}