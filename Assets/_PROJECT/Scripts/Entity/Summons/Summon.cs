using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Summon : Unit
{
    [SerializeField] private SummonObject baseSummon;

    public SummonObject GetBaseSummon => baseSummon;
    public override int GetBaseAttack => GetBaseSummon.MaxAttack;
    public override int GetBaseDefence => GetBaseSummon.MaxDefence;
    public override int GetMaxHealth => GetBaseSummon.MaxHealth;

    public override void UseSkill(TargetInfo targetInfo, BattlestageManager battleStage) { }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Unit healer, int healAmount) => AddCurrentHealth(healAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) { }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes) => 0;
    public override TargetInfo GetAffectedTargets(Unit focusTarget, List<Unit> allEntities) => TargetInfo.Null;

    protected override void Awake()
    {
        base.Awake();
        baseSummon.CalculateMaxStats(this);
    }
}
