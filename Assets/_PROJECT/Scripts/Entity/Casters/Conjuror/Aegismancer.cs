using System;
using System.Collections.Generic;
using UnityEngine;

public class Aegismancer : Conjuror
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.Aegismancer);
    }

    #endregion
}