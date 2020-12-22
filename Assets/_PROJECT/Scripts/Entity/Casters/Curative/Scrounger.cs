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

        int totalDamage = CalculateDamage(targetInfo, runes);
        GetProjectile.AssignTargetDamage(this, targetInfo, totalDamage);

        Transform[] party = battleStage.casterPositions;
        List<Unit> healList = new List<Unit>();
        for (int i = 0; i < party.Length; i++)
        {
            var unit = party[i].GetChild(0).GetComponent<Unit>();
            if (unit == null) continue;
            healList.Add(unit);
        }

        healList.ForEach(i => i.RecieveHealing(this, Round(totalDamage * damageToHealPercent)));
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