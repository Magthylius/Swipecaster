using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    [SerializeField] protected float _runeAdvantageMultiplier;
    [SerializeField] protected float _runeWeaknessMultiplier;

    #region Public Override Methods

    public override void TakeDamage(Entity damager, int damageAmount) => AddHealth(-Mathf.Abs(damageAmount));
    public override void RecieveHealing(Entity healer, int healAmount) => AddHealth(Math.Abs(healAmount));

    public override void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes)
    {
        int totalDamage = CalculateDamage(focusTarget, affectedTargets, runes);

        int nettDamage = totalDamage - focusTarget.GetDefence;
        if (nettDamage < 0) nettDamage = 0;

        focusTarget.TakeDamage(this, nettDamage);
    }

    public override int CalculateDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes)
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
    public override List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities) => null;

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        _currentHealth = _totalHealth;
    }

    #endregion

    #region Protected Methods

    protected int Round(float number) => Mathf.RoundToInt(number);
    protected int ToInt(bool statement) => Convert.ToInt32(statement);

    #endregion
}