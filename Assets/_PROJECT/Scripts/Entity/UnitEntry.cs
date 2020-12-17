using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntry : Entity
{
    public UnitObject BaseUnit => baseUnit;
    public int UnitID => Convert.ToInt32(baseUnit.ID);
}
