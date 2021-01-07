using UnityEngine;

public class DamageTakenUp : EmptyStatus<DamageTakenUp>
{
    #region Variables and Properties

    private float _damageTakenUpPercent;
    public float DamageTakenUpPercent => Mathf.Abs(_damageTakenUpPercent);
    public override string StatusName => "Damage Taken Up";

    #endregion

    #region Override Methods

    public override float GetStatusDamageInModifier() => DamageTakenUpPercent;

    #endregion

    public DamageTakenUp() : base()
    {
        _damageTakenUpPercent = 0.0f;
    }
    public DamageTakenUp(int turns, float baseResistance, bool isPermanent, float damageTakenUpPercent) : base(turns, baseResistance, isPermanent)
    {
        _damageTakenUpPercent = Mathf.Abs(damageTakenUpPercent);
    }
}