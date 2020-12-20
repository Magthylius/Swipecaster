using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected UnitObject baseUnit;
    [Range(1, 50), SerializeField] protected int _currentLevel;
    protected int _currentRarity;
    [SerializeField] protected int _totalHealth;
    [SerializeField] protected int _totalAttack;
    [SerializeField] protected int _totalDefence;
    protected int _currentHealth;
    protected RuneType _runeType;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] protected float baseStatMultiplier;
    [SerializeField] protected float paraCapMultiplier;
    [SerializeField] protected float baseStatCapMultiplier;

    [Header("Action Events")]
    protected static Action _deathEvent;
    protected Action<Entity, int> _grazeEvent;
    protected Action<Entity, int> _hitEvent;
    protected Action _turnBegin;
    protected Action _turnEnd;

    #region Public Abstract Methods

    public abstract void TakeDamage(Entity damager, int damageAmount);
    public abstract void RecieveHealing(Entity healer, int healAmount);
    public abstract void DoDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes);
    public abstract int CalculateDamage(Entity focusTarget, List<Entity> affectedTargets, RuneCollection runes);
    public abstract List<Entity> GetAffectedTargets(Entity focusTarget, List<Entity> allEntities);

    #endregion

    #region Public Virtual Methods

    public virtual int GetAttack => _totalAttack;
    public virtual int GetDefence => _totalDefence;
    public virtual int GetMaxHealth => _totalHealth;
    public virtual int GetCurrentHealth => _currentHealth;

    #endregion

    #region Public Static Methods

    public static void SubscribeDeathEvent(Action method) => _deathEvent += method;
    public static void UnsubscribeDeathEvent(Action method) => _deathEvent -= method;

    #endregion

    #region Public Methods

    public void SubscribeGrazeEvent(Action<Entity, int> method) => _grazeEvent += method;
    public void UnsubscribeGrazeEvent(Action<Entity, int> method) => _grazeEvent -= method;
    public void SubscribeHitEvent(Action<Entity, int> method) => _hitEvent += method;
    public void UnsubscribeHitEvent(Action<Entity, int> method) => _hitEvent -= method;
    public void SubscribeTurnBeginEvent(Action method) => _turnBegin += method;
    public void UnsubscribeTurnBeginEvent(Action method) => _turnBegin -= method;
    public void SubscribeTurnEndEvent(Action method) => _turnEnd += method;
    public void UnsubscribeTurnEndEvent(Action method) => _turnEnd -= method;

    #endregion

    #region Protected Virtual Methods

    protected virtual void Awake()
    {
        _turnBegin = null;
        _turnEnd = null;
    }

    protected virtual void OnValidate() => CalculateActualStats();

    #endregion

    #region Protected Methods

    protected void Initialise()
    {
        _currentLevel = 1;
        _currentRarity = baseUnit.BaseRarity;
        CalculateActualStats();
    }

    protected void CalculateActualStats()
    {
        if (baseUnit == null) return;

        StatInfo currentInfo;

        //! Health
        currentInfo = GenerateStatInfo(baseUnit.MaxHealth);
        _totalHealth = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));

        //! Attack
        currentInfo = GenerateStatInfo(baseUnit.MaxAttack);
        _totalAttack = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));

        //! Defence
        currentInfo = GenerateStatInfo(baseUnit.MaxDefence);
        _totalDefence = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));
    }

    #endregion

    #region Private Virtual Methods



    #endregion

    #region Private Methods

    private StatInfo GenerateStatInfo(float maxCap)
        => new StatInfo(maxCap * baseStatMultiplier, maxCap * paraCapMultiplier, maxCap * baseStatCapMultiplier);

    private float CalculateLevelParabolicStat(StatInfo info)
    {
        int actualLevel = Convert.ToInt32(_currentLevel) - Convert.ToInt32(baseUnit.MaxLevel);
        float paraGradient = info.paraCap / -(baseUnit.MaxLevel * baseUnit.MaxLevel);
        return paraGradient * (actualLevel * actualLevel) + info.paraCap;
    }

    private float CalculateLevelLinearStat(StatInfo info)
    {
        float linearGradient = (info.baseStatCap - info.baseStat) / baseUnit.MaxLevel;
        return linearGradient * _currentLevel + info.baseStat;
    }

    #endregion

    #region Shorthands

    public void AddHealth(int amount) => _currentHealth += amount;
    public void SetHealth(int amount) => _currentHealth = amount;
    public RuneType GetRuneType => _runeType;
    public UnitObject BaseUnit => baseUnit;

    #endregion
}

public struct StatInfo
{
    public float baseStat;
    public float paraCap;
    public float baseStatCap;

    public StatInfo(float baseStat, float paraCap, float baseStatCap)
    {
        this.baseStat = baseStat;
        this.paraCap = paraCap;
        this.baseStatCap = baseStatCap;
    }
};