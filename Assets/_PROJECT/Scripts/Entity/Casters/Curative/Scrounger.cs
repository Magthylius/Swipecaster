using System;
using System.Collections.Generic;
using UnityEngine;

public class Scrounger : Curative
{
    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int rawDamage = CalculateDamage(targetInfo, runes);
        int totalDamage = GetProjectile.AssignTargetDamage(this, targetInfo, rawDamage);
        GetStatusEffects.ForEach(status => status.DoEffectOnAction(targetInfo, totalDamage));

        List<Unit> party = new List<Unit>(battleStage.GetCasterTeamAsUnit());
        party.ForEach(i => i.RecieveHealing(this, Round(totalDamage * currentPassiveHealPercent)));
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