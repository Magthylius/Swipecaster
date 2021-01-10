using UnityEngine;

[System.Serializable]
public class SecondScar : CasterSkill
{
    [SerializeField] private int effectTurns = 3;
    [SerializeField] private float atkUpPercent = 0.2f;
    private StatusEffect AttackUpStatus => Create.A_Status.AttackUp(effectTurns, atkUpPercent);
    private StatusEffect UndyingStatus => Create.A_Status.Ununaliving(effectTurns);

    public override string Description
        => $"Attack +{RoundToPercent(atkUpPercent)}%. " +
           $"HP will not drop below 1 for the duration of the skill.";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        GetUnit.AddStatusEffect(AttackUpStatus);
        GetUnit.AddStatusEffect(UndyingStatus);
        ResetSkillCharge();
    }

    public SecondScar(Unit unit)
    {
        _startEffectDuration = 3;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public SecondScar(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}