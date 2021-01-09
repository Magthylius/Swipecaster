using ConversionFunctions;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MothLamp : CasterSkill
{
    [SerializeField] private float reboundPercent = 1.0f;
    [SerializeField] private Lamp lamp = null;
    private bool LampAlreadySpawned => lamp != null;

    public override string Description
        => $"Summons a Lamp on Casters side. Enemies that attack this Lamp take {RoundToPercent(reboundPercent)}% of Damage Dealt.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (LampAlreadySpawned) return;
        var casterEntityPos = battleStage.casterEntityPositions.ToList();
        var positionAvailable = IdentifyAvailablePositions(casterEntityPos);
        int index = SelectRandomPositionIndex(positionAvailable, battleStage);
        GameObject lampObject = InstantiateSummon(casterEntityPos, index);
        HandleBattleStage(battleStage, lampObject);
        HandleLamp(lampObject);
        ResetSkillCharge();
    }

    private List<bool> IdentifyAvailablePositions(List<Transform> casterEntityPos)
    {
        List<bool> positionAvailable = new List<bool>();
        for (int i = 0; i < casterEntityPos.Count; i++)
        {
            var p = casterEntityPos[i];
            positionAvailable.Add(p.childCount != 0);
        }
        return positionAvailable;
    }

    private int SelectRandomPositionIndex(List<bool> positionAvailable, BattlestageManager battleStage)
    {
        int index = -1;
        int currentIteration = 0;
        bool placementFound = false;
        while (!placementFound)
        {
            currentIteration++;
            if (currentIteration > 100) return 0;

            index = Random.Range(0, battleStage.casterEntityPositions.Length);
            placementFound = positionAvailable[index];
        }
        return index;
    }

    private GameObject InstantiateSummon(List<Transform> casterEntityPos, int index)
    {
        casterEntityPos[index].gameObject.SetActive(true);
        GameObject lampObject = GetUnit.GetBaseUnit.InstantiateSummon(casterEntityPos[index].position, Quaternion.identity, casterEntityPos[index]);
        return lampObject;
    }

    private static void HandleBattleStage(BattlestageManager battleStage, GameObject lampObject)
    {
        battleStage.GetCasterEntityTeam().Add(lampObject);
        battleStage.RegroupLeftPositions(false);
    }

    private void HandleLamp(GameObject lampObject)
    {
        lamp = lampObject.AsType<Lamp>();
        if (lamp == null) return;
        lamp.AddStatusEffect(Create.A_Status.Perm_FixedReboundDamage(reboundPercent));
        lamp.AddStatusEffect(Create.A_Status.Perm_PriorityUp(1));
        Unit.SubscribeDeathEvent(NullLamp);
    }
    private void NullLamp(Unit unit)
    {
        Unit.UnsubscribeDeathEvent(NullLamp);
        if (lamp == null) return;
        if (unit == lamp) lamp = null;
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
