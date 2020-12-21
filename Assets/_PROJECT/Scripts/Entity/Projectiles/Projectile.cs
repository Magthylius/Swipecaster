using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    protected float _damageMultiplier = 1.0f;

    public abstract TargetInfo GetTargets(Entity focus, List<Entity> allEntities);
    public abstract void AssignTargetDamage(Entity damager, TargetInfo info, int damage);
    public void UpdateMultiplier(float newMultiplier) => _damageMultiplier = newMultiplier;

    protected int Round(float number) => Mathf.RoundToInt(number);

    public Projectile() => _damageMultiplier = 1.0f;
    public Projectile(float damageMultiplier) => _damageMultiplier = damageMultiplier;
}