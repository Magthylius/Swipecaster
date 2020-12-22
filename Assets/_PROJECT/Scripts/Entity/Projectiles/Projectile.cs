using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    protected float _projectileDamageMultiplier = 1.0f;

    public abstract TargetInfo GetTargets(Unit focus, List<Unit> allEntities);
    public abstract void AssignTargetDamage(Unit damager, TargetInfo info, int damage);
    public void UpdateMultiplier(float newMultiplier) => _projectileDamageMultiplier = newMultiplier;

    protected int Round(float number) => Mathf.RoundToInt(number);
    
    public void SetProjectileDamageMultiplier(float multiplier) => _projectileDamageMultiplier = multiplier;
    public float GetProjectileDamageMultiplier => _projectileDamageMultiplier;

    public Projectile() => _projectileDamageMultiplier = 1.0f;
    public Projectile(float damageMultiplier) => _projectileDamageMultiplier = Mathf.Abs(damageMultiplier);
}