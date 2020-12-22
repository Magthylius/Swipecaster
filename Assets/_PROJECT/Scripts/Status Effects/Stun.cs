﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    #region Variables and Properties

    private TurnBaseManager _turnBaseManager;
    public override string StatusName => "Stunned";

    #endregion

    #region Override Methods

    public override void DoPreEffect(Unit target)
    {
        if (_turnBaseManager == null) return;

        // end turn code
    }
    public override void DoEffectOnAction(Unit target) { }
    public override void DoOnHitEffect(Unit target) { }
    public override void DoPostEffect(Unit target) => DeductRemainingTurns();
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<Stun>().Count();

    #endregion

    public Stun() : base()
    {
        _turnBaseManager = null;
    }
    public Stun(int turns, float baseResistance, bool isPermanent, TurnBaseManager turnBase) : base(turns, baseResistance, isPermanent)
    {
        _turnBaseManager = turnBase;
    }
}