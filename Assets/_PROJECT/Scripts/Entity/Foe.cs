using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Unit
{
    BattlestageManager battleStageManager;
    protected virtual void Start()
    {
        battleStageManager = BattlestageManager.instance;
    }

    public override void TakeHit(Entity damager, int damageAmount)
    {
        base.TakeHit(damager, damageAmount);
    }

    protected override void TakeDamage(Entity damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);
        if (GetCurrentHealth <= 0)
        {
            battleStageManager.GetEnemyTeam().Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = Round(CalculateDamage(targetInfo, runes) * damageMultiplier);

        int nettDamage = totalDamage - targetInfo.Focus.GetCurrentDefence;
        if (nettDamage < 0) nettDamage = 0;

        GetProjectile.AssignTargetDamage(this, targetInfo, nettDamage);
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => GetCurrentAttack;
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

}
