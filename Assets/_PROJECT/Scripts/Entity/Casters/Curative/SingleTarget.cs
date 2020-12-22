using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : Unit
{
    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        int lowestHp = int.MaxValue;
        Entity target = null;
        Transform[] party = battleStage.casterPositions;
        for(int i = 0; i < party.Length; i++)
        {
            var entity = party[i].GetChild(0).GetComponent<Entity>();
            if (entity == null) continue;

            if(entity.GetCurrentHealth < lowestHp)
            {
                lowestHp = entity.GetCurrentHealth;
                target = entity;
            }
        }

        if (target == null) return;
        target.RecieveHealing(this, Round(totalDamage * damageToHealPercent));
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion


}