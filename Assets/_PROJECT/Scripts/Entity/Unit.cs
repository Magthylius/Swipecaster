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

    [Header("Other Multipliers")]
    [SerializeField] protected float damageMultiplier = 1.0f;

    [Header("Recovery")]
    [SerializeField] protected int passiveHealAmount = 0;
    [SerializeField] protected float damageToHealPercent = 0.0f;

    [Header("Reflection and Deflection")]
    [SerializeField] protected float reboundPercent = 0.2f;

    [Header("RNG")]
    [SerializeField, Range(0.0f, 1.0f)] protected float probability = 0.05f;
    [SerializeField] private int priority = 1;
    protected const int PartySize = 4;

    [Header("Skills")]
    [SerializeField] private int currentSkillCharge;
    [SerializeField] private int _skillChargeCount;
    private ActiveSkill _activeSkill;

    [Header("Action Events")]
    private static Action<Unit> _deathEvent;
    private Action<Unit, int> _grazeEvent;
    private Action<Unit, int> _hitEvent;
    private Action<Unit> _healthChangeEvent;
    private Action<Unit, ActiveSkill> _useSkillEvent;
    private Action _turnBegin;
    private Action _turnEnd;

    #endregion

    #region Public Abstract Methods

    public abstract void UseSkill(TargetInfo targetInfo, StageInfo stageInfo);
    public abstract void TakeHit(Unit damager, int damageAmount);
    public abstract void RecieveHealing(Unit healer, int healAmount);
    public abstract void DoAction(TargetInfo targetInfo, RuneCollection runes);
    public abstract int CalculateDamage(TargetInfo targetInfo, RuneCollection runes);
    public abstract TargetInfo GetAffectedTargets(Unit focusTarget, List<Unit> allEntities);

    #endregion

    #region Public Override Methods

    public override void SetCurrentHealth(int amount)
    {
        base.SetCurrentHealth(amount);
        InvokeHealthChangeEvent(this);
    }

    #endregion

    #region Public Methods

    public void SetAttackStatus(AttackStatus status) => _attackStatus = status;
    public AttackStatus GetAttackStatus => _attackStatus;

    public void SetProjectile(Projectile p) => _projectile = p;
    public Projectile GetProjectile => _projectile;

    public void AddStatusEffect(StatusEffect status) => _statusEffects.Add(status);
    public void RemoveAllStatusEffectsOfType(Type effectType)
    {
        for (int i = _statusEffects.Count - 1; i >= 0; i--)
        {
            var effect = _statusEffects[i];
            if (effect.GetType() == effectType) _statusEffects.RemoveAt(i);
        }
    }
    public List<StatusEffect> GetStatusEffects => _statusEffects;

    public void SyncSkillChargeCount() => _skillChargeCount = HasActiveSkill ? GetActiveSkill.GetMaxSkillCharge : 0;
    public void ResetSkillCharge() => currentSkillCharge = 0;
    public void AddSkillCharge()
        => currentSkillCharge = Mathf.Clamp(currentSkillCharge += HasActiveSkill ? GetActiveSkill.GetChargeGainPerTurn : 0, 0, _skillChargeCount);
    public int GetMaxSkillChargeCount => _skillChargeCount;
    public int GetCurrentSkillChargeCount => currentSkillCharge;
    public bool SkillIsReady => currentSkillCharge == _skillChargeCount;
    public ActiveSkill GetActiveSkill => _activeSkill;
    public void SetActiveSkill(ActiveSkill skill) { _activeSkill = skill; SyncSkillChargeCount(); ResetSkillCharge(); }
    public bool HasActiveSkill => _activeSkill != null;

    public int GetUnitPriority => priority;
    public void SetUnitPriority(int newPriority) => priority = newPriority;

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
        
        AddCurrentHealth(-Mathf.Abs(Round(damageAmount * statusInMultiplier)));
        GetStatusEffects.ForEach(i => i.DoOnHitEffect(this));

        if (gameObject.transform.Find("DamagePopUp"))
        {
            gameObject.transform.Find("DamagePopUp").GetComponentInChildren<DamagePopUp>().showDamage(Mathf.Abs(Round(damageAmount * statusInMultiplier)));
        }

       // gameObject.transform.GetChild(0).GetComponent<DamagePopUp>().showDamage(Mathf.Abs(Round(damageAmount * statusInMultiplier)));
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeHitEvent(TakeDamage);
        UnsubscribeHealthChangeEvent(CheckDeathEvent);
        UnsubscribeUseSkillEvent(UpdateStatusEffectsOnSkill);
        UnsubscribeTurnEndEvent(EndTurnMethods);
        UnsubscribeTurnBeginEvent(UpdatePreStatusEffects);
    }

    #endregion

    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        _statusEffects = new List<StatusEffect>();

        SetProjectile(new CrowFlies());
        SubscribeHitEvent(TakeDamage);
        SubscribeHealthChangeEvent(CheckDeathEvent);
        SubscribeUseSkillEvent(UpdateStatusEffectsOnSkill);
        SubscribeTurnEndEvent(EndTurnMethods);
        SubscribeTurnBeginEvent(UpdatePreStatusEffects);
    }

    #endregion

    #region Protected Methods

    protected bool ProbabilityHit => Random.Range(0.0f, 1.0f - float.Epsilon) < probability;
    protected int Round(float number) => Mathf.RoundToInt(number);
    protected int ToInt(bool statement) => Convert.ToInt32(statement);
    protected void ResetAtkDefStats()
    {
        SetCurrentAttack(GetBaseAttack);
        SetCurrentDefence(GetBaseDefence);
    }
    protected void ResetAllEffects()
    {

    }

    #endregion

    #region Private Methods

    private void EndTurnMethods()
    {
        ResetAttackStatus();
        PostStatusEffect();
        HandleSkillCount();
    }
    private void ResetAttackStatus() => SetAttackStatus(AttackStatus.Normal);
    private void CheckDeathEvent(Unit unit)
    {
        if (GetCurrentHealth <= 0) InvokeDeathEvent(unit);
    }
    private void UpdateStatusEffectsOnSkill(Unit unit, ActiveSkill skill) => UpdatePreStatusEffects();
    private void UpdatePreStatusEffects()
    {
        ResetAtkDefStats();

        for (int i = GetStatusEffects.Count - 1; i >= 0; i--)
        {
            if (GetStatusEffects[i].ShouldClear()) GetStatusEffects.RemoveAt(i);
        }

        GetStatusEffects.ForEach(j => j.DoPreEffect(this));
    }
    private void PostStatusEffect() => GetStatusEffects.ForEach(i => i.DoPostEffect(this));
    private void HandleSkillCount() => AddSkillCharge();

    #endregion
}

public enum AttackStatus
{
    Normal = 0,
    Deflected,
    Reflected
}