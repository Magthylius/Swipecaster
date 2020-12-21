using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    [SerializeField] protected float _runeAdvantageMultiplier;
    [SerializeField] protected float _runeWeaknessMultiplier;

    #region Public Override Methods

    public override void TakeHit(Entity damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Entity healer, int healAmount) => AddHealth(Math.Abs(healAmount));

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = CalculateDamage(targetInfo, runes);

        int nettDamage = totalDamage - targetInfo.Focus.GetDefence;
        if (nettDamage < 0) nettDamage = 0;

        Projectile.AssignTargetDamage(this, targetInfo, nettDamage);
    }

    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = 0;
        var relations = RuneRelations.GetRelations(GetRuneType);
        for (int i = 0; i < runes.GetAllStorages.Count; i++)
        {
            var r = runes.GetAllStorages[i];
            for (int j = 0; j < relations.advantage.Count; j++)
            {
                totalDamage += Round(GetAttack * r.amount * _runeAdvantageMultiplier) * ToInt(relations.advantage[j] == r.runeType);
                r.amount *= ToInt(relations.advantage[j] != r.runeType);
            }
            for (int k = 0; k < relations.weakness.Count; k++)
            {
                totalDamage += Round(GetAttack * r.amount * _runeWeaknessMultiplier) * ToInt(relations.weakness[k] == r.runeType);
                r.amount *= ToInt(relations.weakness[k] != r.runeType);
            }

            totalDamage += GetAttack * r.amount;
        }

        return totalDamage;
    }
    public override TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities)
        => projectile.GetTargets(focusTarget, allEntities);

    #endregion

    #region Protected Virtual Methods

    protected virtual void TakeDamage(Entity damager, int damageAmount)
    {
        if (damager.AttackStatus != AttackStatus.Normal) return;
        AddHealth(-Mathf.Abs(damageAmount));
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        _currentHealth = _totalHealth;
        SetProjectile(new CrowFlies());
        SubscribeTurnEndEvent(ResetAttackStatus);
    }

    #endregion

    #region Protected Methods

    protected int Round(float number) => Mathf.RoundToInt(number);
    protected int ToInt(bool statement) => Convert.ToInt32(statement);
    protected virtual void OnDestroy() => UnsubscribeTurnEndEvent(ResetAttackStatus);
    
    #endregion

    #region Private Methods

    private void ResetAttackStatus() => SetAttackStatus(AttackStatus.Normal);

    #endregion
}