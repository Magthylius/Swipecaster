using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSwapHandler : MonoBehaviour
{
    UnitPositionManager unitPositionManager;
    bool isHeld = false;
    
    void Start()
    {
        unitPositionManager = UnitPositionManager.instance;
    }
    

    #region Accessors

    public void SetIsHeld(bool cond) => isHeld = cond;

    #endregion
    
}
