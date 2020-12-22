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
    private AttackStatus _attackStatus = AttackStatus.Normal;
    private Projectile _projectile;
    private List<StatusEffect> _statusEffects;
    [SerializeField] private UnitObject baseUnit;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] private float baseStatMultiplier = 1/3f;
    [SerializeField] private float paraCapMultiplier = 0.6f;
    [SerializeField] private float baseStatCapMultiplier = 0.4f;

    [Header("Action Events")]
    private static Action<Entity> _deathEvent;
    private Action<Entity, int> _grazeEvent;
    private Action<Entity, int> _hitEvent;
    private Action _healthChangeEvent;
    private Action _turnBegin;
    private Action _turnEnd;

    #endregion

    #region Public Abstract Methods

    public abstract void TakeHit(Entity damager, int damageAmount);
    public abstract void RecieveHealing(Entity healer, int healAmount);
    public abstract void DoAction(TargetInfo targetInfo, RuneCollection runes);
    public abstract int CalculateDamage(TargetInfo targetInfo, RuneCollection runes);
    public abstract TargetInfo GetAffectedTargets(Entity focusTarget, List<Entity> allEntities);

    #endregion

    #region Public Virtual Methods

    public virtual int GetBaseAttack => _totalAttack;
    public virtual int GetCurrentAttack => _currentAttack;
    public virtual void AddCurrentAttack(int amount)
    {
        _currentAttack += amount;
        if (_currentAttack < 0) _currentAttack = 0;
    }
    public virtual int GetBaseDefence => _totalDefence;
    public virtual int GetCurrentDefence => _currentDefence;
    public virtual void AddCurrentDefence(int amount)
    {
        _currentDefence += amount;
        if (_currentDefence < 1) _currentDefence = 1;
    }
    public virtual int GetMaxHealth => _totalHealth;
    public virtual int GetCurrentHealth => _currentHealth;

    public virtual int GetCurrentLevel => _currentLevel;
    public virtual void SetCurrentLevel(int amount)
    {
        _currentLevel = amount;
        CalculateActualStats();
    }

    #endregion

    #region Public Static Methods

    public static void SubscribeDeathEvent(Action<Entity> method) => _deathEvent += method;
    public static void UnsubscribeDeathEvent(Action<Entity> method) => _deathEvent -= method;
    public static void InvokeDeathEvent(Entity a) => _deathEvent?.Invoke(a);

    #endregion

    #region Public Methods

    public UnitObject BaseUnit => baseUnit;

    public void AddHealth(int amount) => SetHealth(GetCurrentHealth + amount);
    public void SetHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(amount, 0, GetMaxHealth);
        InvokeHealthChangeEvent();
    }

    #region Events

    public void SubscribeGrazeEvent(Action<Entity, int> method) => _grazeEvent += method;
    public void UnsubscribeGrazeEvent(Action<Entity, int> method) => _grazeEvent -= method;
    public void InvokeGrazeEvent(Entity a, int b) => _grazeEvent?.Invoke(a, b);

    public void SubscribeHitEvent(Action<Entity, int> method) => _hitEvent += method;
    public void UnsubscribeHitEvent(Action<Entity, int> method) => _hitEvent -= method;
    public void InvokeHitEvent(Entity a, int b) => _hitEvent?.Invoke(a, b);

    public void SubscribeHealthChangeEvent(Action method) => _healthChangeEvent += method;
    public void UnsubscribeHealthChangeEvent(Action method) => _healthChangeEvent -= method;
    public void InvokeHealthChangeEvent() => _healthChangeEvent?.Invoke();

    public void SubscribeTurnBeginEvent(Action method) => _turnBegin += method;
    public void UnsubscribeTurnBeginEvent(Action method) => _turnBegin -= method;
    public void InvokeTurnBeginEvent() => _turnBegin?.Invoke();

    public void SubscribeTurnEndEvent(Action method) => _turnEnd += method;
    public void UnsubscribeTurnEndEvent(Action method) => _turnEnd -= method;
    public void InvokeTurnEndEvent() => _turnEnd?.Invoke();

    #endregion

    public void SetRuneType(RuneType type) => _runeType = type;
    public RuneType GetRuneType => _runeType;

    public void SetAttackStatus(AttackStatus status) => _attackStatus = status;
    public AttackStatus GetAttackStatus => _attackStatus;

    public void SetProjectile(Projectile p) => _projectile = p;
    public Projectile GetProjectile => _projectile;

    public void AddStatusEffect(StatusEffect status) => _statusEffects.Add(status);
    public void RemoveAllStatusEffectsOfType(Type effectType)
    {
        for(int i = _statusEffects.Count - 1; i >= 0; i--)
        {
            var effect = _statusEffects[i];
            if (effect.GetType() == effectType) _statusEffects.RemoveAt(i);
        }
    }
    public List<StatusEffect> GetStatusEffects => _statusEffects;

    #endregion

    #region Protected Virtual Methods

    protected virtual void Awake()
    {
        _turnBegin = null;
        _turnEnd = null;
        _currentHealth = _totalHealth;
        _currentAttack = _totalAttack;
        _currentDefence = _totalDefence;
        _statusEffects = new List<StatusEffect>();
    }

    protected virtual void OnValidate() => CalculateActualStats();

    protected virtual void ResetAtkDefStats()
    {
        _currentAttack = GetBaseAttack;
        _currentDefence = GetBaseDefence;
    }

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

public enum AttackStatus
{
    Normal = 0,
    Deflected,
    Reflected
}