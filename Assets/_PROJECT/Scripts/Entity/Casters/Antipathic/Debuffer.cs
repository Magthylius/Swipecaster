using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Debuffer : Unit
{
    private List<StatusEffect> _statusEffectList = new List<StatusEffect>()
    {
        // on-test and placeholder
        new Flame(2, 0.35f, false, 0.05f, 0.05f),
        new Poison(2, 0.35f, false, 0.05f, 0.05f)
    };

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        if(_statusEffectList.Count != 0)
        {
            StatusEffect randomStatus = _statusEffectList[Random.Range(0, _statusEffectList.Count)];
            if(randomStatus.ProbabilityHit(GetStatusEffects)) targetInfo.Focus.AddStatusEffect(randomStatus);
        }

        base.DoAction(targetInfo, runes);
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    
}
