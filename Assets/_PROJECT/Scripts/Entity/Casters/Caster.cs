using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : Unit
{
    #region Variable Definitions

    [Header("Arch")]
    [SerializeField] private ArchTypeMajor archMajor = ArchTypeMajor.None;
    [SerializeField] private ArchTypeMinor archMinor = ArchTypeMinor.None;

    [Header("Rune Relation Multiplier")]
    [SerializeField] protected float runeAdvantageMultiplier = 1.5f;
    [SerializeField] protected float runeWeaknessMultiplier = 0.5f;

    #endregion

    #region Public Override Methods

    public override void UseSkill(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {
        print("ues");
        /*
        var info = GetActiveSkill.GetActiveSkillTargets(focusTarget, allCasters, allFoes);
        GetActiveSkill.TriggerSkill(info, allCasters, allFoes);
        */
    }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void RecieveHealing(Unit healer, int healAmount) => AddCurrentHealth(Mathf.Abs(healAmount));
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
        for (int i = 0; i < GetStatusEffects.Count; i++) statusOutMultiplier += GetStatusEffects[i].GetStatusDamageOutModifier();

        return Mathf.Abs(Round(totalDamage * statusOutMultiplier));
    }
    public override TargetInfo GetAffectedTargets(Unit focusTarget, List<Unit> allEntities)
        => GetProjectile.GetTargets(focusTarget, allEntities);

    #endregion

    #region Public Methods

    public void SetArchMajor(ArchTypeMajor major) => archMajor = major;
    public ArchTypeMajor ArchMajor => archMajor;
    public void SetArchMinor(ArchTypeMinor minor) => archMinor = minor;
    public ArchTypeMinor ArchMinor => archMinor;

    #endregion
}
