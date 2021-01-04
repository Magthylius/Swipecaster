using System.Collections.Generic;
using UnityEngine;

public class MultiTarget : Curative
{
    [SerializeField] private bool randomHeal;
    [SerializeField, Range(2, 4)] private int targetCount;

    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        
        Transform[] party = battleStage.casterPositions;
        List<Unit> healList = new List<Unit>();
        for(int i = 0; i < party.Length; i++)
        {
            if (party[i].childCount == 0) continue;
            var unit = party[i].GetChild(0).GetComponent<Unit>();
            if (unit == null) continue;
            healList.Add(unit);
        }

        if(randomHeal)
        {
            int deleteCount = PartySize - targetCount;
            for(int i = 0; i < deleteCount; i++) healList.RemoveAt(Random.Range(0, healList.Count));
        }

        healList.ForEach(i => i.RecieveHealing(this, Round(totalDamage * damageToHealPercent)));
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.MultiTarget);
    }

    #endregion
}