using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Unit
{
    public override void UseSkill(TargetInfo targetInfo, BattlestageManager battleStage) { }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int rawDamage = Round(CalculateDamage(targetInfo, runes) * GetDamageMultiplier);
        GetCurrentProjectile.AssignTargetDamage(this, targetInfo, rawDamage);
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => GetCurrentAttack;
    public override TargetInfo GetAffectedTargets(TargetInfo targetInfo)
        => GetCurrentProjectile.GetTargets(targetInfo);

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetIsPlayer(false);
    }

    #endregion
}
