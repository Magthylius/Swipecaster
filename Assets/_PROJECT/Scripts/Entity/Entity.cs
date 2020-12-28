using System;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

public abstract class Entity : MonoBehaviour
{
    #region Variable Definitions

    [Header("General")]
    [Range(1, 50), SerializeField] private int _currentLevel;
    [SerializeField] private int _totalHealth;
    [SerializeField] private int _totalAttack;
    [SerializeField] private int _totalDefence;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _currentAttack;
    [SerializeField] private int _currentDefence;
    private int _currentRarity;

    [Header("Attributes")]
    private RuneType _runeType;
    [SerializeField] private UnitObject baseUnit;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] private float baseStatMultiplier = 1/3f;
    [SerializeField] private float paraCapMultiplier = 0.6f;
    [SerializeField] private float baseStatCapMultiplier = 0.4f;

    #endregion

    #region Public Methods

    public int GetBaseAttack => _totalAttack;
    public int GetCurrentAttack => _currentAttack;
    public void AddCurrentAttack(int amount) => SetCurrentAttack(_currentDefence + amount);
    public void SetCurrentAttack(int amount) => _currentAttack = Mathf.Clamp(amount, 0, int.MaxValue);
    public int GetBaseDefence => _totalDefence;
    public int GetCurrentDefence => _currentDefence;
    public void AddCurrentDefence(int amount) => SetCurrentDefence(_currentDefence + amount);
    public void SetCurrentDefence(int amount) => _currentDefence = Mathf.Clamp(amount, 1, int.MaxValue);
    public int GetMaxHealth => _totalHealth;
    public int GetCurrentHealth => _currentHealth;
    public void AddCurrentHealth(int amount) => SetCurrentHealth(GetCurrentHealth + amount);
    public virtual void SetCurrentHealth(int amount) => _currentHealth = Mathf.Clamp(amount, 0, GetMaxHealth);

    public int GetCurrentLevel => _currentLevel;
    public void SetCurrentLevel(int amount) { _currentLevel = amount; CalculateActualStats(); }

    public UnitObject BaseUnit => baseUnit;

    public void setBaseUnit (UnitObject newUnit)
    {
        baseUnit = newUnit;
    }

    public void SetRuneType(RuneType type) => _runeType = type;
    public RuneType GetRuneType => _runeType;

    #endregion

    #region Protected Virtual Methods

    protected virtual void Awake()
    {
       /* Initialise();
        _currentHealth = _totalHealth;
        _currentAttack = _totalAttack;
        _currentDefence = _totalDefence;*/
    }

    protected virtual void OnValidate() => CalculateActualStats();

    #endregion

    #region Protected Methods

    public void Initialise()
    {
        _currentLevel = 1;
        _currentRarity = baseUnit.BaseRarity;
        CalculateActualStats();
    }

    protected void UpdateCalculatedStats()
    {
        SetCurrentAttack(GetBaseAttack);
        SetCurrentDefence(GetBaseDefence);
        SetCurrentHealth(GetMaxHealth);
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

        UpdateCalculatedStats();
    }

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
}