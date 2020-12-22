using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryOverhead : Artillery
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetProjectile(new SplashOverhead());
        SetArchMinor(ArchTypeMinor.Artillery_Overhead);
    }

    #endregion
}