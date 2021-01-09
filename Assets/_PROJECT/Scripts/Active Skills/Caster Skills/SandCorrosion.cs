using System.Linq;
using UnityEngine;

[System.Serializable]
public class SandCorrosion : CasterSkill
{
    [SerializeField] private int statusOnAttackEffectTurns = 1;
    [SerializeField] private int corrosionEffectTurns = 2;
    [SerializeField] private int corrosionEffectStacks = 2;

    private StatusEffect CorrosionStatus => Create.A_Status.Corrosion(corrosionEffectTurns, corrosionEffectStacks);
    private StatusEffect StatusOnAttackStatus => Create.A_Status.StatusOnAttack(statusOnAttackEffectTurns, CorrosionStatus);

    public override string Description
        => $"All enemies hit by {GetUnit.GetBaseUnit.CharacterName} gains a "
         + $"{corrosionEffectStacks} stacking {corrosionEffectTurns} turn VULNERABLE status.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(StatusOnAttackStatus);
        ResetSkillCharge();
    }

    public SandCorrosion(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 3;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }

    public SandCorrosion(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}