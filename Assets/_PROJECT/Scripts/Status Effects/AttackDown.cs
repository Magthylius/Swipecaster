using UnityEngine;

public class AttackDown : StatusTemplate<AttackDown>
{
    #region Variables and Properties

    private float _attackDownPercent;
    public int AttackDownAmount => -Mathf.Abs(Round(GetUnit.GetBaseAttack * _attackDownPercent));
    public override string StatusName => "ATK Down";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.AddCurrentAttack(AttackDownAmount);

    #endregion

    public AttackDown() : base()
    {
        _attackDownPercent = 0.0f;
    }
    public AttackDown(int turns, float baseResistance, bool isPermanent, float atkDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _attackDownPercent = Mathf.Abs(atkDownPercent);
    }
}