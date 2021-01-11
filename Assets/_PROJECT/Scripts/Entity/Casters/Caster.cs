using ConversionFunctions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Caster : Unit
{
    #region Variable Definitions

    [Header("Arch")]
    [SerializeField] private ArchTypeMajor archMajor = ArchTypeMajor.None;
    [SerializeField] private ArchTypeMinor archMinor = ArchTypeMinor.None;

    #endregion

    #region Public Override Methods

    public override void UseSkill(TargetInfo targetInfo, BattlestageManager battleStage) => GetActiveSkill.TriggerSkill(targetInfo, battleStage);
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        int rawDamage = Round(CalculateDamage(targetInfo, runes) * GetDamageMultiplier);
        int totalDamage = GetCurrentProjectile.AssignTargetDamage(this, targetInfo, rawDamage);
        GetStatusEffects.ForEach(status => status.DoEffectOnAction(targetInfo, totalDamage));
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = GetCurrentAttack;
        if (runes != RuneCollection.Null)
        {
            var relations = RuneRelations.GetRelations(GetRuneType);
            var advan = relations.Advantage.Single(); var weak = relations.Weakness.Single();
            float cummulatedRuneMultiplier = 1.0f + runes.GetAllStorages.Sum(rune => rune.Value.amount * GetRuneMultiplier(advan, weak, rune.Key));
            totalDamage = Round(totalDamage * cummulatedRuneMultiplier);
        }

        float statusOutMultiplier = 1.0f;
        statusOutMultiplier += GetStatusEffects.Sum(status => status.GetStatusDamageOutModifier());

        return Mathf.Abs(Round(totalDamage * statusOutMultiplier));
    }
    public override TargetInfo GetAffectedTargets(TargetInfo info)
        => GetCurrentProjectile.GetTargets(info);

    #endregion

    #region Public Methods

    public void SetArchMajor(ArchTypeMajor major) => archMajor = major;
    public ArchTypeMajor ArchMajor => archMajor;
    public void SetArchMinor(ArchTypeMinor minor) => archMinor = minor;
    public ArchTypeMinor ArchMinor => archMinor;

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetIsPlayer(true);
    }

    protected override void StartTurnMethods()
    {
        base.StartTurnMethods();
        GetActiveSkill?.TurnStartCall();
    }

    protected override void EndTurnMethods()
    {
        base.EndTurnMethods();
        GetActiveSkill?.TurnEndCall();
    }

    #endregion

    #region Private Methods

    private float GetRuneMultiplier(RuneType advantage, RuneType weakness, RuneType thisType)
    {
        return
            (advantage != thisType && weakness != thisType).AsInt() * 1.0f +
            (advantage == thisType).AsInt() * GetRuneAdvantageMultiplier() +
            (weakness == thisType).AsInt() * GetRuneWeaknessMultiplier();
    }

    #endregion
}
