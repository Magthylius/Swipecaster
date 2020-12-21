using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    public abstract TargetInfo GetTargets(Entity focus, List<Entity> allEntities);
    public abstract void AssignTargetDamage(Entity damager, TargetInfo info, int damage);

    protected int Round(float number) => Mathf.RoundToInt(number);
}