using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill
{
    protected float _skillDamageMultiplier = 1.0f;
    protected int _effectDuration = 0;
    protected int _maxSkillCharge = 0;
    protected int _chargeGainPerTurn = 0;
    protected Unit _unit = null;

    public abstract void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage);
    public abstract TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes);

    protected int Round(float number) => Mathf.RoundToInt(number);

    public void SetSkillDamageMultiplier(float a) => _skillDamageMultiplier = a;
    public float GetSkillDamageMultiplier => _skillDamageMultiplier;
    public void SetSkillEffectDuration(int a) => _effectDuration = a;
    public int GetSkillEffectDuration => _effectDuration;
    public void SetMaxSkillCharge(int a) => _maxSkillCharge = a;
    public int GetMaxSkillCharge => _maxSkillCharge;
    public void SetChargeGainPerTurn(int a) => _chargeGainPerTurn = a;
    public int GetChargeGainPerTurn => _chargeGainPerTurn;
    public void SetUnit(Unit a) => _unit = a;
    public Unit GetUnit => _unit;
    public virtual string Description => string.Empty;

    public ActiveSkill()
    {
        _skillDamageMultiplier = 1.0f;
        _effectDuration = 0;
        _maxSkillCharge = 0;
        _chargeGainPerTurn = 0;
        _unit = null;
    }
    public ActiveSkill(float damageMultiplier, int effectDuration, int maxSkillCharge, int chargeGainPerTurn, Unit unit)
    {
        _skillDamageMultiplier = Mathf.Abs(damageMultiplier);
        _effectDuration = Mathf.Abs(effectDuration);
        _maxSkillCharge = Mathf.Abs(maxSkillCharge);
        _chargeGainPerTurn = Mathf.Abs(chargeGainPerTurn);
        _unit = unit;
    }
}
