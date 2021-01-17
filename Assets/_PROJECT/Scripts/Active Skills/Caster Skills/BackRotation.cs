using System.Linq;
using UnityEngine;

[System.Serializable]
public class BackRotation : CasterSkill
{
    [SerializeField] private int effectTurns = 4;
    private StatusEffect ProjectileStatus => Create.A_Status.ProjectileLocker(effectTurns, new CrowFlies(GetUnit));
    private StatusEffect StunStatus => Create.A_Status.Stun(effectTurns, TurnBaseManager.instance);
    
    public override string Description
        => $"Attack turns default. Pushes enemy back 1 position and apply STUN.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(ProjectileStatus);
        FindAndActOnTarget(targetInfo, battleStage);
        ResetChargeAndEffectDuration();
    }

    private void FindAndActOnTarget(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        var enemyTransforms = battleStage.enemyPositions.ToList();
        var enemyPos = GetUnitsFromTransformChild0(enemyTransforms);
        var target = FindTarget(targetInfo, battleStage);
        int index = enemyPos.IndexOf(target) + 1;
        var parent = enemyTransforms[index];
        if (!WithinRange(index, enemyPos) || !HasEmptySpace(parent)) return;
        HandleTarget(target, parent);
    }

    private static Unit FindTarget(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        var target = targetInfo.Focus;
        if (target == null) target = FindRandomTarget(battleStage);
        return target;
    }

    private void HandleTarget(Unit target, Transform parent)
    {
        target.transform.SetParent(null);
        target.transform.SetPositionAndRotation(parent.position, Quaternion.identity);
        target.transform.SetParent(parent);
        target.AddStatusEffect(StunStatus);
    }

    private static bool HasEmptySpace(Transform unitTransform) => unitTransform.childCount == 0;

    private static Unit FindRandomTarget(BattlestageManager battleStage)
    {
        Unit target;
        int randomIndex = Random.Range(0, battleStage.GetEnemyTeamAsUnit().Count);
        target = battleStage.GetEnemyTeamAsUnit().ElementAt(randomIndex);
        return target;
    }

    public BackRotation(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public BackRotation(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}