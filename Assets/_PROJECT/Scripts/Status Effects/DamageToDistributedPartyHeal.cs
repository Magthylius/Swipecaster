using UnityEngine;

public class DamageToDistributedPartyHeal : EmptyStatus<DamageToDistributedPartyHeal>
{
    #region Variables and Properties

    private float _healPercent;
    public int HealAmountPer(int totalDamage, int count) => Round(totalDamage * _healPercent) / count;
    public override string StatusName => "On Damaged Distributed Party Heal";

    #endregion

    #region Override Methods

    public override void DoOnHitEffect(TargetInfo info, int totalDamage)
    {
        if (info.Allies == null) return;

        var healAmountPer = HealAmountPer(totalDamage, info.Allies.Count);
        info.Allies.ForEach(caster => caster.RecieveHealing(_unit, healAmountPer));
    }

    #endregion

    public DamageToDistributedPartyHeal() : base()
    {
        _healPercent = 0.0f;
    }
    public DamageToDistributedPartyHeal(int turns, float baseResistance, bool isPermanent, float healPercent) : base(turns, baseResistance, isPermanent)
    {
        _healPercent = Mathf.Abs(healPercent);
    }
}