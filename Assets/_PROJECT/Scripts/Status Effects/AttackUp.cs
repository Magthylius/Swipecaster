using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : StatusEffect
{
    #region Variables and Properties

    private float _attackUpPercent;
    public float AttackUpPercent => Mathf.Abs(_attackUpPercent);
    public override string StatusName => "ATK Up";

    #endregion

    #region Override Methods

    public override void DoPreEffect(Unit target)
    {
        if (ShouldClear()) return;
        int attackToAdd = Mathf.Abs(Round(target.GetBaseAttack * AttackUpPercent));
        target.AddCurrentAttack(attackToAdd);
    }
    public override void DoEffectOnAction(Unit target) { }
    public override void DoOnHitEffect(Unit target) { }
    public override void DoPostEffect(Unit target) => DeductRemainingTurns();
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<AttackUp>().Count();

    #endregion

    public AttackUp() : base()
    {
        _attackUpPercent = 0.0f;
    }
    public AttackUp(int turns, float baseResistance, bool isPermanent, float atkUpPercent) : base(turns, baseResistance, isPermanent)
    {
        _attackUpPercent = Mathf.Clamp01(atkUpPercent);
    }
}