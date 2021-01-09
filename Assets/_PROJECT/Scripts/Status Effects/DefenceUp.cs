using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUp : StatusTemplate<DefenceUp>
{
    #region Variables and Properties

    private float _defenceUpPercent;
    public int DefenceUpAmount => Mathf.Abs(Round(GetUnit.GetBaseDefence * _defenceUpPercent));
    public override string StatusName => "Defence Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.AddCurrentDefence(DefenceUpAmount);

    #endregion

    public DefenceUp() : base()
    {
        _defenceUpPercent = 0.0f;
    }
    public DefenceUp(int turns, float baseResistance, bool isPermanent, float defUpPercent) : base(turns, baseResistance, isPermanent)
    {
        _defenceUpPercent = Mathf.Abs(defUpPercent);
    }
}