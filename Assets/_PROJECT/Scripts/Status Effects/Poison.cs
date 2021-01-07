﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Poison : EmptyStatus<Poison>
{
    #region Variables and Properties

    private float _maxHealthPercent;
    private float _defenceDownPercent;
    public int HealthDownAmount => -Mathf.Abs(Round(_unit.GetMaxHealth * _maxHealthPercent));
    public int DefenceDownAmount => -Mathf.Abs(Round(_unit.GetBaseDefence * _defenceDownPercent));
    public override string StatusName => "Poisoned";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.AddCurrentDefence(DefenceDownAmount);
    public override void DoPostEffect()
    {
        _unit.AddCurrentHealth(HealthDownAmount);
        base.DoPostEffect();
    }

    #endregion

    public Poison() : base()
    {
        _maxHealthPercent = 0.0f;
        _defenceDownPercent = 0.0f;
    }
    public Poison(int turns, float baseResistance, bool isPermanent, float maxHealthPercent, float defDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _maxHealthPercent = Mathf.Abs(maxHealthPercent);
        _defenceDownPercent = Mathf.Abs(defDownPercent);
    }
}