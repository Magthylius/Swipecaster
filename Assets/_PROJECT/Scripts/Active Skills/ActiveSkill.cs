using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill
{
    private float _skillDamageMultiplier = 1.0f;

    public abstract void TriggerSkill(TargetInfo info, List<Unit> allCasters, List<Unit> allFoes);
    public abstract TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes);

    protected int Round(float number) => Mathf.RoundToInt(number);

    public void SetSkillDamageMultiplier(float multiplier) => _skillDamageMultiplier = multiplier;
    public float GetSkillDamageMultiplier => _skillDamageMultiplier;

    public ActiveSkill() => _skillDamageMultiplier = 1.0f;
    public ActiveSkill(float damageMultiplier) => _skillDamageMultiplier = Mathf.Abs(damageMultiplier);
}
