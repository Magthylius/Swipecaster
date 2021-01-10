using System;
using System.Collections.Generic;
using UnityEngine;

public class Howitzer : Artillery
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        
        SetDefaultProjectile(new Overhead());
        SetArchMinor(ArchTypeMinor.Howitzer);
    }

    #endregion
}