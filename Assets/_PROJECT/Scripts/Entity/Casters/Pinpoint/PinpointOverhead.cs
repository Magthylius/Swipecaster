using System;
using System.Collections.Generic;
using UnityEngine;

public class PinpointOverhead : Pinpoint
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetProjectile(new Overhead());
        SetArchMinor(ArchTypeMinor.Pinpoint_Overhead);
    }

    #endregion
}