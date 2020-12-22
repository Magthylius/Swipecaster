using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Debuffer : Unit
{
    private List<StatusEffect> _statusEffectList = new List<StatusEffect>()
    {
        new AttackDebuff(2, 50),
        new DefenceDebuff(2, 50)
    };

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        StatusEffect randomStatus = _statusEffectList[Random.Range(0, _statusEffectList.Count)];
        targetInfo.Focus.AddStatusEffect(randomStatus);

        base.DoAction(targetInfo, runes);
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    
}
