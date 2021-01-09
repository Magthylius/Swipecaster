using UnityEngine;

public class FixedReboundDamage : StatusTemplate<FixedReboundDamage>
{
    #region Variables and Properties

    private float _reboundPercent;
    private float FixedReboundPercent => _reboundPercent;
    public override string StatusName => "Rebound Damage Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.SetReboundPercent(FixedReboundPercent);
    protected override void Deinitialise()
    {
        GetUnit.ResetReboundPercent();
        base.Deinitialise();
    }

    #endregion

    public FixedReboundDamage() : base()
    {
        _reboundPercent = 0.0f;
    }
    public FixedReboundDamage(int turns, float baseResistance, bool isPermanent, float fixedReboundPercent) : base(turns, baseResistance, isPermanent)
    {
        _reboundPercent = Mathf.Abs(fixedReboundPercent);
    }
}