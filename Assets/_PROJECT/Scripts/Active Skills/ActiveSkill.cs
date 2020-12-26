using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill
{
    private float _skillDamageMultiplier = 1.0f;
    private int _effectDuration = 0;
    private int _maxSkillCharge = 0;
    private int _chargeGainPerTurn = 0;

    public abstract void TriggerSkill(TargetInfo targetInfo, StageInfo stageInfo);
    public abstract TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes);

    protected int Round(float number) => Mathf.RoundToInt(number);

    public float GetSkillDamageMultiplier => _skillDamageMultiplier;
    public int GetSkillEffectDuration => _effectDuration;
    public int GetMaxSkillCharge => _maxSkillCharge;
    public int GetChargeGainPerTurn => _chargeGainPerTurn;

    public ActiveSkill()
    {
        _skillDamageMultiplier = 1.0f;
        _effectDuration = 0;
        _maxSkillCharge = 0;
        _chargeGainPerTurn = 0;
    }
    public ActiveSkill(float damageMultiplier, int effectDuration, int maxSkillCharge, int chargeGainPerTurn)
    {
        _skillDamageMultiplier = Mathf.Abs(damageMultiplier);
        _effectDuration = Mathf.Abs(effectDuration);
        _maxSkillCharge = Mathf.Abs(maxSkillCharge);
        _chargeGainPerTurn = Mathf.Abs(chargeGainPerTurn);
    }
}
