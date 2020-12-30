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

    public override void UseSkill(TargetInfo targetInfo, StageInfo stageInfo) { }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Unit healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = Round(CalculateDamage(targetInfo, runes) * damageMultiplier);

        int nettDamage = totalDamage - targetInfo.Focus.GetCurrentDefence;
        if (nettDamage < 0) nettDamage = 0;

        GetProjectile.AssignTargetDamage(this, targetInfo, nettDamage);
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => GetCurrentAttack;
    public override TargetInfo GetAffectedTargets(Unit focusTarget, List<Unit> allEntities)
        => GetProjectile.GetTargets(focusTarget, allEntities);

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);
    }
}
