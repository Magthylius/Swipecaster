using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothLamp : CasterSkill
{
    public override void TriggerSkill(TargetInfo targetInfo, StageInfo stageInfo)
    {
        //int random = -1;
        //while(random == -1 || )

        //Random.Range(0, stageInfo.CasterEntitySet.Count);


        //GameObject summonObject = GetUnit.BaseUnit.InstantiateSummon();
        //if (summonObject == null) return;

        //! summon randomly on caster side

    }

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {
        return TargetInfo.Null;
    }
}
