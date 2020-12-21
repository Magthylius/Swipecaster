using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Entity
{
    BattlestageManager battleStageManager;
    protected virtual void Start()
    {
        battleStageManager = BattlestageManager.instance;
    }

    public override void TakeHit(Entity damager, int damageAmount) 
    { 
        AddHealth(-10); 
        if(GetCurrentHealth<=0)
        {
            battleStageManager.GetEnemyTeam().Remove(gameObject);
            Destroy(gameObject);
        }
    }
    public override void RecieveHealing(Entity healer, int healAmount) { }
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) { }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => -1;
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => TargetInfo.Null;

}
