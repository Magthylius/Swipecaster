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
        EffectDuration0();
    }

    public override TargetInfo GetActiveSkillTargets(TargetInfo targetInfo) => targetInfo;
    public override void TurnStartCall() { }
    protected override void OnEffectDurationComplete() { }
}
