using System.Linq;
using UnityEngine;

public class AttackToPartyHeal : EmptyStatus<AttackToPartyHeal>
{
    #region Variables and Properties

    private float _healPercent;
    public int HealAmount(int totalDamage) => Round(totalDamage * _healPercent);
    public override string StatusName => "Heal Party with Attack";

    #endregion

    #region Override Methods

    public override void DoEffectOnAction(TargetInfo info, int totalDamage)
        => info.Allies.ToList().ForEach(caster => caster.RecieveHealing(_unit, HealAmount(totalDamage)));

    #endregion

    public AttackToPartyHeal() : base()
    {
        _healPercent = 0.0f;
    }
    public AttackToPartyHeal(int turns, float baseResistance, bool isPermanent, float healPercent) : base(turns, baseResistance, isPermanent)
    {
        _healPercent = Mathf.Abs(healPercent);
    }
}