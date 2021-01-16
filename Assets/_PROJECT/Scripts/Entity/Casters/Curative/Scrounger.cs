using System.Collections.Generic;

public class Scrounger : Curative
{
    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int rawDamage = CalculateDamage(targetInfo, runes);
        int totalDamage = GetCurrentProjectile.AssignTargetDamage(this, targetInfo, rawDamage);
        GetStatusEffects.ForEach(status => status.DoEffectOnAction(targetInfo, totalDamage));
        InvokeOnAttackEvent();

        List<Unit> party = battleStage.GetCasterTeamAsUnit();
        party.ForEach(i => i.RecieveHealing(this, Round(totalDamage * GetPassiveHealPercent)));
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Scrounger);
    }

    #endregion

}