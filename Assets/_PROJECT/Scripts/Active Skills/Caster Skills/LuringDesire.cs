using UnityEngine;

[System.Serializable]
public class LuringDesire : CasterSkill
{
    [SerializeField] private int effectTurns = 1;
    [SerializeField] private int priorityUpAmount = 99;
    [SerializeField] private float healPercent = 0.75f;
    private StatusEffect PriorityUpStatus => Create.A_Status.PriorityUp(effectTurns, priorityUpAmount);
    private StatusEffect HealOnDamagedStatus => Create.A_Status.DamageToDistributedPartyHeal(effectTurns, healPercent);

    public override string Description
        => $"Enemies focus on this caster. Heals team by {RoundToPercent(healPercent)}% distributed of all damage taken.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(PriorityUpStatus);
        GetUnit.AddStatusEffect(HealOnDamagedStatus);
        ResetChargeAndEffectDuration();
    }

    public LuringDesire(Unit unit)
    {
        _startEffectDuration = 1;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public LuringDesire(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}