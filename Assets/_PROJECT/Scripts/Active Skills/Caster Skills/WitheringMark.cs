using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WitheringMark : CasterSkill
{
    [SerializeField] private int effectTurns = 2;
    [SerializeField] private int stackCount = 3;
    [SerializeField] private int targetCount = 2;
    private StatusEffect StatusToApply => Create.A_Status.Vulnerability(effectTurns);

    public override string Description
        => $"Randomly hits {targetCount} enemies. Each enemy hit gets a 3 stack 2 turns VULNERABLE status.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        var hitTargets = SkillAttackTargets(targetInfo);
        ApplyStatusOnHitTargets(hitTargets);
        ResetSkillCharge();
    }

    private void ApplyStatusOnHitTargets(List<Unit> hitTargets)
    {
        for (int i = 0; i < stackCount; i++) hitTargets.ForEach(hit => hit.AddStatusEffect(StatusToApply));
    }

    private List<Unit> SkillAttackTargets(TargetInfo targetInfo)
    {
        int damage = GetUnitDamage();
        var targets = GetTargets(targetInfo);
        targets.ForEach(target => target.TakeHit(GetUnit, damage));
        return targets;
    }

    private int GetUnitDamage() => GetUnit.CalculateDamage(TargetInfo.Null, RuneCollection.Null);
    private List<Unit> GetTargets(TargetInfo info)
    {
        var list = new List<Unit>();
        if (info.Foes.Count == 0) return list;
        else if (info.Foes.Count == 1 || info.Foes.Count == 2)
        {
            info.Foes.ForEach(foe => list.Add(foe));
            return list;
        }

        while(list.Count < targetCount)
        {
            var unit = info.Foes[Random.Range(0, info.Foes.Count)];
            if (list.Contains(unit)) continue;
            list.Add(unit);
        }
        return list;
    }

    public WitheringMark(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }

    public WitheringMark(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}