using System.Collections.Generic;
using UnityEngine;

public class MultiTarget : Unit
{
    [SerializeField] private bool randomHeal;
    [SerializeField, Range(2, 4)] private int targetCount;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => base.TakeHit(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => base.RecieveHealing(healer, healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        UpdatePreStatusEffects();
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        
        Transform[] party = battleStage.casterPositions;
        List<Entity> healList = new List<Entity>();
        for(int i = 0; i < party.Length; i++)
        {
            var entity = party[i].GetChild(0).GetComponent<Entity>();
            if (entity == null) continue;
            healList.Add(entity);
        }

        if(randomHeal)
        {
            int deleteCount = PartySize - targetCount;
            for(int i = 0; i < deleteCount; i++) healList.RemoveAt(Random.Range(0, healList.Count));
        }

        healList.ForEach(i => i.RecieveHealing(this, Round(totalDamage * damageToHealPercent)));
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => base.CalculateDamage(targetInfo, runes);
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => base.GetAffectedTargets(focusTarget, allEntities);

    #endregion

}