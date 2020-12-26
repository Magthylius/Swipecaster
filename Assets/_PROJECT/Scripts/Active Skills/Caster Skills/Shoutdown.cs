using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoutdown : CasterSkill
{
    public override string Description 
        => "Increases all caster DMG by 25%";

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
        => new TargetInfo(focusTarget, null, null, allCasters, allFoes);

    public override void TriggerSkill(TargetInfo targetInfo, StageInfo stageInfo)
    {
        if (targetInfo.Casters == null || targetInfo.Casters.Count == 0) return;

        targetInfo.Casters.ForEach(caster => caster.AddStatusEffect(Create.A_Status.AttackUp(_effectDuration, 0.25f)));
        GetUnit.ResetSkillCharge();
    }

    public Shoutdown(float damageMultiplier, int effectDuration, int maxSkillCharge, int chargeGainPerTurn)
        : base(damageMultiplier, effectDuration, maxSkillCharge, chargeGainPerTurn) { }

    public Shoutdown(Unit unit)
    {
        _skillDamageMultiplier = 1.0f;
        _effectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _unit = unit;
    }
}
