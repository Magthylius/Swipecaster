using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : StatusTemplate<AttackUp>
{
    #region Variables and Properties

    private float _attackUpPercent;
    public int AttackUpAmount => Mathf.Abs(Round(_unit.GetBaseAttack * _attackUpPercent));
    public override string StatusName => "ATK Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.AddCurrentAttack(AttackUpAmount);

    #endregion

    public AttackUp() : base()
    {
        _attackUpPercent = 0.0f;
    }
    public AttackUp(int turns, float baseResistance, bool isPermanent, float atkUpPercent) : base(turns, baseResistance, isPermanent)
    {
        _attackUpPercent = Mathf.Abs(atkUpPercent);
    }
}