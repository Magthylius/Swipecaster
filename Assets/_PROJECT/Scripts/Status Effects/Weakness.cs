using UnityEngine;

public class Weakness : EmptyStatus<Weakness>
{
    #region Variables and Properties

    private float _damageMultiplier;
    public float DamageMultiplier => _damageMultiplier;
    public override string StatusName => "Weakened";

    #endregion

    #region Override Methods

    public override float GetStatusDamageInModifier() => DamageMultiplier;
    
    #endregion

    public Weakness() : base()
    {
        _damageMultiplier = 0.0f;
    }
    public Weakness(int turns, float baseResistance, bool isPermanent, float damageMultiplier) : base(turns, baseResistance, isPermanent)
    {
        _damageMultiplier = Mathf.Abs(damageMultiplier);
    }
}