using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Unit
{
    public override void UseSkill(TargetInfo targetInfo, StageInfo stageInfo) { }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Unit healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int rawDamage = Round(CalculateDamage(targetInfo, runes) * damageMultiplier);
        GetProjectile.AssignTargetDamage(this, targetInfo, rawDamage);
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => GetCurrentAttack;
    public override TargetInfo GetAffectedTargets(Unit focusTarget, List<Unit> allEntities)
        => GetProjectile.GetTargets(focusTarget, allEntities);
}
