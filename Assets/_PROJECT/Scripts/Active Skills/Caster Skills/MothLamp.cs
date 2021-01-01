using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothLamp : CasterSkill
{
    public override void TriggerSkill(TargetInfo targetInfo, StageInfo stageInfo)
    {
        
    }

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {
        return TargetInfo.Null;
    }
}
