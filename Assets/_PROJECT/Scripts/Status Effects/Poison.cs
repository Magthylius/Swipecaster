using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    #region Variables and Properties

    private float _maxHealthPercent;
    private float _defenceDownPercent;
    public float MaxHealthPercent => Mathf.Abs(_maxHealthPercent);
    public float DefenceDownPercent => Mathf.Abs(_defenceDownPercent);
    public override string StatusName => "Poisoned";

    #endregion

    #region Override Methods

    public override void DoPreEffect(Entity target)
    {
        if (ShouldClear()) return;
        int defenceToDeduct = Mathf.Abs(Round(target.GetBaseDefence * DefenceDownPercent));
        target.AddCurrentDefence(-defenceToDeduct);
    }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target)
    {
        if (ShouldClear()) return;
        int hpToDeduct = Mathf.Abs(Round(target.GetMaxHealth * MaxHealthPercent));
        target.AddHealth(-hpToDeduct);

        DeductRemainingTurns();
    }
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<Poison>().Count();

    #endregion

    public Poison() : base()
    {
        _maxHealthPercent = 0.0f;
        _defenceDownPercent = 0.0f;
    }
    public Poison(int turns, float baseResistance, bool isPermanent, float maxHealthPercent, float defDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _maxHealthPercent = Mathf.Clamp01(maxHealthPercent);
        _defenceDownPercent = Mathf.Clamp01(defDownPercent);
    }
}