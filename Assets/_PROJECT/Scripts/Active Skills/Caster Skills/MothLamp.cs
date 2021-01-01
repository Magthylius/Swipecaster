using ConversionFunctions;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MothLamp : CasterSkill
{
    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        var casterEntityPos = battleStage.casterEntityPositions.ToList();
        List<bool> positionAvailable = new List<bool>();
        for (int i = 0; i < casterEntityPos.Count; i++)
        {
            var p = casterEntityPos[i];
            positionAvailable[i] = p.childCount != 0;
        }

        int index = -1;
        int currentIteration = 0;
        bool placementFound = false;
        while(!placementFound)
        {
            currentIteration++;
            if (currentIteration > 50) return;

            index = Random.Range(0, battleStage.casterEntityPositions.Length);
            placementFound = positionAvailable[index];
        }

        casterEntityPos[index].gameObject.SetActive(true);
        GetUnit.GetBaseUnit.InstantiateSummon(casterEntityPos[index].position, Quaternion.identity, casterEntityPos[index]);
    }

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {
        return TargetInfo.Null;
    }
}
