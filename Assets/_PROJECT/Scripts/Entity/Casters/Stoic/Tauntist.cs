using System;
using System.Collections.Generic;
using UnityEngine;

public class Tauntist : Stoic
{
    //! this has higher 'priority' than usual units

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();

        SetArchMinor(ArchTypeMinor.Tauntist);
    }

    #endregion
}