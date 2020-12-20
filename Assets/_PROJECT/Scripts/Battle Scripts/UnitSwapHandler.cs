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
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Hero") && isHeld == false)
        {
            unitPositionManager.CheckPosition(this.gameObject);
        }
    }
    
    #region Accessors

    public void SetIsHeld(bool cond) => isHeld = cond;

    #endregion
    
}
