using UnityEngine;

public class ReboundDamageUp : StatusTemplate<ReboundDamageUp>
{
    #region Variables and Properties

    private float _damageUpPercent;
    private float DamageUpAmount => Mathf.Abs(GetUnit.GetReboundPercent + _damageUpPercent);
    private float ResetDamageUpAmount => Mathf.Abs(GetUnit.GetReboundPercent - _damageUpPercent);
    public override string StatusName => "Rebound Damage Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.SetReboundPercent(DamageUpAmount);
    protected override void Deinitialise()
    {
        GetUnit.SetReboundPercent(ResetDamageUpAmount);
        base.Deinitialise();
    }

    #endregion

    public ReboundDamageUp() : base()
    {
        _damageUpPercent = 0.0f;
    }
    public ReboundDamageUp(int turns, float baseResistance, bool isPermanent, float damageUpPercent) : base(turns, baseResistance, isPermanent)
    {
        _damageUpPercent = Mathf.Abs(damageUpPercent);
    }
}