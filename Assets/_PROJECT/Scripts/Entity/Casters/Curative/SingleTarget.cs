using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SingleTarget : Curative
{
    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        float lowestHpRatio = 1.0f;
        Unit target = null;
        List<Unit> party = battleStage.GetCasterTeamAsUnit();
        for(int i = 0; i < party.Count; i++)
        {
            var unit = party[i];
            float unitHealthRatio = unit.GetHealthRatio;
            if(unitHealthRatio < lowestHpRatio)
            {
                lowestHpRatio = unitHealthRatio;
                target = unit;
            }
        }

        if (target == null)
        {
            var list = new List<Unit>(battleStage.GetCasterTeamAsUnit());
            target = list[Random.Range(0, list.Count)];
            if (target == null) return;
        }
        target.RecieveHealing(this, Round(totalDamage * GetPassiveHealPercent));
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.SingleTarget);
    }

    #endregion
}