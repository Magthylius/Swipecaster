using ConversionFunctions;
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
        InvokeOnAttackEvent();
    }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = GetCurrentAttack;
        float cummulatedRuneMultiplier = 1.0f + CalculateCummulatedRuneMultiplier(runes) * NotEmptyCollection(runes).AsInt();
        float statusOutMultiplier = 1.0f + GetStatusEffects.Sum(status => status.GetStatusDamageOutModifier());
        return AbsRound(totalDamage * cummulatedRuneMultiplier * statusOutMultiplier);
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

    #region Private & Protected Methods

    protected float CalculateCummulatedRuneMultiplier(RuneCollection runes)
        => runes.GetAllStorages.Sum(rune => rune.Value.amount * GetRuneMultiplierFor(rune.Key));

    private float GetRuneMultiplierFor(RuneType comparingType)
    {
        var relations = GetRuneRelations(GetRuneType);
        var advantage = relations.SingleAdvantage;
        var weakness = relations.SingleWeakness;

        return
            (IsNeutral(), IsAdvantage(), IsWeakness()) switch
            {
                (true, false, false) => 1.0f,
                (false, true, false) => GetRuneAdvantageMultiplier(),
                (false, false, true) => GetRuneWeaknessMultiplier(),
                _ => 0.0f,
            };

        bool IsNeutral() => advantage != comparingType && weakness != comparingType;
        bool IsAdvantage() => advantage == comparingType;
        bool IsWeakness() => weakness == comparingType;
    }

    private static bool NotEmptyCollection(RuneCollection runes) => runes != RuneCollection.Null;

    #endregion
}
