using ConversionFunctions;
using Obtain;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DrumBeater : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    [SerializeField] private float attackPercent = 0.35f;
    [SerializeField] private BeatDrum beatDrum = null;
    private bool BeatDrumAlreadySpawned => beatDrum != null;
    private StatusEffect BeatDrumStatus => Create.A_Status.BeatDrumEffect(effectTurns, BattlestageManager.instance);

    public override string Description
        => $"Summons a BEAT DRUM in a random Enemy entity position. The BEAT DRUM does {RoundToPercent(attackPercent)}% of Rhythms' ATK " +
           $"per turn, adjacent enemies recieve a permanent WEAKENED status until BEAT DRUM dies ({effectTurns} turns " +
           $"to death).";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (BeatDrumAlreadySpawned) return;
        var enemyEntityPos = battleStage.enemyEntityPositions.ToList();
        var positionAvailable = IdentifyAvailablePositions(enemyEntityPos);
        int index = SelectRandomPositionIndex(positionAvailable, battleStage);
        GameObject drumObject = InstantiateSummon(enemyEntityPos, index);
        if (drumObject == null) return;
        HandleBattleStage(battleStage, drumObject);
        HandleBeatDrum(drumObject);

        ResetChargeAndEffectDuration();
        FreezeSkillCharge();
    }

    #region Private Methods

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
    private List<bool> IdentifyAvailablePositions(List<Transform> enemyEntityPos)
    {
        List<bool> positionAvailable = new List<bool>();
        for (int i = 0; i < enemyEntityPos.Count; i++)
        {
            var p = enemyEntityPos[i];
            positionAvailable.Add(p.childCount != 0);
        }
        return positionAvailable;
    }
    private GameObject InstantiateSummon(List<Transform> enemyEntityPos, int index)
    {
        if (!enemyEntityPos.ValidIndex(index)) return null;
        enemyEntityPos[index].gameObject.SetActive(true);
        GameObject drumObject = GetUnit.GetBaseUnit.InstantiateSummon(enemyEntityPos[index].position, Quaternion.identity, enemyEntityPos[index]);
        return drumObject;
    }
    private static void HandleBattleStage(BattlestageManager battleStage, GameObject drumObject)
    {
        battleStage.GetEnemyEntityTeam().Add(drumObject);
        battleStage.RegroupLeftPositions(false);
    }
    private void HandleBeatDrum(GameObject drumObject)
    {
        beatDrum = drumObject.AsType<BeatDrum>();
        if (beatDrum == null) return;
        HandleSummonObject(beatDrum.GetBaseSummon);
        beatDrum.AddStatusEffect(BeatDrumStatus);
        beatDrum.UpdateStatusEffects();
        Unit.SubscribeDeathEvent(NullDrum);
    }
    private void HandleSummonObject(SummonObject summonObject)
    {
        if (summonObject == null) return;
        summonObject.SetAttackMultiplier(attackPercent);
        summonObject.CalculateMaxStats(GetUnit);
    }
    private void NullDrum(Unit unit)
    {
        Unit.UnsubscribeDeathEvent(NullDrum);
        if (unit == beatDrum) beatDrum = null;
        UnfreezeSkillCharge();
    }

    #endregion

    public DrumBeater(Unit unit)
    {
        _startEffectDuration = 0;
        _maxSkillCharge = 4;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public DrumBeater(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}