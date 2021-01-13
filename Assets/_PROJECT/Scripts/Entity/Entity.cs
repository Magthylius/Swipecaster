using ConversionFunctions;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region Variable Definitions

    [Header("Configuration")]
    [SerializeField] private UnitObject baseUnit;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _currentAttack;
    [SerializeField] private int _currentDefence;
    
    #endregion

    #region Public (Virtual) Methods

    public virtual int GetBaseAttack => baseUnit.LevelTotalAttack;
    public int GetCurrentAttack => _currentAttack;
    public void AddCurrentAttack(int amount) => SetCurrentAttack(_currentAttack + amount);
    public void SetCurrentAttack(int amount) => _currentAttack = Mathf.Clamp(amount, 0, int.MaxValue);

    public virtual int GetBaseDefence => baseUnit.LevelTotalDefence;
    public int GetCurrentDefence => _currentDefence;
    public void AddCurrentDefence(int amount) => SetCurrentDefence(_currentDefence + amount);
    public void SetCurrentDefence(int amount) => _currentDefence = Mathf.Clamp(amount, 1, int.MaxValue);

    public virtual int GetMaxHealth => baseUnit.LevelTotalHealth;
    public int GetCurrentHealth => _currentHealth;
    public void AddCurrentHealth(int amount) => SetCurrentHealth(_currentHealth + amount);
    public virtual void SetCurrentHealth(int amount) => _currentHealth = Mathf.Clamp(amount, 0, GetMaxHealth);
    public float GetHealthRatio => GetMaxHealth > 0 ? GetCurrentHealth / GetMaxHealth.AsFloat() : 0.0f;
    public virtual void Suicide() => SetCurrentHealth(-1);

    public int GetCurrentLevel => baseUnit.CurrentLevel;
    public void SetCurrentLevel(int amount) => baseUnit.CurrentLevel = amount;
    public void SetCurrentLevelAndCalculate(int amount) { baseUnit.CurrentLevel = amount; CalculateActualStats(); }

    public UnitObject GetBaseUnit => baseUnit;
    public void SetBaseUnit(UnitObject newUnit) { baseUnit = newUnit; CalculateActualStats(); }

    public virtual SummonObject GetBaseSummon => null;
    public virtual string GetEntityName => baseUnit != null ? baseUnit.CharacterName : name;

    public void SetRuneType(RuneType type) => baseUnit.RuneAlignment = type;
    public RuneType GetRuneType => baseUnit.RuneAlignment;

    #endregion

    #region Protected Virtual Methods

    protected virtual void Awake() => UpdateCalculatedStats();
    protected virtual void OnValidate() => CalculateActualStats();
    protected virtual void CalculateActualStats()
    {
        if (baseUnit == null) return;
        baseUnit.CalculateActualStats();
        UpdateCalculatedStats();
    }

    #endregion

    #region Protected Methods

    protected void UpdateCalculatedStats()
    {
        if (baseUnit == null) return;
        SetCurrentAttack(GetBaseAttack);
        SetCurrentDefence(GetBaseDefence);
        SetCurrentHealth(GetMaxHealth);
    }

    #endregion
}