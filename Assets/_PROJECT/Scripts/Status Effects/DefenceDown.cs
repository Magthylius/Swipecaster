using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DefenceDown : StatusTemplate<DefenceDown>
{
    #region Variables and Properties

    private float _defenceDownPercent;
    public int DefenceDownAmount => -Mathf.Abs(Round(_unit.GetBaseDefence * _defenceDownPercent));
    public override string StatusName => "Vulnerable";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.AddCurrentDefence(DefenceDownAmount);

    #endregion

    public DefenceDown() : base()
    {
        _defenceDownPercent = 0.0f;
    }
    public DefenceDown(int turns, float baseResistance, bool isPermanent, float defDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _defenceDownPercent = Mathf.Abs(defDownPercent);
    }
}