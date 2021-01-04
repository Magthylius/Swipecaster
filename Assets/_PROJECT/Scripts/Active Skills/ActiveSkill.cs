using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill
{
    protected float _skillDamageMultiplier = 1.0f;
    protected int _startEffectDuration = 0;
    protected int _currentEffectDuration = 0;
    protected int _maxSkillCharge = 0;
    protected int _currentSkillCharge = 0;
    protected int _chargeGainPerTurn = 0;
    protected Unit _unit = null;

    protected bool _ignoreDuration = false;

    public abstract void TurnStartCall();
    public abstract void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage);
    public abstract TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes);
    protected abstract void OnEffectDurationComplete();
    public virtual void TurnEndCall()
    {
        DeductEffectDuration();
        IncreaseSkillCharge();
    }

    protected int Round(float number) => Mathf.RoundToInt(number);

    public void SetSkillDamageMultiplier(float a) => _skillDamageMultiplier = a;
    public float GetSkillDamageMultiplier => _skillDamageMultiplier;
    public void SetSkillEffectDuration(int a) => _startEffectDuration = a;
    public int GetSkillEffectDuration => _startEffectDuration;
    public void SetCurrentSkillEffectDuration(int a) => _currentEffectDuration = a;
    public int GetCurrentSkillEffectDuration => _currentEffectDuration;
    public void DeductEffectDuration()
    {
        _currentEffectDuration = Mathf.Clamp(_currentEffectDuration - 1, 0, _startEffectDuration);
        if (_currentEffectDuration > 0 || _ignoreDuration) return;
        OnEffectDurationComplete();
    }
    public void SetMaxSkillCharge(int a) => _maxSkillCharge = a;
    public int GetMaxSkillCharge => _maxSkillCharge;
    public void SetChargeGainPerTurn(int a) => _chargeGainPerTurn = a;
    public int GetChargeGainPerTurn => _chargeGainPerTurn;
    public int GetCurrentSkillCharge => _currentSkillCharge;
    public void IncreaseSkillCharge() => _currentSkillCharge += (SkillChargeReady || !EffectDurationReached) ? 0 : _chargeGainPerTurn;
    public void SetUnit(Unit a) => _unit = a;
    public Unit GetUnit => _unit;
    public virtual string Description => string.Empty;

    public void ResetSkillCharge() => _currentSkillCharge = 0;
    public void ResetEffectDuration() => _currentEffectDuration = _startEffectDuration;
    public void EffectDuration0() => _currentEffectDuration = 0;

    public ActiveSkill()
    {
        _skillDamageMultiplier = 1.0f;
        _startEffectDuration = 0;
        _maxSkillCharge = 0;
        _chargeGainPerTurn = 0;
        _ignoreDuration = false;
        _unit = null;
    }

    public bool SkillChargeReady => _currentSkillCharge >= _maxSkillCharge;
    public bool EffectDurationReached => _currentEffectDuration <= 0;
}
