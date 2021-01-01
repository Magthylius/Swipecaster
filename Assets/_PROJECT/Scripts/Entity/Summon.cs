using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Unit
{
    private Unit _latestDamager = null;

    public override void UseSkill(TargetInfo targetInfo, StageInfo stageInfo) { }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Unit healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) { }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => 0;
    public override TargetInfo GetAffectedTargets(Unit focusTarget, List<Unit> allEntities) => TargetInfo.Null;

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);

        if (_latestDamager == damager) return;
        _latestDamager = damager;
        damager.TakeHit(damager, damageAmount);
    }
    protected override void EndTurnMethods()
    {
        base.EndTurnMethods();
        _latestDamager = null;
    }
}
