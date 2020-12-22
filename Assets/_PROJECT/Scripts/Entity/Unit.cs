using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Unit : Entity
{
    #region Variable Definitions

    [Header("Rune Relation Multiplier")]
    [SerializeField] protected float runeAdvantageMultiplier = 1.5f;
    [SerializeField] protected float runeWeaknessMultiplier = 0.5f;

    [Header("Other Multipliers")]
    [SerializeField] protected float damageMultiplier = 1.0f;

    [Header("Recovery")]
    [SerializeField] protected int passiveHealAmount = 0;
    [SerializeField] protected float damageToHealPercent = 0.0f;

    [Header("Reflection and Deflection")]
    [SerializeField] protected float reboundPercent = 0.2f;

    [Header("RNG")]
    [SerializeField, Range(0.0f, 1.0f)] protected float probability = 0.05f;
    protected const int PartySize = 4;

    #endregion

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => AddHealth(Math.Abs(healAmount));

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = Round(CalculateDamage(targetInfo, runes) * damageMultiplier);

        int nettDamage = totalDamage - targetInfo.Focus.GetCurrentDefence;
        if (nettDamage < 0) nettDamage = 0;

        GetProjectile.AssignTargetDamage(this, targetInfo, nettDamage);
    }

    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = 0;
        var relations = RuneRelations.GetRelations(GetRuneType);
        for (int i = 0; i < runes.GetAllStorages.Count; i++)
        {
            var r = runes.GetAllStorages[i];
            for (int j = 0; j < relations.Advantage.Count; j++)
            {
                totalDamage += Round(GetCurrentAttack * r.amount * runeAdvantageMultiplier) * ToInt(relations.Advantage[j] == r.runeType);
                r.amount *= ToInt(relations.Advantage[j] != r.runeType);
            }
            for (int k = 0; k < relations.Weakness.Count; k++)
            {
                totalDamage += Round(GetCurrentAttack * r.amount * runeWeaknessMultiplier) * ToInt(relations.Weakness[k] == r.runeType);
                r.amount *= ToInt(relations.Weakness[k] != r.runeType);
            }

            totalDamage += GetCurrentAttack * r.amount;
        }

        float statusOutMultiplier = 1.0f;
        for(int i = 0; i < GetStatusEffects.Count; i++) statusOutMultiplier += GetStatusEffects[i].GetStatusDamageOutModifier();
        
        return Mathf.Abs(Round(totalDamage * statusOutMultiplier));
    }
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities)
        => GetProjectile.GetTargets(focusTarget, allEntities);

    #endregion

    #region Protected Virtual Methods

    protected virtual void TakeDamage(Entity damager, int damageAmount)
    {
        if (damager.GetAttackStatus != AttackStatus.Normal) return;
        float statusInMultiplier = 1.0f;
        for (int i = 0; i < GetStatusEffects.Count; i++) statusInMultiplier += GetStatusEffects[i].GetStatusDamageInModifier();
        
        AddHealth(-Mathf.Abs(Round(damageAmount * statusInMultiplier)));
    }

    protected virtual void UpdatePreStatusEffects()
    {
        ResetAtkDefStats();

        for(int i = GetStatusEffects.Count - 1; i >= 0; i--)
        {
            if(GetStatusEffects[i].ShouldClear()) GetStatusEffects.RemoveAt(i);
        }

        GetStatusEffects.ForEach(j => j.DoPreEffect(this));
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeHitEvent(TakeDamage);
        UnsubscribeTurnEndEvent(ResetAttackStatus);
        UnsubscribeTurnEndEvent(PostStatusEffect);
        UnsubscribeTurnBeginEvent(UpdatePreStatusEffects);
        UnsubscribeHealthChangeEvent(CheckDeathEvent);
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetProjectile(new CrowFlies());
        SubscribeHitEvent(TakeDamage);
        SubscribeTurnEndEvent(ResetAttackStatus);
        SubscribeTurnEndEvent(PostStatusEffect);
        SubscribeTurnBeginEvent(UpdatePreStatusEffects);
        SubscribeHealthChangeEvent(CheckDeathEvent);
    }

    #endregion

    #region Protected Methods

    protected bool ProbabilityHit => Random.Range(0.0f, 1.0f - float.Epsilon) < probability;
    protected int Round(float number) => Mathf.RoundToInt(number);
    protected int ToInt(bool statement) => Convert.ToInt32(statement);
    
    #endregion

    #region Private Methods

    private void PostStatusEffect() => GetStatusEffects.ForEach(i => i.DoPostEffect(this));
    private void ResetAttackStatus() => SetAttackStatus(AttackStatus.Normal);
    private void CheckDeathEvent()
    {
        if (GetCurrentHealth <= 0) InvokeDeathEvent(this);
    }

    #endregion
}