using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CasterSkill : ActiveSkill
{
    public CasterSkill() : base() { }
    public CasterSkill(float damageMultiplier, int effectDuration, int maxSkillCharge, int chargeGainPerTurn)
        : base(damageMultiplier, effectDuration, maxSkillCharge, chargeGainPerTurn) { }
}
