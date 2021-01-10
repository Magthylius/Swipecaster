using ConversionFunctions;
using ClampFunctions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public abstract class Unit : Entity
{
    #region Variable Definitions

    //! Attributes
    [Header("Information")]
    protected const int PartySize = 4;
    private AttackStatus _attackStatus = AttackStatus.Normal;
    private Projectile _currentProjectile;
    private Projectile _defaultProjectile;
    private List<StatusEffect> _statusEffects;
    private bool _projectileLock = false;
    private bool _undying = false;
    [SerializeField] private bool isPlayer;
    [SerializeField] private int _totalDamageInTurn = 0;

    [Header("Other Multipliers")]
    [SerializeField] protected float damageMultiplier = 1.0f;

    [Header("Recovery")]
    [SerializeField] protected int defaultPassiveHealAmount = 0;
    protected int currentPassiveHealAmount = 0;

    [SerializeField] protected float defaultPassiveHealPercent = 0.0f;
    protected float currentPassiveHealPercent = 0.0f;

    [Header("Reflection and Deflection")]
    [SerializeField] protected float defaultReboundPercent = 0.2f;
    protected float currentReboundPercent = 0.2f;

    [Header("RNG")]
    [SerializeField, Range(0.0f, 1.0f)] protected float defaultProbability = 0.05f;
    protected float currentProbability = 0.05f;
    [SerializeField] private int basePriority = 1;
    private int currentPriority = 0;

    [Header("Skills")]
    [SerializeField] private ActiveSkill _activeSkill;

    [Header("UI")]
    [SerializeField] private DamagePopUp damagePopUp;

    [Header("Action Events")]
    private static Action<Unit> _deathEvent;
    private static Action _allTurnBegin;
    private static Action _allTurnEnd;
    private Action<Unit, int> _grazeEvent;
    private Action<Unit, int> _hitEvent;
    private Action<Unit> _healthChangeEvent;
    private Action<Unit, ActiveSkill> _useSkillEvent;
    private Action _selfTurnBegin;
    private Action _selfTurnEnd;

    #endregion

    #region Public Abstract Methods

    public abstract void UseSkill(TargetInfo targetInfo, BattlestageManager battleStage);
    public abstract void TakeHit(Unit damager, int damageAmount);
    public abstract void DoAction(TargetInfo targetInfo, RuneCollection runes);
    public abstract int CalculateDamage(TargetInfo targetInfo, RuneCollection runes);
    public abstract TargetInfo GetAffectedTargets(TargetInfo targetInfo);

    #endregion

    #region Public Virtual Methods

    public virtual void RecieveHealing(Unit healer, int healAmount)
    {
        if (healAmount <= 0) return;
        AddCurrentHealth(healAmount);
        DamagePopUp(healAmount, false);
    }

    #endregion

    #region Public Override Methods

    public override void SetCurrentHealth(int amount)
    {
        if (GetUndyingStatus && amount <= 0) amount = 1;
        base.SetCurrentHealth(amount);
        InvokeHealthChangeEvent(this);
    }

    public override void Suicide()
    {
        DeactivateUndying();
        base.Suicide();
    }

    #endregion

    #region Public Methods

    #region Unit Info

    public int GetTotalDamageInTurn => _totalDamageInTurn;

    public void SetIsPlayer(bool statement) => isPlayer = statement;
    public bool GetIsPlayer => isPlayer;

    public void SetAttackStatus(AttackStatus status) => _attackStatus = status;
    public AttackStatus GetAttackStatus => _attackStatus;

    #endregion

    #region Projectile

    public void SetProjectileLock(bool statement) => _projectileLock = statement;
    public void SetDefaultProjectile(Projectile p)
    {
        if (_projectileLock) return;
        _defaultProjectile = p;
    }
    public void SubstituteProjectile(Projectile p)
    {
        if (_projectileLock) return;
        _currentProjectile = p;
    }
    public void ResetProjectile()
    {
        if (_projectileLock) return;
        _currentProjectile = GetDefaultProjectile;
    }
    public Projectile GetCurrentProjectile => _currentProjectile;
    private Projectile GetDefaultProjectile => _defaultProjectile;

    #endregion

    #region Status Effects

    public void UpdateStatusEffects()
    {
        ResetAllStats();
        GetStatusEffects.ForEach(j => j.UpdateStatus());
    }
    public void AddStatusEffect(StatusEffect status)
    {
        if (status == null) return;
        status.SetUnit(this);
        _statusEffects.Add(status);
        status.DoImmediateEffect(GetBattleStageInfo());
    }
    public void RemoveAllStatusEffectsOfType(Type effectType)
    {
        for (int i = _statusEffects.Count - 1; i >= 0; i--)
        {
            var effect = _statusEffects[i];
            if (effect.GetType() == effectType) _statusEffects[i].InvokeSelfDestructEvent();
        }
    }
    public void RemoveAllStatusEffects() => GetStatusEffects.ForEach(status => status.InvokeSelfDestructEvent());
    public List<StatusEffect> GetStatusEffects => StatusEffectsNullOrEmpty ? StatusEffect.NullList : _statusEffects;

    #endregion

    #region Active Skill

    public void ResetSkillCharge() => GetActiveSkill?.ResetSkillCharge();
    public int GetMaxSkillChargeCount => HasActiveSkill ? GetActiveSkill.GetMaxSkillCharge : 0;
    public int GetCurrentSkillChargeCount => HasActiveSkill ? GetActiveSkill.GetCurrentSkillCharge : 0;
    public bool SkillIsReady => HasActiveSkill ? GetActiveSkill.SkillChargeReady : false;
    public bool HasActiveSkill => _activeSkill != null;
    public ActiveSkill GetActiveSkill => _activeSkill;
    public void SetActiveSkill(ActiveSkill skill) { _activeSkill = skill; ResetSkillCharge(); }

    #endregion

    #region Misc Stats

    public int GetPassiveHealAmount => currentPassiveHealAmount;
    public void SetPassiveHealAmount(int amount) => currentPassiveHealAmount = amount;
    public void ResetPassiveHealAmount() => currentPassiveHealAmount = defaultPassiveHealAmount;

    public float GetPassiveHealPercent => currentPassiveHealPercent;
    public void SetPassiveHealPercent(float percent) => currentPassiveHealPercent = percent;
    public void ResetPassiveHealPercent() => currentPassiveHealPercent = defaultPassiveHealPercent;

    public float GetReboundPercent => currentReboundPercent;
    public void SetReboundPercent(float percent) => currentReboundPercent = percent;
    public void ResetReboundPercent() => currentReboundPercent = defaultReboundPercent;

    public float GetProbability => currentProbability;
    public void SetProbability(float probability) => currentProbability = probability;
    public void ResetProbability() => currentProbability = defaultProbability;

    public int GetUnitPriority => currentPriority;
    public void SetUnitPriority(int newPriority) => currentPriority = newPriority;
    public void ResetPriority() => currentPriority = basePriority;

    public bool GetUndyingStatus => _undying;
    public void ActivateUndying() => _undying = true;
    public void DeactivateUndying() => _undying = false;

    #endregion

    #region Events

    public static void SubscribeDeathEvent(Action<Unit> method) => _deathEvent += method;
    public static void UnsubscribeDeathEvent(Action<Unit> method) => _deathEvent -= method;
    public static void InvokeDeathEvent(Unit a) => _deathEvent?.Invoke(a);

    public static void SubscribeAllTurnBeginEvent(Action method) => _allTurnBegin += method;
    public static void UnsubscribeAllTurnBeginEvent(Action method) => _allTurnBegin -= method;
    public static void InvokeAllTurnBeginEvent() => _allTurnBegin?.Invoke();

    public static void SubscribeAllTurnEndEvent(Action method) => _allTurnEnd += method;
    public static void UnsubscribeAllTurnEndEvent(Action method) => _allTurnEnd -= method;
    public static void InvokeAllTurnEndEvent() => _allTurnEnd?.Invoke();

    public void SubscribeGrazeEvent(Action<Unit, int> method) => _grazeEvent += method;
    public void UnsubscribeGrazeEvent(Action<Unit, int> method) => _grazeEvent -= method;
    public void InvokeGrazeEvent(Unit a, int b) => _grazeEvent?.Invoke(a, b);

    public void SubscribeHitEvent(Action<Unit, int> method) => _hitEvent += method;
    public void UnsubscribeHitEvent(Action<Unit, int> method) => _hitEvent -= method;
    public void InvokeHitEvent(Unit a, int b) => _hitEvent?.Invoke(a, b);

    public void SubscribeHealthChangeEvent(Action<Unit> method) => _healthChangeEvent += method;
    public void UnsubscribeHealthChangeEvent(Action<Unit> method) => _healthChangeEvent -= method;
    public void InvokeHealthChangeEvent(Unit a) => _healthChangeEvent?.Invoke(a);

    public void SubscribeUseSkillEvent(Action<Unit, ActiveSkill> method) => _useSkillEvent += method;
    public void UnsubscribeUseSkillEvent(Action<Unit, ActiveSkill> method) => _useSkillEvent -= method;
    public void InvokeUseSkillEvent(Unit a, ActiveSkill b) => _useSkillEvent?.Invoke(a, b);

    public void SubscribeSelfTurnBeginEvent(Action method) => _selfTurnBegin += method;
    public void UnsubscribeSelfTurnBeginEvent(Action method) => _selfTurnBegin -= method;
    public void InvokeSelfTurnBeginEvent() => _selfTurnBegin?.Invoke();

    public void SubscribeSelfTurnEndEvent(Action method) => _selfTurnEnd += method;
    public void UnsubscribeSelfTurnEndEvent(Action method) => _selfTurnEnd -= method;
    public void InvokeSelfTurnEndEvent() => _selfTurnEnd?.Invoke();

    #endregion

    #endregion

    #region Protected Virtual Methods

    protected virtual void TakeDamage(Unit damager, int damageAmount)
    {
        if (damager.GetAttackStatus != AttackStatus.Normal) return;

        float statusInMultiplier = 1.0f;
        statusInMultiplier += GetStatusEffects.Sum(status => status.GetStatusDamageInModifier());

        _totalDamageInTurn = (Mathf.Abs(Round(damageAmount * statusInMultiplier)) - GetCurrentDefence).Clamp0();
        AddCurrentHealth(-_totalDamageInTurn);
        GetStatusEffects.ForEach(i => i.DoOnHitEffect(damager, GetBattleStageInfo(), _totalDamageInTurn));

        DamagePopUp(_totalDamageInTurn, true);
    }

    protected virtual void StartTurnMethods()
    {
        InvokeAllTurnBeginEvent();
        ResetTotalDamageInTurn();
    }

    protected virtual void EndTurnMethods()
    {
        InvokeAllTurnEndEvent();
        ResetAttackStatus();
        PostStatusEffect();
        ResetProjectile();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeHitEvent(TakeDamage);
        UnsubscribeHealthChangeEvent(CheckDeathEvent);
        UnsubscribeUseSkillEvent(UpdateStatusEffectsOnSkill);
        UnsubscribeSelfTurnEndEvent(EndTurnMethods);
        UnsubscribeSelfTurnBeginEvent(StartTurnMethods);
        RemoveAllStatusEffects();
    }

    protected virtual void Start()
    {
        HandleActiveSkill();
        ResetAllStats();
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        _statusEffects = new List<StatusEffect>();

        GetDamagePopUp();
        SetDefaultProjectile(new CrowFlies());
        ResetProjectile();
        SubscribeHitEvent(TakeDamage);
        SubscribeHealthChangeEvent(CheckDeathEvent);
        SubscribeUseSkillEvent(UpdateStatusEffectsOnSkill);
        SubscribeSelfTurnEndEvent(EndTurnMethods);
        SubscribeSelfTurnBeginEvent(StartTurnMethods);
    }

    #endregion

    #region Protected Methods

    protected bool ProbabilityHit => Random.Range(0.0f, 1.0f - float.Epsilon) < currentProbability;
    protected int Round(float number) => Mathf.RoundToInt(number);
    protected int ToInt(bool statement) => Convert.ToInt32(statement);
    protected void ResetAtkDefStats()
    {
        SetCurrentAttack(GetBaseAttack);
        SetCurrentDefence(GetBaseDefence);
    }
    protected void ResetAllStats()
    {
        ResetAtkDefStats();
        ResetAttackStatus();
        ResetPriority();
        ResetPassiveHealAmount();
        ResetPassiveHealPercent();
        ResetReboundPercent();
        ResetProbability();
    }
    protected void ResetAllEffects()
    {
        ResetAllStats();
        RemoveAllStatusEffects();
        ResetProjectile();
    }

    #endregion

    #region Private Methods

    private void GetDamagePopUp() => damagePopUp = GetComponentInChildren<DamagePopUp>();
    private void DamagePopUp(int damage, bool isDamage)
    {
        if (damagePopUp == null) return;
        Color textColour = isDamage ? Color.red : Color.green;
        damagePopUp.transform.parent.gameObject.SetActive(true);
        damagePopUp.SetTextColour(textColour);
        damagePopUp.ShowDamage(damage);
    }

    private void ResetAttackStatus() => SetAttackStatus(AttackStatus.Normal);
    private void CheckDeathEvent(Unit unit)
    {
        if (GetCurrentHealth <= 0) InvokeDeathEvent(unit);
    }
    private void UpdateStatusEffectsOnSkill(Unit unit, ActiveSkill skill) => UpdateStatusEffects();
    private void PostStatusEffect() => GetStatusEffects?.ForEach(i => i.DoPostEffect());

    private TargetInfo GetBattleStageInfo()
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return TargetInfo.Null;
        return new TargetInfo(battleStage.GetSelectedTarget().AsUnit(), null, null, battleStage.GetCasterTeamAsUnit(), battleStage.GetEnemyTeamAsUnit());
    }
    private void ResetTotalDamageInTurn() => _totalDamageInTurn = 0;
    private bool StatusEffectsNullOrEmpty => _statusEffects == null || _statusEffects.Count == 0;
    
    private void HandleActiveSkill()
    {
        var skill = GetBaseUnit.GetUnitActiveSkill(this);
        if (skill == null) return;
        SetActiveSkill(skill);
    }

    #endregion
}

public enum AttackStatus
{
    Normal = 0,
    Deflected,
    Reflected
}