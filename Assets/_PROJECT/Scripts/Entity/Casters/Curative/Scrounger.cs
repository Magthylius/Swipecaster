using System;
using System.Collections.Generic;
using UnityEngine;

public class Scrounger : Unit
{
    [SerializeField] private float damageToHealPercent;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        Projectile.AssignTargetDamage(this, targetInfo, totalDamage);

        Transform[] party = battleStage.leftSidePos;
        List<Entity> healList = new List<Entity>();
        for (int i = 0; i < party.Length; i++)
        {
            var entity = party[i].GetChild(0).GetComponent<Entity>();
            if (entity == null) continue;
            healList.Add(entity);
        }

        healList.ForEach(i => i.RecieveHealing(this, Round(totalDamage * damageToHealPercent)));
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

    #region Protected Override Methods

    protected override void TakeDamage(Entity damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);

        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        Transform[] party = battleStage.leftSidePos;
        List<Entity> healList = new List<Entity>();
        for (int i = 0; i < party.Length; i++)
        {
            var entity = party[i].GetChild(0).GetComponent<Entity>();
            if (entity == null) continue;
            healList.Add(entity);
        }

        healList.ForEach(i => i.RecieveHealing(this, Round(damageAmount * damageToHealPercent)));
    }

    #endregion

}