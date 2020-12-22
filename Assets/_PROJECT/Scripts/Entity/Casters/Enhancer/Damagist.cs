using System;
using System.Collections.Generic;
using UnityEngine;

public class Damagist : Enhancer
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.Damagist);
    }

    #endregion
}