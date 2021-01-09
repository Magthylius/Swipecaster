using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusTemplate<Poison>
{
    #region Variables and Properties

    private float _maxHealthPercent;
    private float _defenceDownPercent;
    public int HealthDownAmount => -Mathf.Abs(Round(GetUnit.GetMaxHealth * _maxHealthPercent));
    public int DefenceDownAmount => -Mathf.Abs(Round(GetUnit.GetBaseDefence * _defenceDownPercent));
    public override string StatusName => "Poisoned";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.AddCurrentDefence(DefenceDownAmount);
    public override void DoPostEffect()
    {
        GetUnit.AddCurrentHealth(HealthDownAmount);
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