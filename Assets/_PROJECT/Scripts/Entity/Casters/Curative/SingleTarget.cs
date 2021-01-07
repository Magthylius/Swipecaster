using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : Curative
{
    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        int lowestHp = int.MaxValue;
        Unit target = null;
        Transform[] party = battleStage.casterPositions;
        for(int i = 0; i < party.Length; i++)
        {
            if (party[i].childCount == 0) continue;
            var unit = party[i].GetChild(0).GetComponent<Unit>();
            if (unit == null) continue;

            if(unit.GetCurrentHealth < lowestHp)
            {
                lowestHp = unit.GetCurrentHealth;
                target = unit;
            }
        }

        if (target == null) return;
        target.RecieveHealing(this, Round(totalDamage * currentPassiveHealPercent));
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