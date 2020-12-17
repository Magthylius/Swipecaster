using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private UnitObject baseUnit;
    [Range(1, 50), SerializeField] private int _currentLevel;
    private int _currentRarity;
    [SerializeField] private int _totalHealth;
    [SerializeField] private int _totalAttack;
    [SerializeField] private int _totalDefence;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] private float baseStatMultiplier;
    [SerializeField] private float paraCapMultiplier;
    [SerializeField] private float baseStatCapMultiplier;

    private void Awake()
    {
        
    }

    private void Initialise()
    {
        _currentLevel = 1;
        _currentRarity = baseUnit.BaseRarity;
        CalculateActualStats();
    }

    private void OnValidate()
    {
        CalculateActualStats();
    }

    private void CalculateActualStats()
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

    private struct StatInfo
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
}
