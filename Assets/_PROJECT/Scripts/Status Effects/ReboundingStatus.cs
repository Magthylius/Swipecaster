public class ReboundingStatus : StatusTemplate<ReboundingStatus>
{
    #region Variables and Properties

    private StatusEffect _reboundingStatus;
    private StatusEffect StatusToRebound => _reboundingStatus;
    public override string StatusName => $"Rebounding Status: {StatusToRebound.StatusName}.";

    #endregion

    #region Override Methods

    public override void DoOnHitEffect(Unit damager, TargetInfo info, int totalDamage)
    {
        if (damager == null) return;
        damager.AddStatusEffect(StatusToRebound);
    }

    #endregion

    public ReboundingStatus() : base()
    {
        _reboundingStatus = new NullStatus();
    }
    public ReboundingStatus(int turns, float baseResistance, bool isPermanent, StatusEffect reboundingStatus) : base(turns, baseResistance, isPermanent)
    {
        _reboundingStatus = reboundingStatus;
    }
}