using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : StatusEffect
{
    #region Variables and Properties

    private float _damageMultiplier;
    public float DamageMultiplier => Mathf.Abs(_damageMultiplier);
    public override string StatusName => "Weakened";

    #endregion

    #region Override Methods

    public override void DoPreEffect(Entity target) { }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();
    public override float GetStatusDamageInModifier() => DamageMultiplier;
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<Weakness>().Count();

    #endregion

    public Weakness() : base()
    {
        _damageMultiplier = 0.0f;
    }
    public Weakness(int turns, float baseResistance, bool isPermanent, float damageMultiplier) : base(turns, baseResistance, isPermanent)
    {
        _damageMultiplier = damageMultiplier;
    }
}