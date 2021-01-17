using System.Linq;
using UnityEngine;

public abstract class Summon : Unit
{
    [SerializeField] private SummonObject baseSummon;

    public override SummonObject GetBaseSummon => baseSummon;
    public override string GetEntityName => baseSummon != null ? baseSummon.SummonName : name;
    public override int GetBaseAttack => GetBaseSummon.MaxAttack;
    public override int GetBaseDefence => GetBaseSummon.MaxDefence;
    public override int GetMaxHealth => GetBaseSummon.MaxHealth;

    public override void UseSkill(TargetInfo targetInfo, BattlestageManager battleStage) { }
    public override void TakeHit(Unit damager, int damageAmount) => InvokeHitEvent(damager, damageAmount);
    public override void DoAction(TargetInfo targetInfo, RuneCollection runes) { }
    public override int CalculateDamage(TargetInfo targetInfo, RuneCollection runes)
    {
        int totalDamage = GetCurrentAttack;
        float statusOutMultiplier = 1.0f;
        statusOutMultiplier += GetStatusEffects.Sum(status => status.GetStatusDamageOutModifier());
        return Mathf.Abs(Round(totalDamage * statusOutMultiplier));
    }
    public override TargetInfo GetAffectedTargets(TargetInfo targetInfo) => TargetInfo.Null;

    protected override void Start()
    {
        base.Start();
        GetUnitAudio().SetAudioData(GetBaseSummon.AudioPack);
        baseSummon.CalculateMaxStats(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        transform.parent.gameObject.SetActive(false);
    }
}
