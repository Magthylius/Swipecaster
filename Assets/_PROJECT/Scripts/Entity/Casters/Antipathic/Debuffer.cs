using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Debuffer : Antipathic
{
    private List<StatusEffect> _statusEffectList = new List<StatusEffect>()
    {
        // on-test and placeholder
        new Flame(2, 0.35f, false, 0.05f, 0.05f),
        new Poison(2, 0.35f, false, 0.05f, 0.05f)
    };

    #region Public Override Methods

    public override void DoAction(TargetInfo targetInfo, RuneCollection runes)
    {
        if(_statusEffectList.Count != 0)
        {
            StatusEffect randomStatus = _statusEffectList[Random.Range(0, _statusEffectList.Count)];
            if(randomStatus.ProbabilityHit(GetStatusEffects)) targetInfo.Focus.AddStatusEffect(randomStatus);
        }

        base.DoAction(targetInfo, runes);
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.Debuffer);
    }

    #endregion
}
