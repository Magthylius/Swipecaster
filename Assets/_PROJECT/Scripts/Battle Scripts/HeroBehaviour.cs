using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{
    BattlestageManager BSM;
    [SerializeField]bool isHeld = false;

    void Start()
    {
        BSM = BattlestageManager.instance;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Hero") && isHeld == false)
        {
            BSM.CheckPosition(this.gameObject);
        }
    }

    #region Accessors

    public void SetIsHeld(bool cond) => isHeld = cond;

    #endregion

}
