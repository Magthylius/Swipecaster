using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Flame : StatusEffect
{
    private float _maxHealthPercent;
    private float _attackDownPercent;
    public float MaxHealthPercent => Mathf.Abs(_maxHealthPercent);
    public float AttackDownPercent => Mathf.Abs(_attackDownPercent);
    public override string StatusName => "Aflame";

    public override void DoPreEffect(Entity target)
    {
        if (ShouldClear()) return;
        int attackToDeduct = Mathf.Abs(Round(target.GetBaseAttack * AttackDownPercent));
        target.AddCurrentAttack(-attackToDeduct);
    }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target)
    {
        int hpToDeduct = Mathf.Abs(Round(target.GetMaxHealth * MaxHealthPercent));
        target.AddHealth(-hpToDeduct);

        DeductRemainingTurns();
    }
    public override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<Flame>().Count();

    public Flame() : base()
    {
        _maxHealthPercent = 0.0f;
        _attackDownPercent = 0.0f;
    }
    public Flame(int turns, float baseResistance, bool isPermanent, float maxHealthPercent, float atkDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _maxHealthPercent = Mathf.Clamp01(maxHealthPercent);
        _attackDownPercent = Mathf.Clamp01(atkDownPercent);
    }
}