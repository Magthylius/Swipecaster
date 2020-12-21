using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("General")]
    [Range(1, 50), SerializeField] protected int _currentLevel;
    [SerializeField] protected int _totalHealth;
    [SerializeField] protected int _totalAttack;
    [SerializeField] protected int _totalDefence;
    protected int _currentHealth;
    protected int _currentAttack;
    protected int _currentDefence;
    protected int _currentRarity;

    [Header("Attributes")]
    protected RuneType _runeType;
    protected AttackStatus _attackStatus = AttackStatus.Normal;
    protected Projectile _projectile;
    protected List<StatusEffect> _statusEffects;
    [SerializeField] protected UnitObject baseUnit;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] protected float baseStatMultiplier = 1/3f;
    [SerializeField] protected float paraCapMultiplier = 0.6f;
    [SerializeField] protected float baseStatCapMultiplier = 0.4f;

    [Header("Action Events")]
    protected static Action _deathEvent;
    protected Action<Entity, int> _grazeEvent;
    protected Action<Entity, int> _hitEvent;
    protected Action _turnBegin;
    protected Action _turnEnd;

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

    public static void SubscribeDeathEvent(Action method) => _deathEvent += method;
    public static void UnsubscribeDeathEvent(Action method) => _deathEvent -= method;
    public static void InvokeDeathEvent() => _deathEvent?.Invoke();

    #endregion

    #region Public Methods

    public void SubscribeGrazeEvent(Action<Entity, int> method) => _grazeEvent += method;
    public void UnsubscribeGrazeEvent(Action<Entity, int> method) => _grazeEvent -= method;
    public void InvokeGrazeEvent(Entity a, int b) => _grazeEvent?.Invoke(a, b);

    public void SubscribeHitEvent(Action<Entity, int> method) => _hitEvent += method;
    public void UnsubscribeHitEvent(Action<Entity, int> method) => _hitEvent -= method;
    public void InvokeHitEvent(Entity a, int b) => _hitEvent?.Invoke(a, b);

    public void SubscribeTurnBeginEvent(Action method) => _turnBegin += method;
    public void UnsubscribeTurnBeginEvent(Action method) => _turnBegin -= method;
    public void InvokeTurnBeginEvent() => _turnBegin?.Invoke();

    public void SubscribeTurnEndEvent(Action method) => _turnEnd += method;
    public void UnsubscribeTurnEndEvent(Action method) => _turnEnd -= method;
    public void InvokeTurnEndEvent() => _turnEnd?.Invoke();

    public void SetAttackStatus(AttackStatus status) => _attackStatus = status;
    public AttackStatus AttackStatus => _attackStatus;

    public void SetProjectile(Projectile p) => _projectile = p;
    public Projectile Projectile => _projectile;

    public List<StatusEffect> StatusEffects => _statusEffects;

    #endregion

    #region Protected Virtual Methods

    protected virtual void Awake()
    {
        _turnBegin = null;
        _turnEnd = null;
        _statusEffects = new List<StatusEffect>();
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

public enum AttackStatus
{
    Normal = 0,
    Deflected,
    Reflected
}