using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardCover : CasterSkill
{
    public override string Description
        => "STUNS all enemies (1 turn)";

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
        => new TargetInfo(focusTarget, null, null, allCasters, allFoes);

    public override void TriggerSkill(TargetInfo targetInfo, StageInfo stageInfo)
    {
        if (targetInfo.Foes == null || targetInfo.Foes.Count == 0) return;

        targetInfo.Foes.ForEach(foe => foe.AddStatusEffect(Create.A_Status.Stun(1)));
        GetUnit.ResetSkillCharge();
    }

    // public HardCover(float damageMultiplier, int effectDuration, int maxSkillCharge, int chargeGainPerTurn)
    //     : base(damageMultiplier, effectDuration, maxSkillCharge, chargeGainPerTurn) { }
    public HardCover(Unit unit)
    {
        _skillDamageMultiplier = 1.0f;
        _effectDuration = 0;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _unit = unit;
    }
}
