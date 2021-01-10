using System;
using System.Collections.Generic;
using UnityEngine;

public class Volleyer : Pinpoint
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetDefaultProjectile(new Dual());
        SetArchMinor(ArchTypeMinor.Volleyer);
    }

    #endregion
}