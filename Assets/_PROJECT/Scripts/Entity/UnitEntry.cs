using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntry : Entity
{
    public int UnitID => Convert.ToInt32(GetBaseUnit.ID);
}
