using ConversionFunctions;
using ClampFunctions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Unit : Entity
{
    #region Variable Definitions

    //! Attributes
    private AttackStatus _attackStatus = AttackStatus.Normal;
    private Projectile _projectile;
    private List<StatusEffect> _statusEffects;
    [SerializeField] private bool isPlayer;

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
    protected const int PartySize = 4;

    [Header("Skills")]
    [SerializeField] private int currentSkillCharge;
    [SerializeField] private int _skillChargeCount;
    [SerializeField] private ActiveSkill _activeSkill;

    [Header("UI")]
    [SerializeField] private DamagePopUp damagePopUp;

    [Header("Action Events")]
    private static Action<Unit> _deathEvent;
    private Action<Unit, int> _grazeEvent;
    private Action<Unit, int> _hitEvent;
    private Action<Unit> _healthChangeEvent;
    private Action<Unit, ActiveSkill> _useSkillEvent;
    private Action _turnBegin;
    private Action _turnEnd;

    [Header("Cache")]
    private int _totalDamageInTurn = 0;
    private bool _undying = false;

    #endregion

    #region Public Abstract Methods

    public abstract void UseSkill(TargetInfo targetInfo, BattlestageManager battleStage);
    public abstract void TakeHit(Unit damager, int damageAmount);
    public abstract void RecieveHealing(Unit healer, int healAmount);
    public abstract void DoAction(TargetInfo targetInfo, RuneCollection runes);
    public abstract int CalculateDamage(TargetInfo targetInfo, RuneCollection runes);
    public abstract TargetInfo GetAffectedTargets(TargetInfo targetInfo);

    #endregion

    #region Public Override Methods

    public override void SetCurrentHealth(int amount)
    {
        if (GetUndyingStatus && amount <= 0) amount = 1;
        base.SetCurrentHealth(amount);
        InvokeHealthChangeEvent(this);
    }

    #endregion

    #region Public Methods

    public void SetIsPlayer(bool statement) => isPlayer = statement;
    public bool GetIsPlayer => isPlayer;

    public void SetAttackStatus(AttackStatus status) => _attackStatus = status;
    public AttackStatus GetAttackStatus => _attackStatus;

    public void SetProjectile(Projectile p) => _projectile = p;
    public Projectile GetProjectile => _projectile;

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
            if (effect.GetType() == effectType) _statusEffects.RemoveAt(i);
        }
    }
    public List<StatusEffect> GetStatusEffects => _statusEffects;

    public void ResetSkillCharge() => GetActiveSkill?.ResetSkillCharge();
    public void AddSkillCharge() => GetActiveSkill?.IncreaseSkillCharge();
    public int GetMaxSkillChargeCount => GetActiveSkill != null ? GetActiveSkill.GetMaxSkillCharge : -1;
    public int GetCurrentSkillChargeCount => GetActiveSkill != null ? GetActiveSkill.GetCurrentSkillCharge : -1;
    public bool SkillIsReady => GetActiveSkill != null ? GetActiveSkill.SkillChargeReady : false;
    public ActiveSkill GetActiveSkill => _activeSkill;
    public void SetActiveSkill(ActiveSkill skill) { _activeSkill = skill; ResetSkillCharge(); }
    public bool HasActiveSkill => _activeSkill != null;

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

    public int GetTotalDamageInTurn => _totalDamageInTurn;

    public bool GetUndyingStatus => _undying;
    public void ActivateUndying() => _undying = true;
    public void DeactivateUndying() => _undying = false;

    #region Events

    public static void SubscribeDeathEvent(Action<Unit> method) => _deathEvent += method;
    public static void UnsubscribeDeathEvent(Action<Unit> method) => _deathEvent -= method;
    public static void InvokeDeathEvent(Unit a) => _deathEvent?.Invoke(a);

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

    public void SubscribeTurnBeginEvent(Action method) => _turnBegin += method;
    public void UnsubscribeTurnBeginEvent(Action method) => _turnBegin -= method;
    public void InvokeTurnBeginEvent() => _turnBegin?.Invoke();

    public void SubscribeTurnEndEvent(Action method) => _turnEnd += method;
    public void UnsubscribeTurnEndEvent(Action method) => _turnEnd -= method;
    public void InvokeTurnEndEvent() => _turnEnd?.Invoke();

    #endregion

    #endregion

    #region Protected Virtual Methods

    protected virtual void TakeDamage(Unit damager, int damageAmount)
    {
        if (damager.GetAttackStatus != AttackStatus.Normal) return;
        float statusInMultiplier = 1.0f;
        for (int i = 0; i < GetStatusEffects.Count; i++) statusInMultiplier += GetStatusEffects[i].GetStatusDamageInModifier();

        _totalDamageInTurn = (Mathf.Abs(Round(damageAmount * statusInMultiplier)) - GetCurrentDefence).Clamp0();
        AddCurrentHealth(-_totalDamageInTurn);
        GetStatusEffects.ForEach(i => i.DoOnHitEffect(GetBattleStageInfo(), _totalDamageInTurn));

        if (damagePopUp == null) return;
        damagePopUp.transform.parent.gameObject.SetActive(true);
        damagePopUp.ShowDamage(_totalDamageInTurn);
    }

    protected virtual void StartTurnMethods()
    {
        //UpdateStatusEffects();
        ResetTotalDamageInTurn();
    }

    protected virtual void EndTurnMethods()
    {
        ResetAttackStatus();
        PostStatusEffect();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeHitEvent(TakeDamage);
        UnsubscribeHealthChangeEvent(CheckDeathEvent);
        UnsubscribeUseSkillEvent(UpdateStatusEffectsOnSkill);
        UnsubscribeTurnEndEvent(EndTurnMethods);
        UnsubscribeTurnBeginEvent(StartTurnMethods);
    }

    protected virtual void Start() => ResetAllStats();

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        _statusEffects = new List<StatusEffect>();

        GetDamagePopUp();
        SetProjectile(new CrowFlies());
        SubscribeHitEvent(TakeDamage);
        SubscribeHealthChangeEvent(CheckDeathEvent);
        SubscribeUseSkillEvent(UpdateStatusEffectsOnSkill);
        SubscribeTurnEndEvent(EndTurnMethods);
        SubscribeTurnBeginEvent(StartTurnMethods);
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
        GetStatusEffects.Clear();
    }

    #endregion

    #region Private Methods
    private void GetDamagePopUp()
    {
        if(GetComponentInChildren<DamagePopUp>())
        {
            damagePopUp = GetComponentInChildren<DamagePopUp>();
        }
    }

    private void ResetAttackStatus() => SetAttackStatus(AttackStatus.Normal);
    private void CheckDeathEvent(Unit unit)
    {
        if (GetCurrentHealth <= 0) InvokeDeathEvent(unit);
    }
    private void UpdateStatusEffectsOnSkill(Unit unit, ActiveSkill skill) => UpdateStatusEffects();
    private void PostStatusEffect() => GetStatusEffects.ForEach(i => i.DoPostEffect());

    private TargetInfo GetBattleStageInfo()
    {
        var battleStage = BattlestageManager.instance;
        if (battleStage == null) return TargetInfo.Null;

        return new TargetInfo(battleStage.GetSelectedTarget().AsUnit(), null, null, (List<Unit>)battleStage.GetCasterTeamAsUnit(), (List<Unit>)battleStage.GetEnemyTeamAsUnit());
    }
    private void ResetTotalDamageInTurn() => _totalDamageInTurn = 0;

    #endregion
}

public enum AttackStatus
{
    Normal = 0,
    Deflected,
    Reflected
}