using ConversionFunctions;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MothLamp : CasterSkill
{
    [SerializeField] private float reboundPercent = 1.0f;
    [SerializeField] private Summon lamp = null;
    private bool LampAlreadySpawned => lamp != null;

    public override string Description
        => $"Summons a Lamp on Casters side. Enemies that attack this Lamp take {RoundToPercent(reboundPercent)}% of Damage Dealt.";

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
            if (currentIteration > 100) return;

            index = Random.Range(0, battleStage.casterEntityPositions.Length);
            placementFound = positionAvailable[index];
        }

        casterEntityPos[index].gameObject.SetActive(true);
        GameObject lampObject = GetUnit.GetBaseUnit.InstantiateSummon(casterEntityPos[index].position, Quaternion.identity, casterEntityPos[index]);
        battleStage.GetCasterEntityTeam().Add(lampObject);
        battleStage.RegroupLeftPositions(false);
        
        lamp = lampObject.AsSummon();
        lamp.AddStatusEffect(Create.A_Status.Perm_FixedReboundDamage(reboundPercent));
        lamp.AddStatusEffect(Create.A_Status.Perm_PriorityUp(1));

        ResetSkillCharge();
    }

    public MothLamp(Unit unit)
    {
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }
    public MothLamp(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
