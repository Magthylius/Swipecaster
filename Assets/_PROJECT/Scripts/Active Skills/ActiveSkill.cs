using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActiveSkill
{
    #region Variable Definitions

    protected float _skillDamageMultiplier = 1.0f;
    protected int _startEffectDuration = 0;
    protected int _currentEffectDuration = 0;
    protected int _maxSkillCharge = 0;
    protected int _currentSkillCharge = 0;
    protected int _chargeGainPerTurn = 0;
    protected Unit _unit = null;
    protected bool _freezeSkillCharge = false;

    #endregion

    #region Abstract & Virtual Methods

    public abstract void TurnStartCall();
    public abstract void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage);
    public abstract TargetInfo GetActiveSkillTargets(TargetInfo targetInfo);
    protected abstract void OnEffectDurationComplete();
    public virtual void TurnEndCall()
    {
        DeductEffectDuration();
        IncreaseSkillCharge();
    }

    #endregion

    #region Protected Methods

    protected static string OneDecimal(float number) => number.ToString("0.0");
    protected int Round(float number) => Mathf.RoundToInt(number);
    protected int RoundToPercent(float number) => Round(number * 100);
    protected static List<Unit> GetUnitsFromTransformChild0(List<Transform> unitTransforms)
    {
        var list = new List<Unit>();
        foreach (var u in unitTransforms)
        {
            if (u.childCount == 0)
            {
                list.Add(null);
                continue;
            }
            list.Add(u.GetChild(0).GetComponent<Unit>());
        }
        return list;
    }
    protected static bool UnitFound(Unit unit) => unit != null;
    protected static bool WithinRange<T>(int index, List<T> list) => index >= 0 && index < list.Count;
    protected int AssignDamage(TargetInfo info, int damage) => GetUnit.GetCurrentProjectile.AssignTargetDamage(GetUnit, info, damage);
    protected TargetInfo GetFocusTarget(Unit target) => new TargetInfo(target, new List<Unit>(), new List<Unit>());
    protected TargetInfo GetFocusAndGrazed(Unit target, List<Unit> grazed) => new TargetInfo(target, new List<Unit>(), grazed);
    protected string GetCharacterName => UnitFound(GetUnit) ? GetUnit.GetEntityName : CachedCharacterName;

    #endregion

    #region Public Methods

    public void DeductEffectDuration()
    {
        _currentEffectDuration = Mathf.Clamp(_currentEffectDuration - 1, 0, _startEffectDuration);
        if (_freezeSkillCharge || _currentEffectDuration != 0) return;
        OnEffectDurationComplete();
    }
    public int GetMaxSkillCharge => _maxSkillCharge;
    public int GetCurrentSkillCharge => _currentSkillCharge;
    public void IncreaseSkillCharge()
    {
        if (SkillChargeReady || !EffectDurationReached) return;
        _currentSkillCharge += _chargeGainPerTurn;
    }
    public void SetUnit(Unit a) => _unit = a;
    public Unit GetUnit => _unit;
    public virtual string Description => string.Empty;
    public virtual string Name => string.Empty;
    public string CachedCharacterName { get => string.Empty; set => CachedCharacterName = value; }

    public bool SkillChargeReady => _currentSkillCharge >= _maxSkillCharge;
    public bool EffectDurationReached => _currentEffectDuration <= 0 && !_freezeSkillCharge;
    public void FreezeSkillCharge() => _freezeSkillCharge = true;
    public void UnfreezeSkillCharge() => _freezeSkillCharge = false;
    public void ResetSkillCharge() => _currentSkillCharge = 0;
    public void ResetEffectDuration() => _currentEffectDuration = _startEffectDuration;
    public void EffectDuration0() => _currentEffectDuration = 0;
    public void ResetChargeAndEffectDuration()
    {
        ResetSkillCharge();
        ResetEffectDuration();
    }

    #endregion

    public ActiveSkill()
    {
        _skillDamageMultiplier = 1.0f;
        _startEffectDuration = 0;
        _maxSkillCharge = 0;
        _chargeGainPerTurn = 0;
        _freezeSkillCharge = false;
        _unit = null;
    }
}
