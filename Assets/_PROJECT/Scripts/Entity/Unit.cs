using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Unit : Entity
{
    public UnitObject BaseUnit => baseUnit;


    private void Awake()
    {
        
    }

    #region Shorthands

    public float GetAttack => _totalAttack;
    public float GetDefence => _totalDefence;
    
    #endregion
}
