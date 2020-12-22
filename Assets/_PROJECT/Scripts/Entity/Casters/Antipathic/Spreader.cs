using System;
using System.Collections.Generic;
using UnityEngine;

public class Spreader : Antipathic
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        
        SetProjectile(new Blast());
        SetArchMinor(ArchTypeMinor.Spreader);
    }

    #endregion
}