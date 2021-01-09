using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ReboundRateUpKinectist : StatusTemplate<ReboundRateUpKinectist>
{
    #region Variables and Properties

    private float _rateUpPercent;
    private float RateUpAmount => Mathf.Abs(GetUnit.GetProbability + _rateUpPercent);
    private float ResetRateUpAmount => Mathf.Abs(GetUnit.GetProbability - _rateUpPercent);
    public override string StatusName => "Kinectist Rebound Rate Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus()
    {
        if (GetUnit.GetBaseUnit.ArchTypeMajor != ArchTypeMajor.Kinectist) return;
        GetUnit.SetProbability(RateUpAmount);
    }
    protected override void Deinitialise()
    {
        GetUnit.SetProbability(ResetRateUpAmount);
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