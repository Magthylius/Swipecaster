using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CasterSkill : ActiveSkill
{
    public CasterSkill() : base() { }

    public override void TurnStartCall() { }
    protected override void OnEffectDurationComplete() { }
}
