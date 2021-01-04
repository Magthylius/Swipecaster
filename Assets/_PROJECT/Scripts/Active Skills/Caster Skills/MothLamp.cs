using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MothLamp : CasterSkill
{
    private Summon _lamp = null;

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (LampAlreadySpawned) return;

        var casterEntityPos = battleStage.casterEntityPositions.ToList();
        List<bool> positionAvailable = new List<bool>();
        for (int i = 0; i < casterEntityPos.Count; i++)
        {
            var p = casterEntityPos[i];
            positionAvailable.Add(p.childCount != 0);
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
        GameObject lampObject = GetUnit.GetBaseUnit.InstantiateSummon(casterEntityPos[index].position, Quaternion.identity, casterEntityPos[index]);
        _lamp = lampObject.GetComponent<Summon>();

        battleStage.RegroupLeftPositions(false);

        ResetSkillCharge();
    }

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
    {
        return TargetInfo.Null;
    }

    public MothLamp(Unit unit)
    {
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }

    private bool LampAlreadySpawned => _lamp != null;
}
