using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Flame : StatusTemplate<Flame>
{
    #region Variables and Properties

    private float _maxHealthPercent;
    private float _attackDownPercent;
    public int HealthDownAmount => -Mathf.Abs(Round(_unit.GetMaxHealth * _maxHealthPercent));
    public int AttackDownAmount => -Mathf.Abs(Round(_unit.GetBaseAttack * _attackDownPercent));
    public override string StatusName => "Aflame";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.AddCurrentAttack(AttackDownAmount);
    public override void DoPostEffect()
    {
        _unit.AddCurrentHealth(HealthDownAmount);
        base.DoPostEffect();
    }

    #endregion

    public Flame() : base()
    {
        _maxHealthPercent = 0.0f;
        _attackDownPercent = 0.0f;
    }
    public Flame(int turns, float baseResistance, bool isPermanent, float maxHealthPercent, float atkDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _maxHealthPercent = Mathf.Abs(maxHealthPercent);
        _attackDownPercent = Mathf.Abs(atkDownPercent);
    }
}