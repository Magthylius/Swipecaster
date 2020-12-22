using System;
using System.Collections.Generic;
using UnityEngine;

public class Modal : Enhancer
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.Modal);
    }

    #endregion
}