using System;
using System.Collections.Generic;
using UnityEngine;

public class PinpointPiercer : Pinpoint
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetProjectile(new Piercing());
        SetArchMinor(ArchTypeMinor.Pinpoint_Piercer);
    }

    #endregion
}