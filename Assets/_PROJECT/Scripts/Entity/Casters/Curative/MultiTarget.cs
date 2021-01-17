using System.Collections.Generic;
using UnityEngine;

public class MultiTarget : Curative
{
    [SerializeField] private bool randomHeal = false;
    [SerializeField, Range(2, 4)] private int randomTargetCount = 2;

    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return;

        int totalDamage = CalculateDamage(targetInfo, runes);
        List<Unit> party = new List<Unit>(battleStage.GetCasterTeamAsUnit());

        if(randomHeal)
        {
            int deleteCount = PartySize - randomTargetCount;
            for(int i = 0; i < deleteCount; i++) party.RemoveAt(Random.Range(0, party.Count));
        }

        party.ForEach(i => i.RecieveHealing(this, Round(totalDamage * GetPassiveHealPercent)));
        GetStatusEffects.ForEach(status => status.DoEffectOnAction(targetInfo, totalDamage));
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