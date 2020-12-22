using System;
using System.Collections.Generic;
using UnityEngine;

public class Antagomancer : Conjuror
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.Antagomancer);
    }

    #endregion
}