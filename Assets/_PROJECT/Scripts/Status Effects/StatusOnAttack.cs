public class StatusOnAttack : StatusTemplate<StatusOnAttack>
{
    #region Variables and Properties

    private StatusEffect _statusToApply;
    public StatusEffect StatusToApply => _statusToApply;
    public override string StatusName => $"Status On Attack: {StatusToApply.StatusName}.";

    #endregion

    #region Override Methods

    public override void DoEffectOnAction(TargetInfo info, int totalDamage)
    {
        info.Focus.AddStatusEffect(StatusToApply);
        info.Collateral.ForEach(u => u.AddStatusEffect(StatusToApply));
    }

    #endregion

    public StatusOnAttack() : base()
    {
        _statusToApply = new NullStatus();
    }
    public StatusOnAttack(int turns, float baseResistance, bool isPermanent, StatusEffect statusToApply) : base(turns, baseResistance, isPermanent)
    {
        _statusToApply = statusToApply;
    }
}