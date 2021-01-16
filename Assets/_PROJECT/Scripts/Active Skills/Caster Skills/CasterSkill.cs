using ConversionFunctions;

public abstract class CasterSkill : ActiveSkill
{
    public CasterSkill() : base() { }
    public CasterSkill(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
    {
        _startEffectDuration = startEffectDuration;
        _maxSkillCharge = maxSkillCharge;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = ignoreDuration;
        _unit = unit;
        ResetChargeAndEffectDuration();
        EffectDuration0();
    }

    public override string Name => this.NameOfClass().AddSpacesBeforeCapitalLetters(false);
    public override TargetInfo GetActiveSkillTargets(TargetInfo targetInfo) => targetInfo;
    public override void TurnStartCall() { }
    protected override void OnEffectDurationComplete() { }
}