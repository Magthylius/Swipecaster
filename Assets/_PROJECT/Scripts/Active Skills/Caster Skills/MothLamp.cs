using ConversionFunctions;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Obtain;

[System.Serializable]
public class MothLamp : CasterSkill
{
    [SerializeField] private float reboundPercent = 1.0f;
    [SerializeField] private Lamp lamp = null;
    [SerializeField] private float hpPercent = 0.85f;
    [SerializeField] private float atkPercent = 0.0f;
    [SerializeField] private float defPercent = 1.5f;
    [SerializeField] private int setPriority = 2;
    private bool LampAlreadySpawned => lamp != null;
    private StatusEffect PriorityStatus => Create.A_Status.Perm_PriorityUp(PriorityDifference);
    private StatusEffect ReboundStatus => Create.A_Status.Perm_FixedReboundDamage(reboundPercent);

    public override string Description
        => $"Summons a LAMP on a random Caster entity position. Enemies that attack this LAMP take {RoundToPercent(reboundPercent)}% of DMG Dealt.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (LampAlreadySpawned) return;
        var casterEntityPos = battleStage.casterEntityPositions.ToList();
        var positionAvailable = IdentifyAvailablePositions(casterEntityPos);
        int index = SelectRandomPositionIndex(positionAvailable, battleStage);
        GameObject lampObject = InstantiateSummon(casterEntityPos, index);
        if (lampObject == null) return;
        HandleBattleStage(battleStage, lampObject);
        HandleLamp(lampObject);
        
        ResetChargeAndEffectDuration();
        FreezeSkillCharge();
    }

    #region Private Methods

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
            if (currentIteration++ > 100) return -1;

            index = Random.Range(0, battleStage.casterEntityPositions.Length);
            placementFound = positionAvailable[index];
        }
        return index;
    }

    private GameObject InstantiateSummon(List<Transform> casterEntityPos, int index)
    {
        if (!casterEntityPos.ValidIndex(index)) return null;
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
        HandleSummonObject(lamp.GetBaseSummon);
        lamp.AddStatusEffect(ReboundStatus);
        lamp.AddStatusEffect(PriorityStatus);
        lamp.UpdateStatusEffects();
        Unit.SubscribeDeathEvent(NullLamp);
    }
    private void HandleSummonObject(SummonObject summonObject)
    {
        if (summonObject == null) return;
        summonObject.SetStatMultipliers(hpPercent, atkPercent, defPercent);
        summonObject.CalculateMaxStats(GetUnit);
    }
    private void NullLamp(Unit unit)
    {
        Unit.UnsubscribeDeathEvent(NullLamp);
        if (unit == lamp) lamp = null;
        UnfreezeSkillCharge();
    }
    private int PriorityDifference => (setPriority <= lamp.GetUnitPriority) ? 0 : setPriority - lamp.GetUnitPriority;

    #endregion

    public MothLamp(Unit unit)
    {
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }
    public MothLamp(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
