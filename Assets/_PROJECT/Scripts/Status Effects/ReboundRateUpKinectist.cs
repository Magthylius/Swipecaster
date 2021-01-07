using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ReboundRateUpKinectist : EmptyStatus<ReboundRateUpKinectist>
{
    #region Variables and Properties

    private float _rateUpPercent;
    private float RateUpAmount => Mathf.Abs(_unit.GetProbability + _rateUpPercent);
    private float ResetRateUpAmount => Mathf.Abs(_unit.GetProbability - _rateUpPercent);
    public override string StatusName => "Kinectist Rebound Rate Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus()
    {
        if (_unit.GetBaseUnit.ArchTypeMajor != ArchTypeMajor.Kinectist) return;
        _unit.SetProbability(RateUpAmount);
    }
    protected override void Deinitialise()
    {
        _unit.SetProbability(ResetRateUpAmount);
        base.Deinitialise();
    }

    #endregion

    public ReboundRateUpKinectist() : base()
    {
        _rateUpPercent = 0.0f;
    }
    public ReboundRateUpKinectist(int turns, float baseResistance, bool isPermanent, float rateUpPercent) : base(turns, baseResistance, isPermanent)
    {
        _rateUpPercent = Mathf.Abs(rateUpPercent);
    }
}